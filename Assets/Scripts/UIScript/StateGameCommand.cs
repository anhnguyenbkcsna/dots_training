using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace UIScript
{
    public struct StateGameCommand : IComponentData, IMessageComponent
    {
        public float State {
            get;
            set;
        }
    }
}