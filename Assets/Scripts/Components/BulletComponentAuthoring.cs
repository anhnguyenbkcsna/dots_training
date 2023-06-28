using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class BulletComponentAuthoring : MonoBehaviour
    {
        public float Speed;
        public float bulletId;

        public class BulletComponentBaker : Baker<BulletComponentAuthoring>
        {
            public override void Bake(BulletComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BulletComponent { speed = authoring.Speed, bulletId = authoring.bulletId });
            }
        }
    }
}