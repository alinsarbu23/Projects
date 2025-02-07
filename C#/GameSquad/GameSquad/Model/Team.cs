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

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int NumPlayers { get; set; }

        [NotNull]
        public DateTime CreationDate { get; set; }

        [NotNull]
        public string UserId { get; set; }
    }

}
