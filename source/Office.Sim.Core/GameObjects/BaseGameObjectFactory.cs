using System;
using System.Collections.Concurrent;
using Office.Sim.Core.Messaging;

namespace Office.Sim.Core.GameObjects
{
    public class BaseGameObjectFactory:IGameObjectFactory
    {
        public IMessageBus Bus { get; private set; }
        public ConcurrentDictionary<Guid, IGameObject> GameObjects { get; private set; }
        public BaseGameObjectFactory()
        {
            GameObjects = new ConcurrentDictionary<Guid, IGameObject>();
        }
    }
}