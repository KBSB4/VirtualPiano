namespace UnitTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		//[Test]
		//public void PianoSoundPlayer_GetFadingAudio_PlayTenNotesFor3Seconds()
		//{
		//	PianoSoundPlayer? player = new PianoSoundPlayer("../../../../PianoSoundPlayer/Sounds/Piano/", "", ".wav");
		//	FadingAudio[] fadingAudios = {
		//		player.GetFadingAudio(NoteName.C, 5),
		//		player.GetFadingAudio(NoteName.CSharp, 5),
		//		player.GetFadingAudio(NoteName.D, 5),
		//		player.GetFadingAudio(NoteName.DSharp, 5),
		//		player.GetFadingAudio(NoteName.E, 5),
		//		player.GetFadingAudio(NoteName.F, 5),
		//		player.GetFadingAudio(NoteName.FSharp, 5),
		//		player.GetFadingAudio(NoteName.G, 5),
		//		player.GetFadingAudio(NoteName.GSharp, 5),
		//		player.GetFadingAudio(NoteName.A, 5),
		//	};

		//	Thread? thr1 = new Thread(() =>
		//	{
		//		foreach (FadingAudio audio in fadingAudios)
		//		{
		//			audio.StartPlaying();
		//		}
		//	});
		//	thr1.Start();

		//	Thread? thr2 = new Thread(() =>
		//	{
		//		Thread.Sleep(3000);
		//		foreach (FadingAudio audio in fadingAudios)
		//		{
		//			audio.StopPlaying(0);
		//		}
		//	});
		//	thr2.Start();

		//	Thread.Sleep(3200);
		//	thr1 = null;
		//	thr2 = null;
		//	foreach (FadingAudio fA in fadingAudios)
		//	{
		//		Assert.That(fA.sourceVoice.IsDisposed, Is.EqualTo(true));
		//	}
		//	player.Dispose();
		//}

		//[Test]
		//public void PianoSoundPlayer_GetFadingAudio_PlayTenNotesFor3SecondsFor5Minutes()
		//{
		//	Stopwatch stopwatch = Stopwatch.StartNew();
		//	stopwatch.Start();
		//	while (stopwatch.ElapsedMilliseconds < 300_000 /* 1 minutes */)
		//	{
		//		PianoSoundPlayer_GetFadingAudio_PlayTenNotesFor3Seconds();
		//	}
		//	Assert.That(true);
		//}

		//[Test]
		//public void PianoSoundPlayer_PianoSoundPlayer_ObsoletePath()
		//{
		//	try
		//	{
		//		PianoSoundPlayer player;
		//		player = new PianoSoundPlayer("", "", ".wav");
		//		Assert.Fail("No exception was given, while path was absolete");
		//	}
		//	catch(Exception ex)
		//	{
		//		Assert.That(ex is DirectoryNotFoundException);
		//	}
		//}

		//[Test]
		//public void PianoSoundPlayer_GetAudioClip_NoNotesInDirectory()
		//{
		//	try
		//	{
		//		PianoSoundPlayer player;
		//		player = new PianoSoundPlayer("../../../../PianoSoundPlayer/Sounds/Piano/", "testname", ".wav");
		//		player.PlayNote(NoteName.C, 5);
		//		Assert.Fail("No exception was given, while the folder \"Piano\" only contains the note names " +
		//			"without prefix \"testname\"");
		//	}
		//	catch (Exception ex)
		//	{
		//		Assert.That(ex is FileNotFoundException);
		//	}
		//}
	}
}