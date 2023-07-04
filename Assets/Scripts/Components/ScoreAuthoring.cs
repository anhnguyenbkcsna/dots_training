using Components;
using Unity.Entities;
using UnityEngine;

public class ScoreComponentAuthoring : MonoBehaviour
{
    public class ScoreComponentBaker : Baker<ScoreComponentAuthoring>
    {
        public override void Bake(ScoreComponentAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ScoreComponent
            {
                point = 0
            });
        }
    }
}