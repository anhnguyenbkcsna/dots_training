using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct EnemySpawnerUsingJob : ISystem
    {
        private float _lastSpawner;
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemySpawnerComponent>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var enemySpawnerComponent = SystemAPI.GetSingleton<EnemySpawnerComponent>();
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            var prefab = enemySpawnerComponent.Prefab;
            
            // Query to count enemy
            // EntityQuery enemyQuery;
            // enemyQuery = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<EnemyComponent>());
            // float numberEnemy = enemyQuery.CalculateEntityCount();

            if (_lastSpawner < 0)
            {
                float enemyPerRow = enemySpawnerComponent.NumberOfEnemy;
                float rows = enemySpawnerComponent.Rows;

                // 2 points in rectangle diagonal
                float3 startPoint = enemySpawnerComponent.startPoint;
                float3 endPoint = enemySpawnerComponent.endPoint;
                
                // Calculate the distance between 2 enemies
                float3 distanceX = (startPoint - endPoint) / enemyPerRow;
                float3 distanceZ = (startPoint - endPoint) / rows;
                state.Dependency = new EnemySpawnerJob
                {
                    Ecb = ecb,
                    StartPoint = startPoint,
                    DistanceX = distanceX.x,
                    DistanceZ = distanceZ.z,
                    EnemyPerRow = enemyPerRow,
                    Rows = rows,
                    Prefab = prefab
                }.Schedule(state.Dependency);
                
                state.Dependency.Complete();
                ecb.Playback(state.EntityManager);
                ecb.Dispose();
                _lastSpawner = enemySpawnerComponent.SpawnRate;
            }
            _lastSpawner -= SystemAPI.Time.DeltaTime;
        }
    }
    public partial struct EnemySpawnerJob : IJobEntity
    {
        public float3 StartPoint;
        public float DistanceX;
        public float DistanceZ;
        public float EnemyPerRow;
        public float Rows;
        public EntityCommandBuffer Ecb;
        public Entity Prefab;
        // Pass reference to run once ? Cause there is only 1 EnemySpawnerComponent
        private void Execute(EnemySpawnerComponent enemySpawnerComponent)
        {
            // Debug.Log(DistanceX);
            for (float row = 0f; row < Rows; row++)
            {
                for (float i = 0f; i < EnemyPerRow; i++)
                {
                    var e = Ecb.Instantiate(Prefab);
                    var offset = new float3(DistanceX * i, 0, DistanceZ * row);

                    Ecb.SetComponent(e, new LocalTransform{
                        Position = StartPoint + offset,
                        Scale = 1f,
                        Rotation = Quaternion.identity
                    });
                }
            }
        }
    }
}