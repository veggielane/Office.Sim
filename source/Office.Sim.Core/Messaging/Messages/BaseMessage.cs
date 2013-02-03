using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office.Sim.Core.Messaging.Messages
{
    public abstract class BaseMessage : IMessage
    {
        public DateTime Time { get; private set; }
        protected BaseMessage()
        {
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return "[{0}] <{1}>".Fmt(Time, GetType().Name);
        }
    }
}
