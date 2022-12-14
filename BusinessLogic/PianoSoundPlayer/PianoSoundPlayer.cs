using Melanchall.DryWetMidi.MusicTheory;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System.Diagnostics;
using System.Reflection;

namespace BusinessLogic.SoundPlayer
{
    public class PianoSoundPlayer
    {
        private readonly string pianoFilesFolder;
        private readonly string pianoSoundSuffix;

        private readonly XAudio2 device;

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
        public PianoSoundPlayer(string pianoSoundPrefix, string pianoSoundSuffix)
        {
            this.pianoSoundSuffix = pianoSoundSuffix;
            pianoFilesFolder = ProjectSettings.GetPath(PianoHeroPath.PianoSoundsFolder) + pianoSoundPrefix;
            VerifyDirectory();
            device = new XAudio2();
            _ = new MasteringVoice(device);
        }

        /// <summary>
        /// Verifies that the chosen directory actualy exists.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        private void VerifyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();

            foreach (string str in names)
            {
                if (str.StartsWith(pianoFilesFolder)) return;
            }
            throw new DirectoryNotFoundException(pianoFilesFolder + " was not found");
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

            SourceVoice sourceVoice = new(device, waveFormat, true);

            sourceVoice.SetFrequencyRatio(frequency);
            sourceVoice.BufferEnd += (context) => Debug.WriteLine(" => event received: end of buffer");
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
        public SourceVoice? GetSourceVoice(NoteName noteName, int octave)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();

            string file = pianoFilesFolder + noteName.ToString() + ((uint)octave) + pianoSoundSuffix;

            if (!names.Contains(file))
            {
                return null;
            }

            SoundStream stream = new(assembly.GetManifestResourceStream(file));
            WaveFormat waveFormat = stream.Format;
            AudioBuffer buffer = new()
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };

            SourceVoice sourceVoice = new(device, waveFormat, true);

            sourceVoice.BufferEnd += (context) => Debug.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);

            stream.Close();
            stream.Dispose();

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
        public FadingAudio? GetFadingAudio(NoteName noteName, int octave, float volume = 1)
        {
            SourceVoice? sourceVoice = GetSourceVoice(noteName, octave);
            if (sourceVoice is not null)
            {
                sourceVoice.SetVolume(volume * volume);

                return new FadingAudio(sourceVoice);
            }
            return null;
        }
    }
}