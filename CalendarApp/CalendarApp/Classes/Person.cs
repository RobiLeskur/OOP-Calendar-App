using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Classes
{
    internal class Person
    {
        public string name { get; set; }
        public string surname { get; }
        public string email { get; }
        public Dictionary<Guid, bool> attencance { get; private set; }

        public Person(string aName, string aSurname, string aEmail) { 
            name = aName;
            surname = aSurname;
            email = aEmail;

        
        }
        
    }
}
