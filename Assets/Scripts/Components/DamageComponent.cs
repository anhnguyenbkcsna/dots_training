using Unity.Entities;

namespace Components
{
    public struct DamageComponent : IComponentData
    {
        public Entity TargetEntity;
        public Entity BulletEntity;
    }
}