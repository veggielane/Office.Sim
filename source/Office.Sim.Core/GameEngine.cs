using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Graphics;
using Office.Sim.Core.Messaging;
using Office.Sim.Core.Messaging.Messages;

namespace Office.Sim.Core
{
    public class GameEngine:IGameEngine
    {
        private readonly IGraphicsEngine _graphics;
        public IMessageBus Bus { get; set; }

        public ILevel Level { get; private set; }

        public GameEngine(IMessageBus bus, IGraphicsEngine graphics, ILevel level)
        {
            _graphics = graphics;
            Bus = bus;
            Level = level;
        }

        public void LoadLevel(ILevel level)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Bus.Add(new DebugMessage("Starting"));
            _graphics.Start();

            Bus.Add(new DebugMessage("Loading Level"));
            Console.WriteLine(Level.Map);
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
