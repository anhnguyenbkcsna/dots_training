using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class EnemyComponentAuthoring : MonoBehaviour
    {
        public float hp;
        public class EnemyComponentBaker : Baker<EnemyComponentAuthoring>
        {
            public override void Bake(EnemyComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyComponent
                {
                    HP = authoring.hp
                });
            }
        }
    }
}