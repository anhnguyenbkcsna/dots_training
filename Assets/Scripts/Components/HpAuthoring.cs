using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class HpAuthoring : MonoBehaviour
    {

        public int HP;
        public class HpAuthoringBaker : Baker<HpAuthoring>
        {
            public override void Bake(HpAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HpEntity()
                {
                    HP = authoring.HP
                });
            }
        }
    }
}