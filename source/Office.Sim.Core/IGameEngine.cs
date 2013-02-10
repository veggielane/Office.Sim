using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Mapping;
using Office.Sim.Core.Messaging;

namespace Office.Sim.Core
{
    public interface IGameEngine : IHasMessageBus, IDisposable
    {
        ILevel Level { get; }
        void LoadLevel(ILevel level);
        void Start();
        void Stop();
    }
}
