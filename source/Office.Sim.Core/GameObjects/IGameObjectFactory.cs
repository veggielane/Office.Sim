using System;
using System.Collections.Concurrent;
using Office.Sim.Core.Messaging;

namespace Office.Sim.Core.GameObjects
{
    public interface IGameObjectFactory : IHasMessageBus
    {
        ConcurrentDictionary<Guid, IGameObject> GameObjects { get; }
    }
}