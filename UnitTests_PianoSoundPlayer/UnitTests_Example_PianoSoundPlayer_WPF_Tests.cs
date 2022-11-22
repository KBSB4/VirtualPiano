
using VirtualPiano.PianoSoundPlayer;
using Melanchall.DryWetMidi.MusicTheory;
using SharpDX.XAudio2;

namespace UnitTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void PianoSoundPlayer_GetFadingAudio_PlayTenNotesFor3Seconds()
		{
			PianoSoundPlayer player = new PianoSoundPlayer("../../../../PianoSoundPlayer/Sounds/Piano/", "", ".wav");
			FadingAudio[] fadingAudios = {
				player.GetFadingAudio(NoteName.C, 5),
				player.GetFadingAudio(NoteName.CSharp, 5),
				player.GetFadingAudio(NoteName.D, 5),
				player.GetFadingAudio(NoteName.DSharp, 5),
				player.GetFadingAudio(NoteName.E, 5),
				player.GetFadingAudio(NoteName.F, 5),
				player.GetFadingAudio(NoteName.FSharp, 5),
				player.GetFadingAudio(NoteName.G, 5),
				player.GetFadingAudio(NoteName.GSharp, 5),
				player.GetFadingAudio(NoteName.A, 5),
			};

			new Thread(() =>
			{
				foreach (FadingAudio audio in fadingAudios)
				{
					audio.StartPlaying();
				}
			}).Start();

			new Thread(() =>
			{
				Thread.Sleep(3000);
				foreach (FadingAudio audio in fadingAudios)
				{
					audio.StopPlaying(0);
				}
			}).Start();

			Thread.Sleep(3200);

			foreach (FadingAudio fA in fadingAudios)
			{
				Assert.That(fA.sourceVoice.IsDisposed, Is.EqualTo(true));
			}
		}
	}
}