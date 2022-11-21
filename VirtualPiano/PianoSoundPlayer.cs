using Melanchall.DryWetMidi.MusicTheory;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualPiano
{
	public class PianoSoundPlayer
	{
		private string pianoFilesFolder;
		private string pianoSoundPrefix;
		private string pianoSoundSuffix;


		/// <summary>
		/// The <paramref name="soundsFolder"/> is the folder that contains all the piano files.
		/// File example: "myPiano-CSharp.wav"
		/// The <paramref name="pianoSoundSuffix"/> is first part of the file ("myPiano-" in example.)
		/// The <paramref name="pianoSoundSuffix"/> is the last part / extension of the file. (".wav" in example)
		/// Make sure the note name part is spelled according to the DryWetMidi NoteName enum
		/// </summary>
		/// <param name="soundsFolder"></param>
		/// <param name="pianoSoundPrefix"></param>
		/// <param name="pianoSoundSuffix"></param>
		public PianoSoundPlayer(string soundsFolder, string pianoSoundPrefix, string pianoSoundSuffix)
		{
			pianoFilesFolder = soundsFolder;
			this.pianoSoundPrefix = pianoSoundPrefix;
			this.pianoSoundSuffix = pianoSoundSuffix;
		}

		public void PlayNote(NoteName noteName, int octave)
		{
			float frequency = (float)((float)1 / 1024 * (((float)octave + 5) * 100));
			string pianoNoteString = noteName.ToString();
			string pathToFile = pianoFilesFolder + pianoSoundPrefix + pianoNoteString + pianoSoundSuffix;
			PlaySoundOneshot(pathToFile, frequency);
		}

		public void PlaySoundOneshot(string audioFile, float frequency)
		{
			GetAudioClip(audioFile, frequency).Start();
		}

		public SourceVoice GetAudioClip(string audioFile, float frequency)
		{
			XAudio2 device = new XAudio2();
			MasteringVoice masteringVoice = new MasteringVoice(device);
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

		public FadingAudio GetFadingAudio(NoteName noteName, int octave)
		{
			float frequency = (float)((float)1 / 1024 * (((float)octave + 5) * 100));
			string pianoNoteString = noteName.ToString();
			string pathToFile = pianoFilesFolder + pianoSoundPrefix + pianoNoteString + pianoSoundSuffix;
			return new FadingAudio(GetAudioClip(pathToFile, frequency));
		}

		public class FadingAudio {
			private SourceVoice sourceVoice;

			public FadingAudio()
			{

			}

			public FadingAudio(SourceVoice sourceVoice)
			{
				this.sourceVoice = sourceVoice;
			}

			public void StartPlaying()
			{
				sourceVoice.Start();
			}

			public void StopPlaying(float fadeOutSpeed)
			{
				new Thread(() =>
				{
					float volume = 0;
					sourceVoice.GetVolume(out volume);
					while (volume > 0) {
						volume -= fadeOutSpeed / 1000;
						sourceVoice.SetVolume(volume);
						Thread.Sleep(10);
					}
					sourceVoice.Stop();
					sourceVoice.Dispose();
				}).Start();
			}
		}
	}
}
