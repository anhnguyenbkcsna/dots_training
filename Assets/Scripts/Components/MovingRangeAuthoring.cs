using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class MovingRangeAuthoring : MonoBehaviour
    {
        public float MinX = -5f;
        public float MaxX = 5f;

        public class MovingRangeBaker : Baker<MovingRangeAuthoring>
        {
            public override void Bake(MovingRangeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MovingRange { minAxis = authoring.MinX, maxAxis = authoring.MaxX });
            }
        }
    }
}