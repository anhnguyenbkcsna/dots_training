using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;


namespace Components
{
    public partial struct ChangeMatMeshComponent : IComponentData
    {
        public BatchMeshID MeshId;
        public BatchMaterialID MaterialId;
    }

    public class ChangeMatMeshAuthoring : MonoBehaviour
    {
        // public List<Mesh> Mesh;
        // public List<Material> Material;
        
        public Mesh mesh;
        public Material material;

        public class ChangeMatMeshAuthoringBaker : Baker<ChangeMatMeshAuthoring>
        {
            public override void Bake(ChangeMatMeshAuthoring authoring)
            {
                var hybridRenderer = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EntitiesGraphicsSystem>();
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new ChangeMatMeshComponent
                {
                    MeshId = hybridRenderer.RegisterMesh(authoring.mesh), 
                    MaterialId = hybridRenderer.RegisterMaterial(authoring.material)
                });
            }
        }
    }
}