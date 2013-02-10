using System;
using Office.Sim.Core.Messaging.Messages;

namespace Office.Sim.Core.Messaging
{
    public interface IMessageBus
    {
        IObservable<IMessage> Messages { get; }
        void Add(IMessage message);
    }
}

