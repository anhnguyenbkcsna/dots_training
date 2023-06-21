using Unity.Entities;

namespace Components
{
    public partial struct DestroyComponent : IComponentData
    {
        public bool isDestroy;
    }
}