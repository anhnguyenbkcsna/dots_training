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
            state.RequireForUpdate<StartGameCommand>();
            state.RequireForUpdate<ContinueGameCommand>();
        }
        public void OnUpdate(ref SystemState state)
        {
            foreach (var direction in SystemAPI.Query<RefRW<EnemySpawnerComponent>>())
            {
                foreach (var (tf, moving, range) in SystemAPI.Query<RefRW<LocalTransform>
                             , RefRO<MovingComponent>, RefRO<MovingRange>>().WithNone<ControlledMovingComponent>())
                {
                    tf.ValueRW.Position.x += moving.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime * direction.ValueRW.Direction;
                    if (tf.ValueRW.Position.x < range.ValueRO.minAxis)
                    {
                        Debug.Log("Change direction - min");
                        tf.ValueRW.Position.x = range.ValueRO.minAxis;
                        direction.ValueRW.Direction = -direction.ValueRW.Direction;
                    }
                    if (tf.ValueRW.Position.x > range.ValueRO.maxAxis)
                    {
                        Debug.Log("Change direction - max");
                        tf.ValueRW.Position.x = range.ValueRO.maxAxis;
                        direction.ValueRW.Direction = -direction.ValueRW.Direction;
                    }
                }
            }
        }
    }
}