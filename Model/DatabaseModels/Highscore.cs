namespace Model.DatabaseModels
{
    public class Highscore
    {
        public Song Song { get; set; }
        public User User { get; set; }
        public int Score { get; set; }
    }
}