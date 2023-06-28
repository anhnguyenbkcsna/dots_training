using Components;
using Unity.Entities;
using Unity.Rendering;

namespace Systems
{
    public partial struct ChangeMatMeshSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ChangeMatMeshComponent>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var changeMatMesh = SystemAPI.GetSingleton<ChangeMatMeshComponent>();
            foreach (var matmesh in SystemAPI.Query<RefRW<MaterialMeshInfo>>().WithAll<EnemyComponent, ChangeMatMeshTag>())
            {
                matmesh.ValueRW.MaterialID = changeMatMesh.MaterialId;
                matmesh.ValueRW.MeshID = changeMatMesh.MeshId;
            }
        }
    }
}
