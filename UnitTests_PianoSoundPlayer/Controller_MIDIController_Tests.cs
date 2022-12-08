using Controller;
using Melanchall.DryWetMidi.Core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
	
	public class Controller_MIDIController_Tests
	{
		[Test]
		[TestCase("\\")]
		public void MIDIController_OpenMidi_LoadFileTest(string file)
		{
			
			Assert.DoesNotThrow(() =>
			{
				MIDIController.OpenMidi(file);
			});
		}

		[Test]
		public void MIDIController_Convert_EmptyFile()
		{
			MidiFile file = new MidiFile();
			Song song = MIDIController.Convert(file);

			Assert.That(song, Is.Null);
		}
	}
}
