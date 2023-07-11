using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class EnemyBulletComponentAuthoring : MonoBehaviour
    {
        public float bulletId;
        public class EnemyBulletComponentBaker : Baker<EnemyBulletComponentAuthoring>
        {
            public override void Bake(EnemyBulletComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // Bullet speed init 
                AddComponent(entity, new EnemyBulletComponent { bulletId = authoring.bulletId });
            }
        }
    }
}