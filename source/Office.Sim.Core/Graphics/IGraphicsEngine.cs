using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Mapping;

namespace Office.Sim.Core.Graphics
{
    public interface IGraphicsEngine
    {
        void Start();
        void LoadLevel(ILevel level);
        void Stop();
    }
}
