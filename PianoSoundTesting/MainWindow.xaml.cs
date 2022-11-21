using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtualPiano;
using static VirtualPiano.PianoSoundPlayer;
using Melanchall.DryWetMidi.MusicTheory;

namespace PianoSoundTesting
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string _pianoFilesFolder = "../../../../VirtualPiano/Sounds/Piano/";
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
				currentPlayingAudio[e.Key].StopPlaying(50);
				currentPlayingAudio.Remove(e.Key);
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!currentPlayingAudio.ContainsKey(e.Key))
			{
				FadingAudio fadingAudio = new FadingAudio();
				switch(e.Key)
				{
					case Key.Q:
						fadingAudio = _player.GetFadingAudio(NoteName.C, 4);
						break;
					case Key.D2:
						fadingAudio = _player.GetFadingAudio(NoteName.CSharp, 4);
						break;
					case Key.W:
						fadingAudio = _player.GetFadingAudio(NoteName.D, 4);
						break;
					case Key.E:
						fadingAudio = _player.GetFadingAudio(NoteName.E, 4);
						break;
					case Key.R:
						fadingAudio = _player.GetFadingAudio(NoteName.F, 4);
						break;
					case Key.T:
						fadingAudio = _player.GetFadingAudio(NoteName.G, 4);
						break;
					case Key.Y:
						fadingAudio = _player.GetFadingAudio(NoteName.A, 4);
						break;
					case Key.U:
						fadingAudio = _player.GetFadingAudio(NoteName.B, 4);
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
