// using Unity.Entities;
// using Components;
// using Systems;
// using Unity.Collections;
// using Unity.Physics;

// namespace System
// {
// 	public partial struct CalculateDamageSystem : ISystem
//     {
// 		public partial struct CalculateDamageJob : IJobEntity
// 		{
// 			public EntityCommandBuffer ecb;
// 			public Config config;
// 			public bool IsBullet; // false is target.
// 			public void Execute(HpEntity hpEntity)
// 			{

// 			}
// 		}
//     	public void OnUpdate(ref SystemState state)
//         {
// 			var config = SystemAPI.GetSingleton<Config>();

//             EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
//             state.Dependency = new CalculateDamageSystem.CalculateDamageJob
//             {
// 				config = config,
//                 ecb = ecb,
//                 // IsBullet = false
//             }.Schedule(state.Dependency);
            
//             // Wait until the Dependency complete task.
//             state.Dependency.Complete();
//             ecb.Playback(state.EntityManager);
//             ecb.Dispose();
// 		}
//     }
// }