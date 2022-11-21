using Melanchall.DryWetMidi.MusicTheory;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using SharpDX.XAudio2.Fx;
using System;
using System.Media;
using System.Windows.Input;
using VirtualPiano;

internal class Program
{

	private const string _pianoFilesFolder = "../../../Sounds/Piano/";
	private const string _pianoFileSuffix = "";
	private const string _pianoFilePrefix = ".wav";

	private static void Main(string[] args)
	{
		var player = new PianoSoundPlayer(_pianoFilesFolder, _pianoFilePrefix, _pianoFileSuffix);

		for (; ; )
		{
			if (Console.KeyAvailable)
			{
				ConsoleKey key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.Q:
						player.PlayNote(NoteName.C, 4);
						break;
					case ConsoleKey.D2:
						player.PlayNote(NoteName.CSharp, 4);
						break;
					case ConsoleKey.W:
						player.PlayNote(NoteName.D, 4);
						break;
					case ConsoleKey.D3:
						player.PlayNote(NoteName.DSharp, 4);
						break;
					case ConsoleKey.E:
						player.PlayNote(NoteName.E, 4);
						break;
					case ConsoleKey.R:
						player.PlayNote(NoteName.F, 4);
						break;
					case ConsoleKey.D5:
						player.PlayNote(NoteName.FSharp, 4);
						break;
					case ConsoleKey.T:
						player.PlayNote(NoteName.G, 4);
						break;
					case ConsoleKey.D6:
						player.PlayNote(NoteName.GSharp, 4);
						break;
					case ConsoleKey.Y:
						player.PlayNote(NoteName.A, 4);
						break;
					case ConsoleKey.D7:
						player.PlayNote(NoteName.ASharp, 4);
						break;
					case ConsoleKey.U:
						player.PlayNote(NoteName.B, 4);
						break;
				}
			}
			Thread.Sleep(10);
		}
	}

	

	public static bool IsKeyPressed(ConsoleKey key)
	{
		return Console.KeyAvailable && Console.ReadKey(true).Key == key;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="x"></param>
	/// <returns>The number zero</returns>
	public static int Test(int x)
	{
		if (true) Console.WriteLine("test");
		Console.WriteLine(x);
		return 0;
	}
}