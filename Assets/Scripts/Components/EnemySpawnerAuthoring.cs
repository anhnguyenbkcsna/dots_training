using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int NumberOfEnemy;
        public float SpawnRate;
        public float Rows;
        public float3 startPoint;
        public float3 endPoint;
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
                        Rows = authoring.Rows,
                        startPoint = authoring.startPoint,
                        endPoint = authoring.endPoint
                    });
            }
        }
    }
}