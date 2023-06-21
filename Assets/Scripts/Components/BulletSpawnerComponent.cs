using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public partial struct BulletSpawnerComponent:IComponentData
    {
        public Entity prefab;
        public float speed;

        public float spawnSpeed;

        public float lastSpawnedTime;

        // public float3 offset;

    }
}