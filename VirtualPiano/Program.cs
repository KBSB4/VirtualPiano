using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.MusicTheory;
using Note = Melanchall.DryWetMidi.Interaction.Note;


using (var outputDevice = OutputDevice.GetByIndex(0))
{
    var midiFile = MidiFile.Read("C:\\Users\\keesw\\source\\repos\\MIDITESTING\\MIDITESTING\\MIDI-files\\test3.mid");
    Console.WriteLine("test");
    midiFile.Play(outputDevice);
    Console.WriteLine("test");
    var chords = midiFile.GetChords();
    foreach (var chord in chords)
    {
        Console.WriteLine("Hello, World!");
        Test(1);
        SoundPlayer snd = new SoundPlayer("../../../Sounds/639466__timouse__piano-loop-7.wav");
        snd.Play();

        for (; ; )
        {
            Thread.Sleep(100);
        }
        //Test
        Console.WriteLine(chord.Notes.GetEnumerator().Current.GetMusicTheoryNote().NoteName);
        //chord.Notes.Play();
    }

    Note note1 = new Note(NoteName.CSharp, 100);
}

//foreach (var chord in chords)
//{

//}