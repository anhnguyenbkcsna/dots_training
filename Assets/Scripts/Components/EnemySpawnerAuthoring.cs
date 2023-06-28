using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int NumberOfEnemy;
        public float SpawnRate;
        public bool Rectangle;
        public bool Line;
        public class EnemySpawnerComponentBaker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new EnemySpawnerComponent
                    {
                        NumberOfEnemy = authoring.NumberOfEnemy,
                        Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                        SpawnRate = authoring.SpawnRate,
                        Rectangle = authoring.Rectangle,
                        Line = authoring.Line
                    });
            }
        }
    }
}