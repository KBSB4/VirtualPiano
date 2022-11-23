using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Melanchall.DryWetMidi.MusicTheory;
using VirtualPiano.PianoSoundPlayer;

namespace PianoSoundTesting
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string _pianoFilesFolder = "../../../../PianoSoundPlayer/Sounds/Piano/";
		private const string _pianoFilePrefix = "";
		private const string _pianoFileSuffix = ".wav";

		private Dictionary<Key, FadingAudio> currentPlayingAudio = new();

		private PianoSoundPlayer _player;

		public MainWindow()
		{
			InitializeComponent();
			_player = new PianoSoundPlayer(_pianoFilesFolder, _pianoFilePrefix, _pianoFileSuffix);

			KeyDown += OnKeyDown;
			KeyUp += OnKeyUp; ;

		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (currentPlayingAudio.ContainsKey(e.Key))
			{
				currentPlayingAudio[e.Key].StopPlaying(25);
				currentPlayingAudio.Remove(e.Key);
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!currentPlayingAudio.ContainsKey(e.Key))
			{
				FadingAudio? fadingAudio = new FadingAudio();
				int topOctave = 5;
				int buttomOctave = 4;
				switch(e.Key)
				{
					case Key.Q:
						fadingAudio = _player.GetFadingAudio(NoteName.C, topOctave);
						break;
					case Key.D2:
						fadingAudio = _player.GetFadingAudio(NoteName.CSharp, topOctave);
						break;
					case Key.W:
						fadingAudio = _player.GetFadingAudio(NoteName.D, topOctave);
						break;
					case Key.D3:
						fadingAudio = _player.GetFadingAudio(NoteName.DSharp, topOctave);
						break;
					case Key.E:
						fadingAudio = _player.GetFadingAudio(NoteName.E, topOctave);
						break;
					case Key.R:
						fadingAudio = _player.GetFadingAudio(NoteName.F, topOctave);
						break;
					case Key.D5:
						fadingAudio = _player.GetFadingAudio(NoteName.FSharp, topOctave);
						break;
					case Key.T:
						fadingAudio = _player.GetFadingAudio(NoteName.G, topOctave);
						break;
					case Key.D6:
						fadingAudio = _player.GetFadingAudio(NoteName.GSharp, topOctave);
						break;
					case Key.Y:
						fadingAudio = _player.GetFadingAudio(NoteName.A, topOctave);
						break;
					case Key.D7:
						fadingAudio = _player.GetFadingAudio(NoteName.ASharp, topOctave);
						break;
					case Key.U:
						fadingAudio = _player.GetFadingAudio(NoteName.B, topOctave);
						break;

					case Key.Z:
						fadingAudio = _player.GetFadingAudio(NoteName.C, buttomOctave);
						break;
					case Key.S:
						fadingAudio = _player.GetFadingAudio(NoteName.CSharp, buttomOctave);
						break;
					case Key.X:
						fadingAudio = _player.GetFadingAudio(NoteName.D, buttomOctave);
						break;
					case Key.D:
						fadingAudio = _player.GetFadingAudio(NoteName.DSharp, buttomOctave);
						break;
					case Key.C:
						fadingAudio = _player.GetFadingAudio(NoteName.E, buttomOctave);
						break;
					case Key.V:
						fadingAudio = _player.GetFadingAudio(NoteName.F, buttomOctave);
						break;
					case Key.G:
						fadingAudio = _player.GetFadingAudio(NoteName.FSharp, buttomOctave);
						break;
					case Key.B:
						fadingAudio = _player.GetFadingAudio(NoteName.G, buttomOctave);
						break;
					case Key.H:
						fadingAudio = _player.GetFadingAudio(NoteName.GSharp, buttomOctave);
						break;
					case Key.N:
						fadingAudio = _player.GetFadingAudio(NoteName.A, buttomOctave);
						break;
					case Key.J:
						fadingAudio = _player.GetFadingAudio(NoteName.ASharp, buttomOctave);
						break;
					case Key.M:
						fadingAudio = _player.GetFadingAudio(NoteName.B, buttomOctave);
						break;
					default:
						fadingAudio = null;
						break;
				}

				if (fadingAudio != null)
				{
					fadingAudio.StartPlaying();
					currentPlayingAudio.Add(e.Key, fadingAudio);
				}
			}
		}
	}
}
