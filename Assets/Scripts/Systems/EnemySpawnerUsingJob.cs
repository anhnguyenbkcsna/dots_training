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
            if (_lastSpawner < 0 && (enemySpawnerComponent.Line || enemySpawnerComponent.Rectangle))
            {
                float3 startPoint = new float3(-10f, 0f, 15f);
                float3 endPoint = new float3(5f, 0f, 0f);
                float numberToSpawn = enemySpawnerComponent.NumberOfEnemy;
                var prefab = enemySpawnerComponent.Prefab;
                
                state.Dependency = new EnemySpawnerJob
                {
                    Ecb = ecb,
                    StartPoint = startPoint,
                    Distance = (endPoint - startPoint) / numberToSpawn,
                    NumberToSpawn = numberToSpawn,
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
        public float3 Distance;
        public float NumberToSpawn;
        public EntityCommandBuffer Ecb;
        public Entity Prefab;
        private void Execute(EnemySpawnerComponent enemySpawnerComponent)
        {
            if (enemySpawnerComponent.Line)
            {
                for (float i = 0f; i < NumberToSpawn; i++)
                {
                    var e = Ecb.Instantiate(Prefab);
                    Ecb.SetComponent(e, new LocalTransform{
                        Position = StartPoint, 
                        Scale = 1f,
                        Rotation = Quaternion.identity
                    });
                    StartPoint += Distance;
                }
            }
        }
    }
}