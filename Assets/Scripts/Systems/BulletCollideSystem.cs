using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct BulletCollideSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // return;
            // Entity enemy = null;
            foreach (var (bullettf, bullet, bulletEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<BulletComponent>>().WithEntityAccess())
            {
                float distanceToNearestEnemy = math.INFINITY;
                Entity nearestEnemy = Entity.Null;
                //find nearest enemy move to job
                foreach (var (enemytf, enemy, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemyComponent>>().WithEntityAccess())
                {
                    float distanceToEnemy = math.distancesq(bullettf.ValueRO.Position, enemytf.ValueRO.Position);
                    if (distanceToEnemy < distanceToNearestEnemy)
                    {
                        distanceToNearestEnemy = distanceToEnemy;
                        nearestEnemy = enemyEntity;
                    }
                }
                
                // var f1 = new float3();
                // var f2 = new float3();
                // var dist =  math.distancesq(f1, f2);
                if (distanceToNearestEnemy <= bullet.ValueRO.minDistance && nearestEnemy != Entity.Null)
                {
                    //add component "collided" {bullet, enemy}
                    state.EntityManager.SetComponentData(bulletEntity, new ColliderComponent
                    {
                        entityA = bulletEntity, entityB = nearestEnemy
                    });
                }
            }
        }
    }
    
    //collideResutl{bullet, }
}