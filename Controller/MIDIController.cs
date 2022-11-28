using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace Controller
{
    public static class MIDIController
    {
        public static MidiFile OriginalMIDI { get; set; }
        private static MidiFile MIDI { get; set; }
        private static MidiFile MIDIPianoIsolated { get; set; }

        //Note PlayBack has events notesplaybackstarted and notesplaybackfinished

        /// <summary>
        /// Read MIDI File and get karoake MIDIs out of it as well.
        /// </summary>
        /// <param name="MIDIpath"></param>
        public static void OpenMidi(string MIDIpath)
        {
            OriginalMIDI = MidiFile.Read(MIDIpath);

            //Get all tracks in the MIDI
            List<TrackChunk> trackChunks = TrackChunkUtilities.GetTrackChunks(OriginalMIDI).ToList();

            //UNDONE Figure out which track is piano (might be impossible, must be done in database or get highest amount of notes)
            TrackChunk pianoTrackChunks = trackChunks[1];
            MIDIPianoIsolated = new(pianoTrackChunks);

            //Delete the piano track in trackChunks and make that the new midi file
            trackChunks.RemoveAt(1);
            IEnumerable<TrackChunk> newTrackChunks = trackChunks.AsEnumerable();
            MIDI = new(newTrackChunks);
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
                        OriginalMIDI?.Play(outputDevice);
                    }
                }
                catch (ThreadInterruptedException ex)
                {
                    //Ignore
                }
            }
        }
    }
}
