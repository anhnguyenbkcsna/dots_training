using Unity.Entities;
using Components;
using Unity.Transforms;

namespace Systems
{
    public partial struct MovingSystem:ISystem
    {

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (tf, moving, range) in SystemAPI.Query<RefRW<LocalTransform>
                         , RefRO<MovingComponent>, RefRO<MovingRange>>().WithNone<ControlledMovingComponent>())
            {
                tf.ValueRW.Position.x += moving.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
                if (tf.ValueRW.Position.x < range.ValueRO.minX)
                {
                    tf.ValueRW.Position.x = range.ValueRO.minX;
                }
                 if (tf.ValueRW.Position.x > range.ValueRO.maxX)
                {
                    tf.ValueRW.Position.x = range.ValueRO.minX;
                }
            }
        }
    }
}