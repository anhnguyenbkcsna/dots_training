using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public partial struct EnemySpawnerComponent:IComponentData
    {
        public int NumberOfEnemy;
        public Entity Prefab;
        public float SpawnRate;
        public bool Rectangle;
        public bool Line;
    }
}
