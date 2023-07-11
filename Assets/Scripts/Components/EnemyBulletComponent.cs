using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public partial struct EnemyBulletComponent:IComponentData, IEnableableComponent
    {
        public float speed;
        public float3 direction;
        public float bulletId;

        public Entity nearestPlayer;
        public float minDistance; //if distance less than this mean collide

        //tagcollided;
    }
}