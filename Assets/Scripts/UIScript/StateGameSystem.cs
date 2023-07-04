using Unity.Entities;
using UnityEngine;

namespace UIScript
{ 
    public partial struct StateGameSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StateGameCommand>();       
        }
        
        public void OnUpdate(ref SystemState state)
        {
            new StartGameCommandListenerJob().Schedule();
            
            state.Enabled = false;
        }
    }
    
    public partial struct StartGameCommandListenerJob : IJobEntity
    {
        public void Execute(in StateGameCommand command)
        {
            Debug.Log($"Game started.");
        }
    }
}