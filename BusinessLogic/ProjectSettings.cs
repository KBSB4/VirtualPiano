namespace BusinessLogic
{
    public static class ProjectSettings
    {
        private static Dictionary<PianoHeroPath, string> paths = new()
        {
            { PianoHeroPath.PianoSoundsFolder, "BusinessLogic.PianoSoundPlayer.Sounds.Piano." },
            { PianoHeroPath.StartTune, "BusinessLogic.PianoSoundPlayer.Sounds.StartTune.mid" },
            { PianoHeroPath.ImagesFolder, "/Images/"}
        };

        public static Dictionary<PianoHeroPath, string> Paths { get => paths; set => paths = value; }

        /// <summary>
        /// Get path of directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetPath(PianoHeroPath directory)
        {
            return Paths[directory];
        }
    }

    /// <summary>
    /// <b>Piano Hero</b> Directory - stores directories for quick access.
    /// </summary>
    public enum PianoHeroPath
    {
        PianoSoundsFolder,
        StartTune,
        ImagesFolder
    }
}