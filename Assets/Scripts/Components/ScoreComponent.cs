using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct ScoreComponent : IComponentData
    {
        public float point;
    }
}