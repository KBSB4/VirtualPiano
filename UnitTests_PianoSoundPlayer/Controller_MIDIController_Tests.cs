using Controller;
using Melanchall.DryWetMidi.Core;
using Model;

namespace UnitTests
{

	public class Controller_MIDIController_Tests
	{
		[Test]
		[TestCase("\\")]
		public void MIDIController_OpenMidi_LoadFileTest(string file)
		{
			Assert.Multiple(() =>
			{
				Assert.DoesNotThrow(() =>
				{
					MidiController.OpenMidi(file);
				});

				Assert.That(SongController.CurrentSong, Is.Null);
			});
		}

		[Test]
		public void MIDIController_Convert_EmptyFile()
		{
			MidiFile file = new();
			Song? song = MidiController.Convert(file);

			Assert.That(song, Is.Null);
		}


	}
}
