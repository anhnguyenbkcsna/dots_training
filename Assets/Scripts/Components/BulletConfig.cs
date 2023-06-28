using Unity.Entities;

namespace Components
{
    public partial struct BulletConfig : IComponentData
    {
        public float BulletId;
        public float BulletDamage;
    }
}