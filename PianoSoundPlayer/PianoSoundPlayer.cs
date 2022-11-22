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
            device = new XAudio2();
            MasteringVoice masteringVoice = new MasteringVoice(device);
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
            GetAudioClip(audioFile, frequency).Start();
        }

        /// <summary>
        /// Instantiates a new <see cref="SourceVoice"/> and attaches the file with filepath <paramref name="audioFile"/>. The frequency can be changed using <paramref name="frequency"/>
        /// </summary>
        /// <param name="audioFile"></param>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public SourceVoice GetAudioClip(string audioFile, float frequency)
        {
            var stream = new SoundStream(File.OpenRead(audioFile));
            var waveFormat = stream.Format;
            var buffer = new AudioBuffer
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };

            var sourceVoice = new SourceVoice(device, waveFormat, true);
            sourceVoice.SetFrequencyRatio(frequency);
            sourceVoice.BufferEnd += (context) => Console.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);

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
            return new FadingAudio(GetAudioClip(pathToFile, frequency));
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
            switch (octave)
            {
                case 2:
                    return 0.125f;
                case 3:
                    return 0.25f;
				case 4:
					return 0.5f;
				case 5:
					return 1;
				default:
                    return 0;
            }
		}
    }
}
