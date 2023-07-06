using Unity.Entities;
using UnityEngine;

namespace UIScript
{ 
    public partial struct PauseGameSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StartGameCommand>();       
        }
        
        public void OnUpdate(ref SystemState state)
        {
            new PauseGameCommandListenerJob().Schedule();
            state.Enabled = false;
        }
    }
    
    public partial struct PauseGameCommandListenerJob : IJobEntity
    {
        public void Execute(in StartGameCommand command)
        {
            Debug.Log("Pausing....");
        }
    }
}