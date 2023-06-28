using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public partial struct EnemySpawnerComponent:IComponentData
    {
        public int NumberOfEnemy;
        public Entity Prefab;
    }
}
