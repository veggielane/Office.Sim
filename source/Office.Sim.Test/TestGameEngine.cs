using Office.Sim.Core;
using Office.Sim.Core.GameObjects;
using Office.Sim.Core.Graphics;
using Office.Sim.Core.Mapping;
using Office.Sim.Core.Messaging;
using Office.Sim.Core.Timing;

namespace Office.Sim.Test
{
    public class TestGameEngine : BaseGameEngine
    {
        public TestGameEngine(IMessageBus bus, IGraphicsEngine graphics, ILevel level, ITimer timer, IGameObjectFactory gameObjectFactory)
            : base(bus, graphics, level, timer, gameObjectFactory)
        {
        }
    }
}