using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class Record
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int PlayerId { get; set; }

        [NotNull]
        public int GoalsScored { get; set; }

        [NotNull]
        public int Assists {  get; set; }

        public string? Description { get; set; }

        [NotNull]
        public DateTime Date { get; set; }

        [NotNull]
        public String UserId { get; set; }
    }
}
