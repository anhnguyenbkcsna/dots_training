using Unity.Entities;

namespace Components
{
    // [InternalBufferCapacity(8)]
    // public struct PlayerDamage : IBufferElementData
    // {
    //     public float Value;
    // }
    public partial struct Config : IComponentData
    {
        public float Level;
        public float EnemyDamage;
        public float PlayerDamage;
    }
}