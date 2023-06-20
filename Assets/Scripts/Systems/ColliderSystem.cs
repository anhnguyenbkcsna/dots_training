using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Components
{
    public partial struct ColliderSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            
        }
        
        // public partial struct ColliderJob : IJobEntity
        // {
        //     void Execute()
        // }
    }
}