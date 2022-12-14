using SharpDX.XAudio2;

namespace BusinessLogic.SoundPlayer
{
    public class FadingAudio
    {
        public SourceVoice sourceVoice { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="FadingAudio"/> and sets the <see cref="sourceVoice"/> to <paramref name="sourceVoice"/>
        /// </summary>
        /// <param name="sourceVoice"></param>
        public FadingAudio(SourceVoice sourceVoice)
        {
            this.sourceVoice = sourceVoice;
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
                    ThreadPool.QueueUserWorkItem(DecreaseVolume, new object[] { fadeOutSpeed, sourceVoice });
                }
            }
        }

        /// <summary>
        /// Selfexplanatory
        /// </summary>
        /// <param name="state"></param>
        public static void DecreaseVolume(object? state)
        {
            if (state is not object[] array)
            {
                return;
            }
            float fadeOutSpeed = (float)array[0];
            SourceVoice sourceVoice = (SourceVoice)array[1];
            sourceVoice.GetVolume(out float volume);
            while (volume > 0)
            {
                volume -= fadeOutSpeed / 1000;
                sourceVoice.SetVolume(volume);
                Thread.Sleep(10);
            }
            sourceVoice.Stop();
            sourceVoice.Dispose();
        }
    }
}
