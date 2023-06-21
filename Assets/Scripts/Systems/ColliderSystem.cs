using Components;
using Unity.Entities;

namespace Systems
{
    public partial struct ColliderSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // Wait until the collider
            state.RequireForUpdate<HpReduce>();
        }
        public void OnUpdate(ref SystemState state)
        {
            // Query the objects have HpReduce component, set new HP by reduce hpReduce
            foreach (var (entity, Reduce) in SystemAPI.Query<RefRW<HpEntity>, RefRO<HpReduce>>())
            {
                entity.ValueRW.HP = entity.ValueRW.HP - Reduce.ValueRO.hpReduce;
            }
        }
        
    }
    
}