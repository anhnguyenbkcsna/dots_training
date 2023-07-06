using Components;
using UIScript;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial struct BulletMovingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StartGameCommand>();
            state.RequireForUpdate<ContinueGameCommand>();
        }
        public void OnUpdate(ref SystemState state)
        {
            new MovingJob { deltaTime = SystemAPI.Time.DeltaTime }.ScheduleParallel();
        }


        public partial struct MovingJob : IJobEntity
        {
            public float deltaTime;

            void Execute(RefRW<LocalTransform> tf, RefRO<BulletComponent> bullet)
            {
                tf.ValueRW.Position += bullet.ValueRO.direction * bullet.ValueRO.speed * deltaTime;
            }
        }
    }
}