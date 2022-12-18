using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DatabaseModels
{
	public class Highscore
	{
		public Song Song { get; set; }
		public User User { get; set; }
		public int Score { get; set; }
	}
}
