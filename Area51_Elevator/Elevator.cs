using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Area51_Elevator
{
    enum Floor { Ground, Secret, T1, T2 }

    class Elevator
    {
        private Semaphore semaphore;
        public Floor ElevatorCurrentFloor { get; set; }
        public Floor CalledToFloor { get; set; }
        public Agent CurrentAgent { get; set; }

        public Elevator()
        {
            semaphore = new Semaphore(1, 1);
            ElevatorCurrentFloor = Floor.Ground;
        }

        public void Enter(Agent agent)
        {
            while (agent.AgentCurrentFloor != ElevatorCurrentFloor) {}
            CurrentAgent = agent;
        }

        public void Call(Floor CalledToFloor)
        {
            semaphore.WaitOne();
            Console.WriteLine("Elevator called to "+CalledToFloor);
            GoToFloor(CalledToFloor);
        }

        public bool GoToFloor(Floor floor)
        {
            if (ElevatorCurrentFloor != floor)
            {
                Console.WriteLine("Elevator is going from floor " + ElevatorCurrentFloor + " to " + floor);
            }   
            Thread.Sleep(Math.Abs((int)ElevatorCurrentFloor-(int)floor)*1000);
            ElevatorCurrentFloor = floor;
            if (CurrentAgent != null)
            {
                if (CurrentAgent.SecurityLevel < (int)ElevatorCurrentFloor)
                {
                    Console.WriteLine("Access denied!");
                    return false;
                }
                Console.WriteLine("Access granted!");
                Leave();          
            }
            return true;
        }

        public void Leave()
        {
            CurrentAgent.AgentCurrentFloor = ElevatorCurrentFloor;
            semaphore.Release();
            CurrentAgent = null;
        }
    }
}
