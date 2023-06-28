using Unity.Entities;

namespace Components
{
    public partial struct MovingRange:IComponentData
    {
        public float minAxis;
        public float maxAxis;
    }
}