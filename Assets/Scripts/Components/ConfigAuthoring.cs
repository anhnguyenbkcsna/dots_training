using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class ConfigAuthoring : MonoBehaviour
    {
        // public float[] NumberOfEnemies;
        // public float[] DamageComponent;
        public float level;
        public float enemyDamage;
        public float playerDamage;
        public class ConfigBaker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                // The config doesn't move so I don't want this have LocalTransform
                var config = GetEntity(TransformUsageFlags.None);
                AddComponent(config, new Config{
                    Level = authoring.level,
                    EnemyDamage = authoring.enemyDamage,
                    PlayerDamage = authoring.playerDamage
                });

            }
        }
    }
}