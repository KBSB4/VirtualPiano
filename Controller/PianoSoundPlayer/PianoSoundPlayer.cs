using Melanchall.DryWetMidi.MusicTheory;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace VirtualPiano.PianoSoundPlayer
{
    public class PianoSoundPlayer
    {
        private string pianoFilesFolder;
        private string pianoSoundPrefix;
        private string pianoSoundSuffix;

        private XAudio2 device;
        MasteringVoice masteringVoice;

        /// <summary>
        /// The <paramref name="soundsFolder"/> is the folder that contains all the piano files.
        /// <para>
        /// 
        /// </para>
        /// <example>
        /// 
        /// File example: <b>"myPiano-CSharp.wav"</b>
        /// <code>
        /// The <paramref name="pianoSoundSuffix"/> is first part of the file (<b>"myPiano-"</b> in example.)
        /// The <paramref name="pianoSoundSuffix"/> is the last part / extension of the file. (<b>".wav"</b> in example)
        /// </code>
        /// </example>
        /// Make sure the note name part is spelled according to the <see cref="NoteName"/> enum from DryWetMidi
        /// </summary>
        /// <param name="soundsFolder"></param>
        /// <param name="pianoSoundPrefix"></param>
        /// <param name="pianoSoundSuffix"></param>
        public PianoSoundPlayer(string soundsFolder, string pianoSoundPrefix, string pianoSoundSuffix)
        {
            pianoFilesFolder = soundsFolder;
            this.pianoSoundPrefix = pianoSoundPrefix;
            this.pianoSoundSuffix = pianoSoundSuffix;
            VerifyDirectory();
            device = new XAudio2();
            masteringVoice = new MasteringVoice(device);
        }

        /// <summary>
        /// Verifies that the chosen directory actualy exists.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        private void VerifyDirectory()
        {
            if (!Directory.Exists(pianoFilesFolder))
                throw new DirectoryNotFoundException(pianoFilesFolder + " was not found");
        }

        /// <summary>
        /// Plays a single note until the length of the audio has been reached, using the <paramref name="noteName"/> to get the associated note, and <paramref name="octave"/> to increase / decrease octave by pitch
        /// </summary>
        /// <param name="noteName"></param>
        /// <param name="octave"></param>
        public void PlayNote(NoteName noteName, int octave)
        {
            float frequency = GetOctaveFrequencyRatio(octave);
            string pianoNoteString = noteName.ToString();
            string pathToFile = pianoFilesFolder + pianoSoundPrefix + pianoNoteString + pianoSoundSuffix;
            PlaySoundOneshot(pathToFile, frequency);
        }

        /// <summary>
        /// Plays a new <see cref="SourceVoice"/> using the file path <paramref name="audioFile"/>. The frequency of the <see cref="SourceVoice"/> is adjusted by <paramref name="frequency"/>
        /// </summary>
        /// <param name="audioFile"></param>
        /// <param name="frequency"></param>
        public void PlaySoundOneshot(string audioFile, float frequency)
        {
            GetSourceVoice(audioFile, frequency).Start();
        }

        /// <summary>
        /// Instantiates a new <see cref="SourceVoice"/> and attaches the file with filepath <paramref name="audioFile"/>. The frequency can be changed using <paramref name="frequency"/>
        /// </summary>
        /// <param name="audioFile"></param>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public SourceVoice GetSourceVoice(string audioFile, float frequency)
        {
            SoundStream stream = new(File.OpenRead(audioFile));
            WaveFormat waveFormat = stream.Format;
            AudioBuffer buffer = new()
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };

            SourceVoice sourceVoice = new SourceVoice(device, waveFormat, true);

            sourceVoice.SetFrequencyRatio(frequency);
            sourceVoice.BufferEnd += (context) => Console.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);

            return sourceVoice;
        }

        /// <summary>
        /// Instantiates a new <see cref="SourceVoice"/> that contains the audiofile found by "<see cref="pianoFilesFolder"/> + 
        /// <see cref="pianoSoundPrefix"/> + <paramref name="noteName"/> + <paramref name="octave"/> + <see cref="pianoSoundPrefix"/>"
        /// </summary>
        /// <param name="noteName"></param>
        /// <param name="octave"></param>
        /// <returns></returns>
        public SourceVoice GetSourceVoice(NoteName noteName, int octave)
        {
            string file = pianoFilesFolder + pianoSoundPrefix + noteName.ToString() + ((uint)octave) + pianoSoundSuffix;
            SoundStream stream = new(File.OpenRead(file));
            WaveFormat waveFormat = stream.Format;
            AudioBuffer buffer = new()
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };

            SourceVoice sourceVoice = new SourceVoice(device, waveFormat, true);

            sourceVoice.BufferEnd += (context) => Console.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);

            stream.Close();

            return sourceVoice;
        }

        /// <summary>
        /// Gets a new instance of <see cref="FadingAudio"/> using <paramref name="noteName"/> to get the correct file associated to the note. 
        /// Increases or decreases the pitch of the audio according to <paramref name="octave"/>
        /// <para>
        /// <i>
        /// <see cref="FadingAudio"/> can be used to Play, Stop and Stop(Fade-out) a <see cref="SourceVoice"/>
        /// </i>
        /// </para>
        /// </summary>
        /// <param name="noteName"></param>
        /// <param name="octave"></param>
        /// <returns></returns>
        public FadingAudio GetFadingAudio(NoteName noteName, int octave)
        {
            float frequency = GetOctaveFrequencyRatio(octave);
            string pianoNoteString = noteName.ToString();
            string pathToFile = pianoFilesFolder + pianoSoundPrefix + pianoNoteString + pianoSoundSuffix;
            //return new FadingAudio(GetSourceVoice(pathToFile, frequency));
            return new FadingAudio(GetSourceVoice(noteName, octave));
        }

        /// <summary>
        /// Gets the currect pitchshift for each octave specifiek by <paramref name="octave"/>.
        /// <para>
        /// Min <paramref name="octave"/> = 2, Max <paramref name="octave"/> = 5 else returns 0
        /// </para>
        /// </summary>
        /// <param name="octave"></param>
        /// <returns></returns>
        private float GetOctaveFrequencyRatio(int octave)
        {
            return octave switch
            {
                2 => 0.125f,
                3 => 0.25f,
                4 => 0.5f,
                5 => 1,
                _ => 0,
            };
        }

        /// <summary>
        /// Nullafies the object and remove unncessary objects 
        /// </summary>
        public void Dispose()
        {
            device.Dispose();
            masteringVoice.Dispose();
        }
    }
}
