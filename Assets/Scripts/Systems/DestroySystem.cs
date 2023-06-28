using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public partial struct DestroySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer _ecb = new EntityCommandBuffer(Allocator.TempJob);

            // Access each entity has DestroyComponent and Destroy using ecb.
            foreach (var (destroyComponent, entity) in SystemAPI.Query<RefRO<DestroyComponent>>().WithEntityAccess() )
            {
                _ecb.DestroyEntity(entity);
            }

            foreach (var (enemyComponent, e) in SystemAPI.Query<RefRO<EnemyComponent>>().WithEntityAccess())
            {
                
                if (enemyComponent.ValueRO.HP <= 0f)
                {
                    _ecb.DestroyEntity(e);
                }
            }
            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }
    }
}