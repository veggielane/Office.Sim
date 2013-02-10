using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.GameObjects;
using Office.Sim.Core.Graphics;
using Office.Sim.Core.Mapping;
using Office.Sim.Core.Messaging;
using Office.Sim.Core.Messaging.Messages;
using Office.Sim.Core.Timing;

namespace Office.Sim.Core
{
    public abstract class BaseGameEngine:IGameEngine
    {
        private readonly IGraphicsEngine _graphics;
        private readonly ITimer _timer;
        private readonly IGameObjectFactory _gameObjectFactory;
        public IMessageBus Bus { get; set; }

        public ILevel Level { get; private set; }

        protected BaseGameEngine(IMessageBus bus, IGraphicsEngine graphics, ILevel level, ITimer timer, IGameObjectFactory gameObjectFactory)
        {
            _graphics = graphics;
            _timer = timer;
            _gameObjectFactory = gameObjectFactory;
            Bus = bus;
            Level = level;
            _timer.Ticks.Subscribe(Update);
        }

        public void LoadLevel(ILevel level)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Bus.Add(new DebugMessage("Starting"));
            _timer.Start();
            _graphics.Start();
            Bus.Add(new DebugMessage("Loading Level"));
        }

        public void Stop()
        {
            Bus.Add(new DebugMessage("Stopping"));
        }


        private void Update(ITick tick)
        {
            foreach (var go in _gameObjectFactory.GameObjects.Values)
            {
                go.Update(tick);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
