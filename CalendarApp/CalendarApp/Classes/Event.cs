using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Classes
{
    internal class Event
    {
        public Guid id { get; }
        public string name { get; set; }
        public string location { get; set; }
        public DateTime startingDate { get; set; }
        public DateTime endingDate { get; set; }
        public List<string> emailListOfAttendances { get; private set; }


        public Event(string aName, string aLocation, DateTime aStartingDate, DateTime aEndingDate)
        { 
            id = Guid.NewGuid();
            name = aName;
            location = aLocation;
            startingDate = aStartingDate;
            endingDate = aEndingDate;
        }


    }

   

}
