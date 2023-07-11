using Components;
using UIScript;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct BulletSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StartGameCommand>();
            state.RequireForUpdate<ContinueGameCommand>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var isPressedSpace = Input.GetKey(KeyCode.Space);
            // Player bullet spawner
            foreach (var (tf, spawner)
                     in SystemAPI.Query<RefRO<LocalTransform>, RefRW<BulletSpawnerComponent>>().WithAll<ControlledMovingComponent>())
            {
                if (!isPressedSpace)
                {
                    // press Space
                    spawner.ValueRW.lastSpawnedTime = 0;
                }
                else
                {
                    if (spawner.ValueRO.lastSpawnedTime <= 0)
                    {
                        // Spawn Bullet
                        var newBulletE = state.EntityManager.Instantiate(spawner.ValueRO.prefab);
                        state.EntityManager.SetComponentData(newBulletE, new LocalTransform
                        {
                            Position = tf.ValueRO.Position,
                            Scale = 2f,
                            Rotation = Quaternion.identity,
                        });
                        // Set direction for bullet
                        state.EntityManager.SetComponentData(newBulletE, new BulletComponent
                        {
                            speed =  10f, // Should put in config
                            direction = CalculateDirection(tf),
                        });
                        // set cooldown time for bullet spawner.
                        spawner.ValueRW.lastSpawnedTime = spawner.ValueRO.spawnSpeed;
                    }
                    else
                    {
                        spawner.ValueRW.lastSpawnedTime -= SystemAPI.Time.DeltaTime;
                    }
                }
            }

            // Enemy bullet spawner
            foreach (var (tf, spawner)
                     in SystemAPI.Query<RefRO<LocalTransform>, RefRW<BulletSpawnerComponent>>().WithNone<ControlledMovingComponent>())
            {
                if (spawner.ValueRO.lastSpawnedTime <= 0)
                {
                    // Spawn Bullet
                    var newBulletE = state.EntityManager.Instantiate(spawner.ValueRO.prefab);
                    state.EntityManager.SetComponentData(newBulletE, new LocalTransform
                    {
                        Position = tf.ValueRO.Position,
                        Scale = 1f,
                        Rotation = Quaternion.identity,
                    });
                    // Set direction for bullet
                    state.EntityManager.SetComponentData(newBulletE, new EnemyBulletComponent
                    {
                        speed =  3f, // Should put in config
                        direction = new float3(0, -1, 0)
                    });
                    // set cooldown time for bullet spawner.
                    spawner.ValueRW.lastSpawnedTime = spawner.ValueRO.spawnSpeed;
                }
                else
                {
                    spawner.ValueRW.lastSpawnedTime -= SystemAPI.Time.DeltaTime;
                }
            }
        }
        
        private float3 CalculateDirection(RefRO<LocalTransform> tf)
        {
            // return the forward direction of spawner
            return tf.ValueRO.Forward();
            // return new float3(0, 1, 0);
        }   
    }
}