using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Area51_Elevator
{
    enum AgentActionOutdoors { Elevator, Home, Patrol}
    enum AgentActionFloor { Work, Leave, LookAround}
    class Agent
    { 
        public Elevator Elevator { get; set; }
        public int SecurityLevel { get; set; }
        public int AgentCode { get; set; }
        public Floor AgentCurrentFloor { get; set; }

        Random rand = new Random();

        ManualResetEvent EndWorkDay = new ManualResetEvent(false);

        private AgentActionOutdoors GetRandomOutdoorAction()
        {
            int val = rand.Next(10);
            if (val < 7) return AgentActionOutdoors.Elevator;
            else return AgentActionOutdoors.Home;
        }
        private AgentActionFloor GetRandomFloorAction()
        {
            int val = rand.Next(10);
            if (val < 4) return AgentActionFloor.LookAround;
            else if (val < 8) return AgentActionFloor.Work;
            else return AgentActionFloor.Leave;
        }
        private Floor GetRandomFloor()
        {
            int val = rand.Next(10);
            if (val < 3) return Floor.Secret;
            else if (val < 6) return Floor.T1;
            else if (val < 9) return Floor.T2;
            else return Floor.Ground;
        }

        private void PatrolArea()
        {
            Console.WriteLine("Agent-"+AgentCode + " patrols the area.(" + SecurityLevel+")");
            Thread.Sleep(500);
        }

        private void Work()
        {
            if (AgentCurrentFloor==Floor.Secret)
            {
                Console.WriteLine("Agent-" + AgentCode + " is sending nukes.");
            }
            else if (AgentCurrentFloor == Floor.T1)
            {
                Console.WriteLine("Agent-" + AgentCode + " is testing secret weapons.");
            }
            else if (AgentCurrentFloor == Floor.T2)
            {
                Console.WriteLine("Agent-" + AgentCode + " is examining alien remains.");
            }
            else
            {
                Console.WriteLine("Agent-" + AgentCode + " is working.");
            }

        }

        private void CallElevator()
        {

            Elevator.Call(AgentCurrentFloor);
            Elevator.Enter(this);
            Console.WriteLine("Agent-" + AgentCode + " entered the elevator.");
            Floor RandomFloor = GetRandomFloor();
            while (!Elevator.GoToFloor(RandomFloor))
            {
                RandomFloor = GetRandomFloor();             
            }
            AgentCurrentFloor = RandomFloor;
            if (AgentCurrentFloor == Floor.Ground) return;
            while (true)
            {
                AgentActionFloor FloorAction = GetRandomFloorAction();
                switch (FloorAction)
                {
                    case AgentActionFloor.Work:
                        Work();
                        Thread.Sleep(10000);
                        break;
                    case AgentActionFloor.LookAround:
                        Console.WriteLine("Agent-" + AgentCode + " is checking the area.");
                        Thread.Sleep(5000);
                        break;
                    case AgentActionFloor.Leave:
                        CallElevator();
                        return;
                    default:
                        throw new ArgumentException(FloorAction + " action is not supported!");
                }
            }         
        }

        private void StartWorkDayInternal()
        {
            while (true)
            {
                PatrolArea();
                AgentActionOutdoors outdoorAction = GetRandomOutdoorAction();
                switch (outdoorAction)
                {
                    case AgentActionOutdoors.Elevator:
                        CallElevator();
                        break;
                    case AgentActionOutdoors.Home:
                        Console.WriteLine("Agent-" +AgentCode+ " finished work day.");
                        EndWorkDay.Set();
                        return;
                    default:
                        throw new ArgumentException(outdoorAction + " action is not supported!");
                }
            }
        }

        public void StartWorkDay()
        {
            Thread t = new Thread(StartWorkDayInternal);
            t.Start();
        }

        public bool EndDay
        {
            get
            {
                return EndWorkDay.WaitOne(0);
            }
        }
    }
}
