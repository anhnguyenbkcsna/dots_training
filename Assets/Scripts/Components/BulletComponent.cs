using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public partial struct BulletComponent:IComponentData, IEnableableComponent
    {
        public float speed;
        public float3 direction;
        public float bulletId;

        public Entity nearestEnemy;
        public float minDistance; //if distance less than this mean collide

        //tagcollided;
    }
}