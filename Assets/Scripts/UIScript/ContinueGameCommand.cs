using CortexDeveloper.ECSMessages.Components;
using Unity.Entities;

namespace UIScript
{
    public partial struct ContinueGameCommand : IComponentData, IMessageComponent
    {
        public bool Continue {
            get;
            set;
        }
    }
}