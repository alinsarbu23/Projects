using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class TeamPlayer
    {
        [PrimaryKey]
        public int TeamId { get; set; }

        [PrimaryKey]
        public int PlayerId { get; set; }
    }
}
