using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Office.Sim.Core.Messaging
{
    public interface IMessageBus
    {
        IObservable<IMessage> Messages { get; }
        void Add(IMessage message);
    }

    public interface IMessage
    {
        DateTime Time { get; }
    }

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

    public interface IHasMessageBus
    {
        IMessageBus Bus { get; }
    }



}

