using Unity.Entities;

namespace Components
{
    public struct EnemyComponent:IComponentData, IEnableableComponent
    {
        public float HP;
    }
}