using Components;
using Unity.Collections;
using Unity.Entities;

public partial struct CalculateDamageSystem : ISystem
{
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
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}