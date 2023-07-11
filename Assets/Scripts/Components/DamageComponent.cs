using Unity.Entities;

namespace Components
{
    public struct DamageComponent : IComponentData
    {
        // public Entity TargetEntity;
        // public Entity BulletEntity;
        public float PlayerDamage;
        public float EnemyDamage;
    }
}