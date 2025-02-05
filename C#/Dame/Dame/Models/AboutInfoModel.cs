using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dame.Models
{
    public class AboutInfoModel
    {
        public string numeStudent { get; set; }
        public string adresaEmail { get; set; }
        public string grupa { get; set; }
        public string descriere { get; set; }

        public AboutInfoModel(string StudentName, string EmailAddress, string Group, string GameDescription)
        {

            numeStudent = StudentName;
            adresaEmail = EmailAddress;
            grupa = Group;
            descriere = GameDescription;

        }

    }
}

