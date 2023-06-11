using CalendarApp.Classes;
using System;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Channels;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        public static void SetUpPeopleAndEvents(List<Person> people, List<Event> events)
        {
            foreach (Person person in people)
            {
                person.initialiseEvents(events);
            }
            events[0].addListOfEmailsToAnEvent(new List<string> { people[4].email, people[2].email, people[7].email });
            events[1].addListOfEmailsToAnEvent(new List<string> { people[0].email, people[1].email, people[4].email });
            events[2].addListOfEmailsToAnEvent(new List<string> { people[7].email, people[2].email, people[5].email });
            events[3].addListOfEmailsToAnEvent(new List<string> { people[1].email, people[8].email, people[7].email });
            events[4].addListOfEmailsToAnEvent(new List<string> { people[0].email, people[9].email, people[4].email });
            people[0].setAttendanceToTrue(new List<Event> { events[0], events[1], events[2], events[3], events[4] });
            people[1].setAttendanceToTrue(new List<Event> { events[0], events[2], events[3], events[4] });
            people[2].setAttendanceToTrue(new List<Event> { events[0], events[1], events[4] });
            people[3].setAttendanceToTrue(new List<Event> { events[2], events[3], events[4] });
            people[4].setAttendanceToTrue(new List<Event> { events[0], events[1], events[3], events[4] });
            people[5].setAttendanceToTrue(new List<Event> { events[1], events[2], events[3], events[4] });
            people[6].setAttendanceToTrue(new List<Event> { events[2], events[3], events[4] });
            people[7].setAttendanceToTrue(new List<Event> { events[0], events[1], events[2], });
            people[8].setAttendanceToTrue(new List<Event> { events[1], events[2], events[3], events[4] });
            people[9].setAttendanceToTrue(new List<Event> { events[0], events[1], events[2], events[3], events[4] });



        }


        public static List<Event> ReturnPreDefinedListOfEvents()
        {
            return new List<Event>()
    {
        new Event("Ultra 2023", "Split", new DateTime(2022, 2, 7), new DateTime(2023, 12, 21)),
        new Event("Tomorowland 2023", "Boom", new DateTime(2023, 7, 21), new DateTime(2023, 7, 23)),
        new Event("FIFA 2022", "Quatar", new DateTime(2022, 11, 18), new DateTime(2022, 11, 20)),
        new Event("Interliber", "Zagreb", new DateTime(2024, 11, 8), new DateTime(2024, 11, 13)),
        new Event("Aviation Nation 2022", "Nellis Airforce Base", new DateTime(2022, 11, 5), new DateTime(2022, 11, 6))
    };
        }

        public static List<Person> ReturnPreDefinedListOfPeople()
        {
            return new List<Person>()
    {
        new Person ("Ante", "Antic", "ante.antic@gmail.com"),
        new Person ("Roko", "Rokic", "roko.rokic@gmail.com"),
        new Person ("Mate", "Matic", "mate.matic@gmail.com"),
        new Person ("Marko", "Markic", "marko.markic@gmail.com"),
        new Person ("Petar", "Petric", "petar.petric@gmail.com"),
        new Person ("Matija", "Matijic", "matija.matijic@gmail.com"),
        new Person ("Karlo", "Karlic", "karlo.karlic@gmail.com"),
        new Person ("Zeljko", "Zeljkic", "zeljko.zeljkic@gmail.com"),
        new Person ("Toni", "Tonic", "toni.tonic@gmail.com"),
        new Person ("Lovre", "Lovric", "lovre.lovric@gmail.com")
    };
        }

        static int inputIncludingParsing(int numberOfChoices)
        {
            int number;
            bool isValidNumber = false;

            do
            {
                Console.Write("Unesite broj: ");
                var input = Console.ReadLine();
                isValidNumber = int.TryParse(input, out number);

                if (number > numberOfChoices || number <= 0)
                {
                    Console.Write("Unesite valjani broj: ");
                    isValidNumber = false;

                }
                else if (!isValidNumber)
                {
                    Console.Write("Morate unijeti broj, pokusajte ponovo: ");
                }

            } while (!isValidNumber);

            return number;

        }


        static int staringScreen()
        {
            Console.Clear();
            Console.WriteLine("1 - Aktivni eventi");
            Console.WriteLine("2 - Nadolazeci eventi");
            Console.WriteLine("3 - Eventi koji su zavrsili");
            Console.WriteLine("4 - Kreiraj event");
            Console.WriteLine("5 - Izlazak iz aplikacije");

            return inputIncludingParsing(5);

        }

        static bool isEventActive(Event e)
        {
            if (e.startingDate < DateTime.Now && e.endingDate > DateTime.Now)
                return true;
            else
                return false;

        }

        static void printIdNameAndLocationOfEvent(Event anEvent) {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"ID - {anEvent.id}");
            Console.WriteLine($"Naziv - {anEvent.name}");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"Lokacija - {anEvent.location}");
        }

        static void printListOfAttendances(Event anEvent)
        {
            Console.WriteLine("Popis sudionika: ");
            foreach (var item in anEvent.emailListOfAttendances)
                Console.WriteLine(item);

        }

        static void printActiveEvent(Event item)
        {
            printIdNameAndLocationOfEvent(item);
            TimeSpan difference = item.endingDate.Subtract(DateTime.Now);
            double differenceInHours = difference.TotalDays;
            Console.WriteLine($"Završava za - {Math.Round(differenceInHours, 1)} dana");
            printListOfAttendances(item);
        }

        static int eventsSubmenu(List<string> listOfMessages)
        {
            Console.WriteLine("-----------------------------------------");
            foreach(var item in listOfMessages)
            Console.WriteLine(item);
          
            return inputIncludingParsing(listOfMessages.Count);

        }

        static Guid inputGuidWithParse()
        {
            Guid guid;
            bool isInputValid;
            do
            {
                Console.WriteLine("Unesi postojeci id eventa: ");
                string str = Console.ReadLine();


                isInputValid = Guid.TryParse(str, out guid);

            } while (!isInputValid);

            return guid;
        }

        static bool checkIfIdExists(List<Event> events, Guid guid)
        {
            List<Guid> listOfIds = new List<Guid>();
            foreach (var item in events)
            {
                listOfIds.Add(item.id);
            }

            if (listOfIds.Contains(guid))
                return true;
            else
                return false;
        }

        static bool checkIfPersonsEmailExists(List<string> people, string email, string message)
        {

            if (people.Contains(email)) return true;
            else {
                Console.Write(message);
                return false;
            }

        }

        static List<string> inputListOfPeople(List<string> people, string str, string message)
        {
            List<string> listOfPeople = new List<string>();
            Console.Write(str);
            string email;
            do
            {
                email = Console.ReadLine();

                if (email == "0")
                    break;

                if (checkIfPersonsEmailExists(people, email, message)) {
                    listOfPeople.Add(email);
                    Console.WriteLine("Osoba zabilježena!");

                }




            } while (email != "0");

            return listOfPeople;

        }

        static void settingPersonsAttendanceToFalse(List<string> people, Guid eventId, List<Person> listOfPerson, List<Event> listOfEvents) {
            foreach (var person in people)
                listOfPerson[listOfPerson.FindIndex(obj => obj.email == person)].events[eventId] = false;
        }

        static void removingPeopeFromEvent(List<string> people, Guid eventId, List<Person> listOfPerson, List<Event> listOfEvents)
        {
            foreach (var person in people)
                listOfPerson[listOfPerson.FindIndex(obj => obj.email == person)].events.Remove(eventId);

        }

        static int indexOfPersonByEventId(List<Event> events, Guid givenEventId){
            while (!checkIfIdExists(events, givenEventId))
                givenEventId = inputGuidWithParse();

            return events.FindIndex(obj => obj.id == givenEventId);
}


        static void settingAttendanceToFalseFinal(List<Event> events, List<Person> people)
        {
            Guid givenEventId = inputGuidWithParse();
            int index = indexOfPersonByEventId(events, givenEventId);
            List<string> listOfPeopleToRemove = inputListOfPeople(events[index].emailListOfAttendances, "Unesi popis suodionika koji nisu bili prisutni, unesi 0 za izlaz: ", "Osoba nije prisutna na eventu");
            settingPersonsAttendanceToFalse(listOfPeopleToRemove, givenEventId, people, events);
        }
        static void removingPeopleFromEventFinal(List<Event> events, List<Person> people)
        {
            Guid givenEventId = inputGuidWithParse();
            int index = indexOfPersonByEventId(events, givenEventId);
            List<string> listOfPeopleToRemove = inputListOfPeople(events[index].emailListOfAttendances, "Unesi popis suodionika koje zelis uklonuti sa eveta, unesi 0 za izlaz: ", "Osoba nije prisutna na eventu");
            removingPeopeFromEvent(listOfPeopleToRemove, givenEventId, people, events);
        }

        static void activeEventsSubmenuWithSwitch(List<Event> events, List<Person> people)
        {
            int submenuSwitchNumber = eventsSubmenu(new List<string>() { "1 - Zabilježi neprisutnost", "2 - Povratak za glavni izbornik"});
            switch (submenuSwitchNumber)
            {
                case 1:
                    settingAttendanceToFalseFinal(events, people);

                    break;
                case 2:
                    break;
            }
        }

        static void deleteFutureEvent(List<Event> events, List<Person> people)
        {
            Guid givenEventId;
            do
            {
                givenEventId = inputGuidWithParse();
            } while (!checkIfIdExists(events, givenEventId));

            int index = events.FindIndex(obj => obj.id == givenEventId);

            foreach (var email in events[index].emailListOfAttendances)
                removeEventFromPerson(people, email, givenEventId);
            events.RemoveAt(index);
        }

        static void removeEventFromPerson(List<Person> people, string email, Guid eventId)
        {
            int index = people.FindIndex(obj => obj.email == email);
            people[index].eraseGivenEventFromPerson(eventId);
        }

        static void futureEventsSubmenuWithSwitch(List<Event> events, List<Person> people)
        {
            int submenuSwitchNumber = eventsSubmenu(new List<string>() { "1 - Izbrisi event", "2 - Ukloni osobe s eventa", "3 - Povratak na glavni izbornik" });
            switch (submenuSwitchNumber)
            {
                case 1:
                    deleteFutureEvent(events, people);
                    break;
                case 2:
                    removingPeopleFromEventFinal(events, people);
                    break;
                case 3:
                    break;
            }
        }

   


        static bool isEventInFuture(Event anEvent)
        {
            if(anEvent.startingDate > DateTime.Now) return true;
            return false;

        }


        static void printFutureEvent(Event item)
        {
            printIdNameAndLocationOfEvent(item);
            TimeSpan difference = item.startingDate.Subtract(DateTime.Now);
            Console.WriteLine($"Pocinje za - {Math.Round(difference.TotalDays, 1)} dana");
            Console.WriteLine($"Traje - {returnDurationOfAnEvent(item)} dana");

        }

        delegate void MyPrintFunction (Event item);
        delegate bool MyActivityFunction (Event item);

        static void printEventsOfGivenFunction(MyPrintFunction printFunction, MyActivityFunction checkActivityFunction, List<Event> events)
        {
            Console.Clear();
            foreach (var item in events)
            {
                if (checkActivityFunction(item) == true)
                {
                    printFunction(item);
                }
            }

        }

       

        static void PastEventsPrint(MyPrintFunction printFunction, MyActivityFunction checkActivityFunction, List<Event> events, List<Person> people)
        {
            Console.Clear();
            foreach (var item in events)
            {
                if (checkActivityFunction(item) == true)
                {
                    printFunction(item);

                    printPeopleWhoDidOrDidntAttend(people,item.emailListOfAttendances, item.id, true);
                    printPeopleWhoDidOrDidntAttend(people,item.emailListOfAttendances, item.id, false);

                }
            }
            Console.Write("Bilo koji znak za povratak: ");
            Console.ReadLine();
        }

        static void printPeopleWhoDidOrDidntAttend(List<Person> people,List<string> attendances, Guid eventId, bool didAttend)
        {
            if (didAttend == true) 
                Console.WriteLine("\n----Prisutni su bili----");
            else
                Console.WriteLine("----Nisu bili prisutni----");

            foreach (Person person in people)
            {
                if (person.events[eventId] == didAttend)
                    Console.WriteLine($"{person.name} {person.surname}");
            }
            Console.WriteLine();
        }


        static double returnDurationOfAnEvent(Event anEvent)
        {
            TimeSpan duration = anEvent.endingDate.Subtract(anEvent.startingDate);
            return Math.Round(duration.TotalDays, 1);
        }

        static bool isEventInPast(Event e)
        {
            if (e.endingDate < DateTime.Now)
                return true;
            else
                return false;

        }

        

        static void printPastEvent(Event item)
        {
            printIdNameAndLocationOfEvent(item);
            TimeSpan difference = DateTime.Now.Subtract(item.endingDate);
            Console.WriteLine($"Završio je prije - {Math.Round(difference.TotalDays, 1)} dana \nTrajao je - {returnDurationOfAnEvent(item)} - dana");  
        }

        static DateTime dateTimeInputWithParse()
        {
            string input = Console.ReadLine();

            DateTime date;
           
            while(DateTime.TryParse(input, out date) != true){
                if(DateTime.TryParse(input, out date) == false) {
                    Console.WriteLine("Unesi ispravan datum!");
                input = Console.ReadLine();
                }
            }

            return date;

        }

        static void createEvent(List<Event> events, List<Person> people)
        {
            Console.Clear();
            Console.Write("Unesi Naziv Eventa: ");
            string nameOfEvent = Console.ReadLine();
            Console.Write("Unesi Lokaciju Eventa: ");
            string locationOfEvent = Console.ReadLine();
            DateTime startingDate;
            DateTime endingDate;

            do
            {
                Console.WriteLine("Unesi datum pocetka (yyyy-mm-dd): ");
                startingDate = dateTimeInputWithParse();
                Console.WriteLine("Unesi datum kraja (yyyy-mm-dd): ");
                endingDate = dateTimeInputWithParse();

                if(startingDate > endingDate)
                    Console.WriteLine("Datum kraja ne moze biti prije pocetka!\n");

            } while(startingDate > endingDate);

            List<string> listOfAllEmails = new List<string>();
            Console.WriteLine("-------------------------------\nLista ljudi koje mozes dodati na event");
            foreach (Person person in people) {
                listOfAllEmails.Add(person.email);
                Console.WriteLine(person.email);
            }

            List<string> listOfEmails = inputListOfPeople(listOfAllEmails, "-------------------------------\nUnesi ljude koje zelis na eventu(0 - povratak): ", "Ne postoji osoba s unesenim e-mailom, unesi ponovo: ");
            //List<string> newListOfEmails = checkIfPersonIsAttendingAnotherEventAtTheSameTime(people, events, listOfAllEmails, startingDate, endingDate);
            
            if(listOfEmails.Count == 0) { 
                Console.WriteLine("Nitko ne dolazi na event pa je otkazan");
                return;
            }
            else
            {

                Event newEvent = new Event(nameOfEvent, locationOfEvent, startingDate, endingDate);
                events.Add(newEvent);
                newEvent.addListOfEmailsToAnEvent(listOfEmails);

                foreach(var item in listOfEmails)
                    Console.WriteLine(item);
                Console.ReadLine();

                foreach (var email in listOfEmails)
                  {
                      int index = people.FindIndex(obj => obj.email == email);
                      people[index].addEventToPerson(newEvent.id, true);
                }
               
                foreach(var email in listOfAllEmails)
                {
                    if (!listOfEmails.Contains(email)) { 
                        int index = people.FindIndex(obj => obj.email == email);
                    people[index].addEventToPerson(newEvent.id, false);
                    }
                }
             



            }
            Console.ReadLine();
        }

        static List<string> checkIfPersonIsAttendingAnotherEventAtTheSameTime(List<Person> people, List<Event> events,List<string> listOfEmails, DateTime newStartingDate, DateTime newEndingDate)
        {
            List<string> finalList = new List<string>();

            Console.WriteLine("---------------------------");

            foreach (var email in listOfEmails) { 
                int index = people.FindIndex(obj => obj.email == email);
                    foreach(var anEvent in events)
                {
                    if (anEvent.emailListOfAttendances.Contains(email) == true || newStartingDate > anEvent.startingDate || newEndingDate < anEvent.endingDate) {
                        finalList.Add(email);
                    }
                    else
                        Console.WriteLine($"Sudionik sa: {email} ne moze pristupiti jer ide na neki drugi event!");

                }

            }
            Console.WriteLine("---------------------------");
            return finalList;
        }



        static void Main()
        {
            var people = ReturnPreDefinedListOfPeople();
            var events = ReturnPreDefinedListOfEvents();
            SetUpPeopleAndEvents(people, events);

            int switchNumber = 1;


            while (true) {

                switchNumber = staringScreen();

                switch (switchNumber)
                {
                    case 1:
                        printEventsOfGivenFunction(printActiveEvent, isEventActive, events);
                        activeEventsSubmenuWithSwitch(events, people);

                        break;



                    case 2:
                        printEventsOfGivenFunction(printFutureEvent, isEventInFuture, events);
                        futureEventsSubmenuWithSwitch(events, people);

                        break;

                    case 3:
                        PastEventsPrint(printPastEvent, isEventInPast, events, people);
                        break;

                    case 4:
                        createEvent(events, people);
                        break;                
                }

                if (switchNumber == 5)
                    return;
           }


        }g
    }
}