using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class ColliderAuthoring : MonoBehaviour
    {
        public class ColliderBaker : Baker<ColliderAuthoring>
        {
            public override void Bake(ColliderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ColliderComponent
                {
                    // Entity Collider is set by ColliderSystem when the collision appear
                    entityA = Entity.Null, entityB = Entity.Null
                });
            }
        }
    }
}