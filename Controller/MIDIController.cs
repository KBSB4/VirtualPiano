using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using System.Diagnostics;

namespace Controller
{
    public static class MIDIController
    {
        public static MidiFile OriginalMIDI { get; set; }
        private static MidiFile MIDI { get; set; }
        private static MidiFile MIDIPianoIsolated { get; set; }
        public static Note[] AllNotes { get; set; }
        public static MidiTimeSpan CurrentTick { get; set; }


        //Note PlayBack has events notesplaybackstarted and notesplaybackfinished

        /// <summary>
        /// Read MIDI File and get karoake MIDIs out of it as well.
        /// </summary>
        /// <param name="MIDIpath"></param>
        public static void OpenMidi(string MIDIpath)
        {
            OriginalMIDI = MidiFile.Read(MIDIpath);
            SongController.LoadSong();

            //In case of isolation: 
            /*
						//Get all tracks in the MIDI
						List<TrackChunk> trackChunks = Melanchall.DryWetMidi.Core.TrackChunkUtilities.GetTrackChunks(OriginalMIDI).ToList();

						//UNDONE Figure out which track is piano/sythensizer (might be impossible, must be done in database)
						//TrackChunk pianoTrackChunks = trackChunks[1];
						//MIDIPianoIsolated = new(pianoTrackChunks);

						//Delete the piano track in trackChunks and make that the new midi file
						trackChunks.RemoveAt(1);
						IEnumerable<TrackChunk> newTrackChunks = trackChunks.AsEnumerable();
						MIDI = new(newTrackChunks);
			*/
        }

        /// <summary>
        /// Play MIDI File and check if PlayIsolated is true. 
        /// To prevent exception when thread gets interuppted, it is encased in a try catch
        /// </summary>
        /// <param name="PlayIsolated"></param>
        public static void PlayMidi(Boolean PlayIsolated = false)
        {
            //Get first available device and play it
            using (var outputDevice = OutputDevice.GetAll().ToArray()[0])
            {
                try
                {
                    if (PlayIsolated)
                    {
                        MIDI?.Play(outputDevice);
                    }
                    else
                    {
                        var _playback = OriginalMIDI?.GetPlayback(outputDevice, new PlaybackSettings
                        {
                            ClockSettings = new MidiClockSettings
                            {
                                CreateTickGeneratorCallback = () => new RegularPrecisionTickGenerator()
                            }
                        });

                        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
                        PlaybackCurrentTimeWatcher.Instance.AddPlayback(_playback, TimeSpanType.Midi);
                        PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
                        PlaybackCurrentTimeWatcher.Instance.Start();

                        AllNotes = MIDIController.OriginalMIDI.GetNotes().ToArray();
                        _playback.Start();

                        SpinWait.SpinUntil(() => !_playback.IsRunning);

                        Console.WriteLine("Playback stopped or finished.");

                        outputDevice.Dispose();
                        _playback.Dispose();
                    }
                }
                catch (ThreadInterruptedException ex)
                {
                    //Ignore
                }
            }
        }

        private static void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
        {
            //For the future
        }

        private static void OnCurrentTimeChanged(object sender, PlaybackCurrentTimeChangedEventArgs e)
        {
            //Current tick
            foreach (var playbackTime in e.Times)
            {
                CurrentTick = (MidiTimeSpan)playbackTime.Time;
                //CurrentTick += (MidiTimeSpan)100;
                //Debug.WriteLine($"Current time is {CurrentTick}.");
            }
        }
    }
}