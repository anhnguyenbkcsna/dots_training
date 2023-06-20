using Unity.Entities;
using UnityEngine;

namespace Components
{
    public partial struct ColliderComponent : IComponentData
    {
        public Entity entityA;
        public Entity entityB;
    }
}