using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class MatchTeam
    {
        [PrimaryKey]
        public int MatchId { get; set; }

        [PrimaryKey]
        public int TeamId { get; set; }
    }
}
