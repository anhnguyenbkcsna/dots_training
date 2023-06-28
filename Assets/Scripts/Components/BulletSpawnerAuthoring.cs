using Unity.Entities;
using UnityEngine;

namespace Components
{
    public class BulletSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public float Speed = 50f;
        public float SpawnSpeed = 0.5f;

        // Dont need offset, fire at firepoint 
        // public Vector3 Offset = new Vector3(0f, 1f, 0f);

        public class BulletSpawnerComponentBaker : Baker<BulletSpawnerAuthoring>
        {
            public override void Bake(BulletSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new BulletSpawnerComponent
                    {
                        prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic), speed = authoring.Speed, spawnSpeed = authoring.SpawnSpeed
                    });
            }
        }
    }
}