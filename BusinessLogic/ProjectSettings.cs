namespace BusinessLogic
{
	public static class ProjectSettings
	{
		public static Dictionary<PianoHeroPath, string> paths = new()
		{
			{ PianoHeroPath.PianoSoundsFolder, "./PianoSoundPlayer/Sounds/Piano/" },
			{ PianoHeroPath.StartTune, "./PianoSoundPlayer/Sounds/StartTune.mid" },
			{ PianoHeroPath.ImagesFolder, "/Images/"}
		};

		/// <summary>
		/// Get path of directory
		/// </summary>
		/// <param name="directory"></param>
		/// <returns></returns>
		public static string GetPath(PianoHeroPath directory)
		{
			return paths[directory];
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