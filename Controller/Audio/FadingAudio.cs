using SharpDX.XAudio2;

namespace VirtualPiano.PianoSoundPlayer
{
    public class FadingAudio
    {

        private SourceVoice sourceVoice;

        /// <summary>
        /// Creates an instance of <see cref="FadingAudio"/> and sets the <see cref="sourceVoice"/> to <paramref name="sourceVoice"/>
        /// </summary>
        /// <param name="sourceVoice"></param>

        public FadingAudio(SourceVoice sourceVoice)
        {
            this.sourceVoice = sourceVoice;
        }

        /// <summary>
        /// Constructor that does not set a sourcevoice
        /// </summary>
        public FadingAudio() : this(null)
        {

        }

        /// <summary>
        /// Starts the <see cref="sourceVoice"/> and keeps playing until is shutdown by <see cref="StopPlaying(float)"/>
        /// </summary>
        public void StartPlaying()
        {
            if (sourceVoice != null)
                sourceVoice.Start();
        }

        /// <summary>
        /// Decreases the volume of <see cref="sourceVoice"/> in a new <see cref="Thread"/> by the amount specified by <paramref name="fadeOutSpeed"/> 
        /// <para><paramref name="fadeOutSpeed"/> should be between 0 - 1000</para>
        /// <para>
        /// <example>
        /// If the <paramref name="fadeOutSpeed"/> is set to <b>0</b> then the sound will stop playing immediately
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="fadeOutSpeed"></param>
        public void StopPlaying(float fadeOutSpeed)
        {
            if (sourceVoice != null)
            {
                if (fadeOutSpeed == 0 || fadeOutSpeed > 1000)
                {
                    sourceVoice.Stop();
                    sourceVoice.Dispose();
                }
                else
                {
                    new Thread(() =>
                    {
                        float volume = 0;
                        sourceVoice.GetVolume(out volume);
                        while (volume > 0)
                        {
                            volume -= fadeOutSpeed / 1000;
                            sourceVoice.SetVolume(volume);
                            Thread.Sleep(10);
                        }
                        sourceVoice.Stop();
                        sourceVoice.Dispose();
                    }).Start();
                }
            }
        }
    }
}
