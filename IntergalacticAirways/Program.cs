using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SharpTrooper.Core;

namespace IntergalacticAirways
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpTrooperCore core = new SharpTrooperCore();

            var pilots = core.GetAllPeople();
            var starships = core.GetAllStarships();

            Console.WriteLine("Enter the number of passengers:");
            string input = Console.ReadLine();
            int passengerNum;

            if(!int.TryParse(input, out passengerNum))
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(0);
            }

            passengerNum = int.Parse(input);

            var availableStarships = from starship in starships.results.ToList()
                                     where
                                        int.Parse(Regex.Replace(starship.passengers, "[^0-9]", "0"),NumberStyles.AllowThousands) >= passengerNum
                                        && starship.pilots.Count() > 0
                                     select starship;

            if (availableStarships.ToList().Count == 0)
            {
                Console.WriteLine("There are no starships that can accomodate that ammount of passengers.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("The available starship and pilot pairs are listed below.");

            foreach(var starship in availableStarships)
            {

                foreach(var pilot in starship.pilots)
                {
                    var currentPilot = from person in pilots.results.ToList()
                                       where
                                        person.url == pilot
                                       select person.name;

                    Console.WriteLine("{0} - {1}", starship.name, currentPilot.First());
                }
                Console.WriteLine("---");
                
            }

            Console.Read();
        }
    }
}
