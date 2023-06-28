using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems
{
    public partial struct EnemySpawner : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // Init path
            float3 startPoint = new float3(0f, 0f, 0f);
            float3 endPoint = new float3(15f, 0f, 0f);
            float numberToSpawn = 0f;

            // Query to count enemy
            EntityQuery enemyQuery;
            enemyQuery = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<EnemyComponent>());
            float numberEnemy = enemyQuery.CalculateEntityCount();
            
            foreach (var enemySpawner in SystemAPI.Query<RefRW<EnemySpawnerComponent>>())
            {
                numberToSpawn = enemySpawner.ValueRO.NumberOfEnemy;
                // If the system has spawned, set value number of enemy to spawn to 0
                enemySpawner.ValueRW.NumberOfEnemy = 0;
            }
            
            if (numberToSpawn == 0f && numberEnemy == 0) // && currentEnemy = 0
            {
                // spawn next level
                numberToSpawn = 5;
                // return;
            }

            float3 EnemyDistance = (endPoint - startPoint) / numberToSpawn;
            foreach (var (enemySpawner, tf) in SystemAPI.Query<RefRO<EnemySpawnerComponent>, RefRW<LocalTransform>>())
            {
                for (float i = 0f;i < numberToSpawn;i++)
                {
                    var newEnemyEntity = state.EntityManager.Instantiate(enemySpawner.ValueRO.Prefab);
                    state.EntityManager.SetComponentData(newEnemyEntity, new LocalTransform
                    {
                        Position = startPoint,
                        Scale = 1f,
                        Rotation = Quaternion.identity
                    });
                    startPoint += EnemyDistance;
                }
            }
        }
    }
}