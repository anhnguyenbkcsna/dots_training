using Unity.Entities;
using UnityEngine;

namespace Components
{
    public partial struct ScoreComponent : IComponentData
    {
        public float point;
    }
}