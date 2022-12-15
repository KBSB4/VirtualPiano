using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DatabaseModels
{
	public class Highscore
	{
		public int SongId { get; set; }
		public int UserId { get; set; }
		public int Score { get; set; }
	}
}
