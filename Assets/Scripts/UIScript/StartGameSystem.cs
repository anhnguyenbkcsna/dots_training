using Unity.Entities;
using UnityEngine;

namespace UIScript
{ 
    public partial struct StartGameSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<StartGameCommand>();       
        }
        
        public void OnUpdate(ref SystemState state)
        {
            new StartGameCommandListenerJob().Schedule();
            
            state.Enabled = false;
        }
    }
    
    public partial struct StartGameCommandListenerJob : IJobEntity
    {
        public void Execute(in StartGameCommand command)
        {
            Debug.Log($"Game started.");
        }
    }
}