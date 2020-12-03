using System;
using System.Linq;

namespace Area51_Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            Elevator elevator = new Elevator();
            Random rnd = new Random();
            int AccessLevel;

            var Agents = 
                Enumerable.Range(1, 51)
                .Select(i => new Agent { AgentCode = i, Elevator = elevator})
                .ToList();

            foreach (var agent in Agents)
            {
                AccessLevel = rnd.Next(0,4);
                if (AccessLevel == 2) AccessLevel++;
                agent.SecurityLevel = AccessLevel;
                agent.StartWorkDay();
            }

            while (Agents.Any(s => !s.EndDay)) { }
            Console.WriteLine("Day is over.");
            Console.ReadLine();
        }
    }
}
