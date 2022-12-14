﻿using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace Model
{
    public class Song
    {
        public MidiFile File { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public MetricTimeSpan Duration { get; set; }
        public Queue<PianoKey> PianoKeys { get; set; }
        public Thread? SongTimerThread { get; set; }
        public bool IsPlaying { get; set; }

        public event EventHandler<PianoKeyEventArgs>? NotePlayed;

        public Song(MidiFile file, string name, Difficulty difficulty, MetricTimeSpan duration, Queue<PianoKey> pianoKeys)
        {
            File = file;
            Name = name;
            Difficulty = difficulty;
            Duration = duration;
            PianoKeys = pianoKeys;
        }

        public void InvokeNotePlayed(Song song, PianoKeyEventArgs pianoKeyEventArgs)
        {
            song.NotePlayed?.Invoke(song, pianoKeyEventArgs);
        }
    }
}

