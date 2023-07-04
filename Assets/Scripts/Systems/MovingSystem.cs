using Unity.Entities;
using Components;
using UIScript;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct MovingSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StateGameCommand>();
        }
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (tf, moving, range) in SystemAPI.Query<RefRW<LocalTransform>
                         , RefRO<MovingComponent>, RefRO<MovingRange>>().WithNone<ControlledMovingComponent>())
            {
                tf.ValueRW.Position.y -= moving.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
                if (tf.ValueRW.Position.y < range.ValueRO.minAxis)
                {
                    tf.ValueRW.Position.y = range.ValueRO.maxAxis;
                }
                 if (tf.ValueRW.Position.y > range.ValueRO.maxAxis)
                {
                    tf.ValueRW.Position.y = range.ValueRO.minAxis;
                }
            }
        }
    }
}