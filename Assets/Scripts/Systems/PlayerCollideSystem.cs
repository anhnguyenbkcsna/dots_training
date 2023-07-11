using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace Systems
{
    public partial struct PlayerCollideSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ControlledMovingComponent>();
            state.RequireForUpdate<EnemyBulletComponent>();
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<Config>();
        }
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            var config = SystemAPI.GetSingleton<Config>();
            state.Dependency = new CheckPlayerCollisionJob
            {
                PlayerLookup = state.GetComponentLookup<ControlledMovingComponent>(),
                EnemyBulletLookup = state.GetComponentLookup<EnemyBulletComponent>(),
                Ecb = ecb,
                Damage = config.EnemyDamage
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            
            // Wait until the Dependency complete task.
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
        public struct CheckPlayerCollisionJob : ITriggerEventsJob
        {
            public ComponentLookup<ControlledMovingComponent> PlayerLookup;
            public ComponentLookup<EnemyBulletComponent> EnemyBulletLookup;
            public EntityCommandBuffer Ecb;

            public float Damage;
            private bool isPlayer(Entity e)
            {
                return PlayerLookup.HasComponent(e);
            }

            private bool isEnemyBullet(Entity e)
            {
                return EnemyBulletLookup.HasComponent(e);
            }
            public void Execute(TriggerEvent triggerEvent)
            {
                var isPlayerA = isPlayer(triggerEvent.EntityA);
                var isPlayerB = isPlayer(triggerEvent.EntityB);
                var isEnemyBulletA = isEnemyBullet(triggerEvent.EntityA);
                var isEnemyBulletB = isEnemyBullet(triggerEvent.EntityB);

                bool destroyableA = false;
                bool destroyableB = false;

                if ((isEnemyBulletA && isPlayerB) || (isEnemyBulletB && isPlayerA))
                {
                    // Player Receive Damage
                    if (isPlayerA)
                    {
                        if (PlayerLookup.IsComponentEnabled(triggerEvent.EntityA))
                        {
                            
                            Ecb.AddComponent(triggerEvent.EntityA, new DamageComponent
                            {
                                PlayerDamage = Damage
                            });
                        }
                    }
                    else
                    {
                        if (PlayerLookup.IsComponentEnabled(triggerEvent.EntityB))
                        {
                            Ecb.AddComponent(triggerEvent.EntityB, new DamageComponent
                            {
                                PlayerDamage = Damage
                            });
                        }
                    }
                    // ----- Bullet -----
                    if (isEnemyBulletA)
                    {
                        // if (EnemyBulletLookup.IsComponentEnabled((triggerEvent.EntityA)))
                        // {
                        //     EnemyBulletLookup.SetComponentEnabled(triggerEvent.EntityA, false);
                        // }
                        destroyableA = true;
                    }
                    else
                    {
                        // if (EnemyBulletLookup.IsComponentEnabled((triggerEvent.EntityB)))
                        // {
                        //     EnemyBulletLookup.SetComponentEnabled(triggerEvent.EntityB, false);
                        // }    
                        destroyableB = true;
                    }
                    
                    // Add component Destroy to Destroy the Enemy bullet
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
        }
    }
}