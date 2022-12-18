using System.Diagnostics;

namespace BusinessLogic
{
	public static class ProjectSettings
	{
		public static Dictionary<PianoHeroPath, string> paths = new()
		{
			{ PianoHeroPath.PianoSoundsFolder, "../../../../BusinessLogic/PianoSoundPlayer/Sounds/Piano/" },
			{ PianoHeroPath.StartTune, "../../../../BusinessLogic/PianoSoundPlayer/Sounds/StartTune.mid" },
			{ PianoHeroPath.ImagesFolder, "/Images/"},
			{ PianoHeroPath.CMDFiles, "../../../../BusinessLogic/CMDFiles/databaseConnect.bat"}
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

        public  static void ExecuteSSHConnection()
        {
            Process process = new Process(); 

            string _batDir = GetPath(PianoHeroPath.CMDFiles);

            process.StartInfo.FileName = _batDir;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            //process.Close();
            Debug.Write("Executed bat file");
        }
    }

    


    /// <summary>
    /// <b>Piano Hero</b> Directory - stores directories for quick access.
    /// </summary>
    public enum PianoHeroPath
	{
		PianoSoundsFolder,
		StartTune,
		ImagesFolder,
		CMDFiles
	}
}