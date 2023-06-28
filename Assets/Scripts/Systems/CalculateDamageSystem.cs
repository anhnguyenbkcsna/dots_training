using Components;
using Unity.Entities;

public partial struct CalculateDamageSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        foreach (var (dmgComponent, enemyComponent) in SystemAPI.Query<RefRW<DamageComponent>, RefRW<EnemyComponent>>())
        {
            enemyComponent.ValueRW.HP -= config.PlayerDamage;
        }
        // Remove Component
    }
}