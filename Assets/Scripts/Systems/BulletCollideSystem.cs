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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyComponent>();
            state.RequireForUpdate<BulletComponent>();
            state.RequireForUpdate<SimulationSingleton>();
        }
        public struct JobCheckCollision: ITriggerEventsJob
        {
            public ComponentLookup<EnemyComponent> TargetLookup;
            public ComponentLookup<BulletComponent> BulletLookup;
            public EntityCommandBuffer Ecb;

            public float Damage;
            // Support func for Execute
            public bool IsTarget(Entity e)
            {
                return TargetLookup.HasComponent(e);
            }
            public bool IsBullet(Entity e)
            {
                return BulletLookup.HasComponent(e);
            }

            public void Execute(TriggerEvent triggerEvent)
            {
                var isEnemyA = IsTarget(triggerEvent.EntityA);
                var isEnemyB = IsTarget(triggerEvent.EntityB);
                
                var isBulletA = IsBullet(triggerEvent.EntityA);
                var isBulletB = IsBullet(triggerEvent.EntityB);

                bool destroyableA = false;
                bool changeMatA = false;
                bool destroyableB = false;
                bool changeMatB = false;
                
                if ((isEnemyA && isBulletB) || (isEnemyB && isBulletA))
                {
                    if (isEnemyA)
                    {
                        if (TargetLookup.IsComponentEnabled(triggerEvent.EntityA))
                        {
                            
                            Ecb.AddComponent(triggerEvent.EntityA, new DamageComponent
                            {
                                Damage = Damage
                                // TargetEntity = triggerEvent.EntityA,
                                // BulletEntity = triggerEvent.EntityB
                            });
                            changeMatA = true;
                            // destroyableA = true;
                        }
                    }
                    else
                    {
                        if (TargetLookup.IsComponentEnabled(triggerEvent.EntityB))
                        {
                            Ecb.AddComponent(triggerEvent.EntityB, new DamageComponent
                            {
                                Damage = Damage
                                // TargetEntity = triggerEvent.EntityB,
                                // BulletEntity = triggerEvent.EntityA
                            });
                            changeMatB = true;
                            // destroyableB = true;
                        }
                    }
                    // Set Disable for Bullet, then the Bullet will disappear
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

                // Change material
                if (changeMatA)
                {
                    Ecb.AddComponent<ChangeMatMeshTag>(triggerEvent.EntityA);
                }

                if (changeMatB)
                {
                    Ecb.AddComponent<ChangeMatMeshTag>(triggerEvent.EntityB);
                }
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            var config = SystemAPI.GetSingleton<Config>();
            state.Dependency = new JobCheckCollision
            {
                TargetLookup = state.GetComponentLookup<EnemyComponent>(),
                BulletLookup = state.GetComponentLookup<BulletComponent>(),
                Ecb = ecb,
                Damage = config.PlayerDamage
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            
            // Wait until the Dependency complete task.
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            
        }
    }
}