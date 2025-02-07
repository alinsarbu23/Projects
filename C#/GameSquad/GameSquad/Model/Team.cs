using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class Team
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(20)]
        public string Name { get; set; }

        [NotNull]
        public int NumPlayers { get; set; }

        [NotNull]
        public DateTime CreationDate { get; set; }

        [NotNull, MaxLength(36)]
        public string UserId { get; set; }
    }

}
