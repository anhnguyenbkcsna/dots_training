using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct BulletSpawnerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // var isPressedSpace = Input.GetKeyDown(KeyCode.Space);
            // Spawn continously without press Space...
            foreach (var (tf, spawner)
                     in SystemAPI.Query<RefRO<LocalTransform>, RefRW<BulletSpawnerComponent>>()
                    )
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    spawner.ValueRW.lastSpawnedTime = 0;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    spawner.ValueRW.lastSpawnedTime = spawner.ValueRO.spawnSpeed;
                }
                else
                {
                    if (spawner.ValueRO.lastSpawnedTime <= 0)
                    {
                        //Spawn Bullet
                        var newBulletE = state.EntityManager.Instantiate(spawner.ValueRO.prefab);
                        state.EntityManager.SetComponentData(newBulletE, new LocalTransform
                        {
                            Position = tf.ValueRO.Position + spawner.ValueRO.offset,
                            Scale = 1f,
                            Rotation = Quaternion.identity,
                        });
                        state.EntityManager.SetComponentData(newBulletE, new BulletComponent
                        {
                            speed =  3f,
                            direction = calculateDirection(tf, ref state),
                        });
                        spawner.ValueRW.lastSpawnedTime = spawner.ValueRO.spawnSpeed;
                    }
                    else
                    {
                        spawner.ValueRW.lastSpawnedTime -= SystemAPI.Time.DeltaTime;
                    }
                }
            }
        }

        private float3 calculateDirection(RefRO<LocalTransform> tf, ref SystemState state)
        {
            // set null direction
            float3 direction = new float3(0, 0, 1);
            return direction;

            // query the character
            foreach (var (characterTf, character) in SystemAPI
                         .Query<RefRO<LocalTransform>, RefRO<ControlledMovingComponent>>())
            {
                // query the firepoint
                foreach (var (firepointTf, firepoint) in SystemAPI
                             .Query<RefRO<LocalTransform>, RefRO<BulletSpawnerComponent>>())
                {
                    // calculate the direction 
                    direction = characterTf.ValueRO.Position - firepointTf.ValueRO.Position;
                }
            }
        }
    }
}