namespace BusinessLogic
{
	public static class ProjectSettings
	{
		public static Dictionary<PianoHeroPath, string> paths = new()
		{
			{ PianoHeroPath.PianoSoundsFolder, "../../../../BusinessLogic/PianoSoundPlayer/Sounds/Piano/" },
			{ PianoHeroPath.StartTune, "../../../../BusinessLogic/PianoSoundPlayer/Sounds/StartTune.mid" }
		};

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
	}
}
