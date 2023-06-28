using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public partial struct EnemySpawnerComponent:IComponentData
    {
        public int NumberOfEnemy;
        public Entity Prefab;
        public float SpawnRate;
        public float Rows;
        public float3 startPoint;
        public float3 endPoint;
    }
}
