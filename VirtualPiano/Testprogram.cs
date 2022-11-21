using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.MusicTheory;

namespace InputDeviceExample
{
    class Program
    {
        private static IInputDevice _inputDevice;

        static void Main(string[] args)
        {
            _inputDevice = InputDevice.GetByName("Launchkey 49");
            _inputDevice.EventReceived += OnEventReceived;
            _inputDevice.StartEventsListening();

            Console.WriteLine("Input device is listening for events. Press any key to exit...");
            Console.ReadKey();

            (_inputDevice as IDisposable)?.Dispose();
        }

        private static void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            var midiDevice = (MidiDevice)sender;
            Console.WriteLine($"Event received from '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");

            //PlayNote(NoteName.C);

            //int.Parse(e.Event.ToString().Substring(13, 2))
        }

        public static void PlayNote(NoteName noteName)
        {
            TempoMapManager tempoMapManager = new TempoMapManager();
            tempoMapManager.SetTempo(new MetricTimeSpan(0, 0, 20), new Tempo(400000));
            TempoMap map = tempoMapManager.TempoMap;

            var pattern = new PatternBuilder()
            .Note(noteName, MusicalTimeSpan.Quarter)
            .Build();

            using (var outputDevice = OutputDevice.GetByIndex(0))
            {
                pattern.Play(map, (FourBitNumber)1, outputDevice);
            }
        }
    }
}