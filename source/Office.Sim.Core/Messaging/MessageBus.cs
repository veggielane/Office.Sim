using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Office.Sim.Core.Messaging.Messages;

namespace Office.Sim.Core.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly Subject<IMessage> _subject;
        public IObservable<IMessage> Messages { get; private set; }

        public MessageBus()
        {
            _subject = new Subject<IMessage>();
            Messages = _subject.AsObservable();

        }

        public void Add(IMessage message)
        {
            _subject.OnNext(message);
        }
    }
}