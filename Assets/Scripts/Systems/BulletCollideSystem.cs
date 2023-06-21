using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] // bring physics into 
    [UpdateAfter(typeof(SimulationSystemGroup))] // Simulate then calculate collision
    public partial struct BulletCollideSystem:ISystem
    {
        // public void OnUpdate(ref SystemState state)
        // {
        //     // return;
        //     // Entity enemy = null;
        //     foreach (var (bullettf, bullet, bulletEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<BulletComponent>>().WithEntityAccess())
        //     {
        //         float distanceToNearestEnemy = math.INFINITY;
        //         Entity nearestEnemy = Entity.Null;
        //         //find nearest enemy move to job
        //         foreach (var (enemytf, enemy, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemyComponent>>().WithEntityAccess())
        //         {
        //             float distanceToEnemy = math.distancesq(bullettf.ValueRO.Position, enemytf.ValueRO.Position);
        //             if (distanceToEnemy < distanceToNearestEnemy)
        //             {
        //                 distanceToNearestEnemy = distanceToEnemy;
        //                 nearestEnemy = enemyEntity;
        //             }
        //         }
        //         
        //         // var f1 = new float3();
        //         // var f2 = new float3();
        //         // var dist =  math.distancesq(f1, f2);
        //         if (distanceToNearestEnemy <= bullet.ValueRO.minDistance && nearestEnemy != Entity.Null)
        //         {
        //             //add component "collided" {bullet, enemy}
        //             state.EntityManager.AddComponent<ColliderComponent>(bulletEntity);
        //             // set Collider Entity
        //             state.EntityManager.SetComponentData(bulletEntity, new ColliderComponent
        //             {
        //                 entityA = bulletEntity, entityB = nearestEnemy
        //             });
        //         }
        //     }
        // }
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyComponent>();
            state.RequireForUpdate<BulletComponent>();
            state.RequireForUpdate<SimulationSingleton>();
        }
        public struct JobCheckCollision: ITriggerEventsJob
        {
            public ComponentLookup<EnemyComponent> EnemyLookup;
            public ComponentLookup<BulletComponent> BulletLookup;
            public EntityCommandBuffer Ecb;
            // Support func for Execute
            public bool IsEnemy(Entity e)
            {
                return EnemyLookup.HasComponent(e);
            }
            public bool IsBullet(Entity e)
            {
                return BulletLookup.HasComponent(e);
            }

            public void Execute(TriggerEvent triggerEvent)
            {
                var isEnemyA = IsEnemy(triggerEvent.EntityA);
                var isEnemyB = IsEnemy(triggerEvent.EntityB);
                
                var isBulletA = IsBullet(triggerEvent.EntityA);
                var isBulletB = IsBullet(triggerEvent.EntityB);

                bool destroyableA = false;
                bool destroyableB = false;
                
                if ((isEnemyA && isBulletB) || (isEnemyB && isBulletA))
                {
                    if (isEnemyA)
                    {
                        if (EnemyLookup.IsComponentEnabled(triggerEvent.EntityA))
                        {
                            // Set Component ReduceHP and HPdata
                            // Ecb.AddComponent(triggerEvent.EntityA, new HpReduce
                            // {
                            //     
                            // });
                            Ecb.AddComponent<HpReduce>(triggerEvent.EntityA);
                        }
                    }
                    else
                    {
                        if (EnemyLookup.IsComponentEnabled(triggerEvent.EntityB))
                        {
                            EnemyLookup.SetComponentEnabled(triggerEvent.EntityB, false);
                        }

                    }
                    
                    
                    // Set Disable for Bullet
                    if (isBulletA)
                    {
                        if (BulletLookup.IsComponentEnabled(triggerEvent.EntityA))
                        {
                            BulletLookup.SetComponentEnabled(triggerEvent.EntityA, false);
                        }

                        destroyableA = true;
                    }
                    else
                    {
                        if (BulletLookup.IsComponentEnabled(triggerEvent.EntityB))
                        {
                            BulletLookup.SetComponentEnabled(triggerEvent.EntityB, false);

                        }
                        destroyableB = true;
                    }
                    
                }
            
                // Add component Destroy for DestroySystem
                if (destroyableA)
                {
                    Ecb.AddComponent<DestroyComponent>(triggerEvent.EntityA);
                }

                if (destroyableB)
                {
                    Ecb.AddComponent<DestroyComponent>(triggerEvent.EntityB);
                }
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            state.Dependency = new JobCheckCollision
            {
                EnemyLookup = state.GetComponentLookup<EnemyComponent>(),
                BulletLookup = state.GetComponentLookup<BulletComponent>(),
                Ecb = ecb
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            
            // Wait until the Dependency complete task.
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            
        }
    }
}