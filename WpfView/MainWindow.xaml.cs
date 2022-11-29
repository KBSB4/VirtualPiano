using BusinessLogic;
using Controller;
using Melanchall.DryWetMidi.Multimedia;
using Model;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PianoGridGenerator pianoGrid;

        private static IInputDevice _inputDevice;

        public MainWindow()
        {
            InitializeComponent();
            PianoController.CreatePiano();
            pianoGrid = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);

            //Add keydown event for the keys
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;
            //this.KeyDown += OnEventReceived;

            _inputDevice = Melanchall.DryWetMidi.Multimedia.InputDevice.GetByName("Keystation Mini 32");
            _inputDevice.EventReceived += OnEventReceived;
            _inputDevice.StartEventsListening();
        }


        private void OnEventReceived(object? sender, EventArgs e)
        {
            //var midiDevice = (MidiDevice)sender;

            if (e is MidiEventReceivedEventArgs a)
            {

                Debug.WriteLine($"{a.Event}");
                PianoKey? key = PianoController.ParseMidiNote(a.Event);

                if (key is not null)
                {
                    if (key.PressedDown)
                    {
						PianoController.PlayPianoSound(key);
						/*
												//KeyPressed(null, new KeyEventArgs( , new PresentationSource(), 1, (Key)((int)key.MicrosoftBind)));

												pianoGrid.DisplayPianoKey(key);

												PianoKey testKey = new PianoKey(Octave.Two, Melanchall.DryWetMidi.MusicTheory.NoteName.C, MicrosoftKeybinds.D1);
												testKey.PressedDown = true;
												pianoGrid.DisplayPianoKey(testKey);

												KeyPressed(null, CreateKeyEVentArgs(key));
												WhiteKeysGrid.InvalidateVisual();
												//WhiteKeysGrid.Dispatcher.Invoke(EmptyDelegate, DispatcherPriority.Render);
						*/
					}
                    else
                    {
                        PianoController.StopPianoSound(key);
                        //pianoGrid.DisplayPianoKey(key);
                    }
                }
            }
        }
        private static Action EmptyDelegate = delegate () { };

        /// <summary>
        /// Eventhandler for when the key gets pressed. Updates key and plays the audio
        /// Updates the view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyPressed(object? source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;

            PianoKey? key = PianoController.GetPressedPianoKey(intValue);
            if (key is not null)
            {
                pianoGrid.DisplayPianoKey(key);
                PianoController.PlayPianoSound(key);
            }

            if (e.Key == Key.CapsLock)
                PianoLogic.SwapOctave(PianoController.Piano);
        }

        public KeyEventArgs CreateKeyEVentArgs(PianoKey key)
        {
            switch (key.MicrosoftBind)
            {
                case MicrosoftKeybinds.D1:
                    return new KeyEventArgs(null, null, 0, Key.D1);
                    break;
                case MicrosoftKeybinds.D2:
                    return new KeyEventArgs(null, null, 0, Key.D2);
                    break;
                case MicrosoftKeybinds.D3:
                    return new KeyEventArgs(null, null, 0, Key.D3);
                    break;
                default:
                    return new KeyEventArgs(null, null, 0, Key.D5);
                    break;
            }
        }

        /// <summary>
        /// If pressed down keyboard key gets released, stop the audio playing for the pianokey and unpress it.
        /// Updates the view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void KeyReleased(object source, KeyEventArgs e)
        {
            int intValue = (int)e.Key;
            PianoKey? key = PianoController.GetReleasedKey(intValue);
            if (key is not null)
            {
                PianoController.StopPianoSound(key);
                pianoGrid.DisplayPianoKey(key);
            }

        }
    }
}
