using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace UIScript
{
    public partial struct StartGameCommand : IComponentData, IMessageComponent
    {
        public bool Start {
            get;
            set;
        }
    }
}