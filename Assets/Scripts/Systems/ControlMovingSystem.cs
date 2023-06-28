using Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct ControlMovingSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            foreach (var (tf, moving, range) in SystemAPI.Query<RefRW<LocalTransform>
                         , RefRO<MovingComponent>, RefRO<MovingRange>>().WithAll<ControlledMovingComponent>())
            {
                tf.ValueRW.Position.x +=  horizontalInput*moving.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
                if (tf.ValueRW.Position.x < range.ValueRO.minAxis)
                {
                    tf.ValueRW.Position.x = range.ValueRO.minAxis;
                }
                if (tf.ValueRW.Position.x > range.ValueRO.maxAxis)
                {
                    tf.ValueRW.Position.x = range.ValueRO.maxAxis;
                }
            }
        }
        
    }
}