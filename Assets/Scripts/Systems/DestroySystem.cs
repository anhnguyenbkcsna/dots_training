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
            EntityCommandBuffer _ecb = new EntityCommandBuffer(Allocator.Temp);

            // Access each entity has DestroyComponent and Destroy using ecb.
            foreach (var (destroyComponent, entity) in SystemAPI.Query<RefRO<DestroyComponent>>().WithEntityAccess() )
            {
                _ecb.DestroyEntity(entity);
            }
            _ecb.Playback(state.EntityManager);
            _ecb.Dispose();
        }
    }
}