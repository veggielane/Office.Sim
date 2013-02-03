using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Messaging;
using Office.Sim.Core.Messaging.Messages;

namespace Office.Sim.Core
{
    public class GameEngine:IGameEngine
    {
        public IMessageBus Bus { get; set; }

        public GameEngine(IMessageBus bus)
        {
            Bus = bus;
        }

        public void LoadLevel(ILevel level)
        {
            Bus.Add(new DebugMessage("Loading Level"));
            Console.WriteLine(level.Map);
        }

        public void Start()
        {
            Bus.Add(new DebugMessage("Starting"));
        }

        public void Stop()
        {
            Bus.Add(new DebugMessage("Stopping"));
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
