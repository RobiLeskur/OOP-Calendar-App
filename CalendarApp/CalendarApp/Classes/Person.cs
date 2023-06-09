﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Classes
{
    internal class Person
    {
        public string name { get; set; }
        public string surname { get; }
        public string email { get; }
        public Dictionary<Guid, bool> events { get; private set; }

        public Person(string aName, string aSurname, string aEmail) { 
            name = aName;
            surname = aSurname;
            email = aEmail;
            events = new Dictionary<Guid, bool>();
        }

        public void eraseGivenEventFromPerson(Guid eventId)
        {
            events.Remove(eventId);

        }

        public void initialiseEvents(List<Event> events)
        {
            foreach (var item in events) {
                this.events.Add(item.id, false);
            }

        }

        public void setAttendanceToTrue(List<Event> listOfEvents)
        {
            foreach (var item in listOfEvents)
            {
                events[item.id] = true;
            }
        }

        public void addEventToPerson(Guid id, bool isTrue)
        {
            if (!events.ContainsKey(id))
            {
                events.Add(id, isTrue);
            }
        }
        
    }
}
