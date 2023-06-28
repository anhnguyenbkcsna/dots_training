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
                tf.ValueRW.Position.z -= moving.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
                if (tf.ValueRW.Position.z < range.ValueRO.minAxis)
                {
                    tf.ValueRW.Position.z = range.ValueRO.maxAxis;
                }
                 if (tf.ValueRW.Position.z > range.ValueRO.maxAxis)
                {
                    tf.ValueRW.Position.z = range.ValueRO.minAxis;
                }
            }
        }
    }
}