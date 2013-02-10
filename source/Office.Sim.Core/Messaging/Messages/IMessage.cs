using System;

namespace Office.Sim.Core.Messaging.Messages
{
    public interface IMessage
    {
        DateTime Time { get; }
    }
}