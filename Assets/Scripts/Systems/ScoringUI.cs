using System.Collections;
using Components;
using TMPro;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public class ScoringUI : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private EntityManager _entityManager;
        private Entity _playerEntity;

        
        // This script attached into Canvas/Score to display score in windows.
        // The ScoreComponent contains Score data and authoring to ControlledMovingComponent(PlayerComponent) 
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _playerEntity = _entityManager.CreateEntityQuery(typeof(ControlledMovingComponent)).GetSingletonEntity();
        }
        private void Update()
        {
            var currentPoint = _entityManager.GetComponentData<ScoreComponent>(_playerEntity).point;
            _text.text = $"Score: {currentPoint}";
        }
    }
}