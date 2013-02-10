using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Messaging;
using Office.Sim.Core.Timing;
using Veg.Maths;

namespace Office.Sim.Core.GameObjects
{
    public interface IGameObject : IHasMessageBus
    {
        Guid Id { get; }
        void Update(ITick delta);
        Mat4 Transformation { get; set; }
    }
}
