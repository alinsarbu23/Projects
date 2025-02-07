using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class Match
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime MatchDate { get; set; }

        [NotNull]
        public int Score1 { get; set; }

        [NotNull]
        public int Score2 { get; set; }

        public int? MinutesPlayed { get; set; }
        public int? TargetScore { get; set; }
        public string? Description { get; set; }
        public int? MvpPlayerId { get; set; }

        [NotNull]
        public string UserId { get; set; }
    }
}
