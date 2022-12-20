using SharpDX.XAudio2;

namespace BusinessLogic.SoundPlayer
{
    public class FadingAudio
    {
        public SourceVoice SourceVoice { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="FadingAudio"/> and sets the <see cref="SourceVoice"/> to <paramref name="sourceVoice"/>
        /// </summary>
        /// <param name="sourceVoice"></param>
        public FadingAudio(SourceVoice sourceVoice)
        {
            this.SourceVoice = sourceVoice;
        }

        /// <summary>
        /// Starts the <see cref="SourceVoice"/> and keeps playing until is shutdown by <see cref="StopPlaying(float)"/>
        /// </summary>
        public void StartPlaying()
        {
            SourceVoice?.Start();
        }

        /// <summary>
        /// Decreases the volume of <see cref="SourceVoice"/> in a new <see cref="Thread"/> by the amount specified by <paramref name="fadeOutSpeed"/> 
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
            float sourceVoiceVolume = 0;
            SourceVoice.GetVolume(out sourceVoiceVolume);

            fadeOutSpeed = fadeOutSpeed * sourceVoiceVolume;

			if (SourceVoice != null)
            {
                if (fadeOutSpeed == 0 || fadeOutSpeed > 1000)
                {
                    SourceVoice.DestroyVoice();
                    //SourceVoice.Stop();
                    //SourceVoice.Dispose();
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(DecreaseVolume, new object[] { fadeOutSpeed, SourceVoice });
                }
            }
        }

        /// <summary>
        /// Selfexplanatory
        /// </summary>
        /// <param name="state"></param>
        private static void DecreaseVolume(object? state)
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
            //sourceVoice.Stop();
            //sourceVoice.Dispose();
            sourceVoice.DestroyVoice();
        }
    }
}