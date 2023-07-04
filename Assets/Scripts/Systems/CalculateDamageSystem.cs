using Components;
using Unity.Collections;
using Unity.Entities;

public partial struct CalculateDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ScoreComponent>();
    }
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        
        foreach (var (enemyComponent, dmgComponent, entity) in 
                 SystemAPI.Query<RefRW<EnemyComponent>, RefRW<DamageComponent>>().WithEntityAccess())
        {
            enemyComponent.ValueRW.HP -= dmgComponent.ValueRW.Damage;
            ecb.RemoveComponent<DamageComponent>(entity);
            if (enemyComponent.ValueRW.HP <= 0f)
            {
                ecb.AddComponent<DestroyComponent>(entity);

                // Instead of this way, we can add tag AddScore and query this tag in other system.
                foreach (var scoring in SystemAPI.Query<RefRW<ScoreComponent>>())
                {
                    // The score could be update base on each type of enemies type base on point in Config later.
                    // Add 1 point in default when enemy dead.
                    scoring.ValueRW.point += 1;
                }
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}