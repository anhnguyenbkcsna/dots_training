using CortexDeveloper.ECSMessages.Service;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [RequireComponent(typeof(Button))]
    public class PauseGameButton : MonoBehaviour
    {
        
        private Button _button;
        private TextMeshProUGUI _text;
        private bool _isPause = false;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _button.onClick.AddListener(PauseGame);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PauseGame);
        }

        private void PauseGame()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (_isPause)
            {
                _isPause = !_isPause;
                MessageBroadcaster.RemoveAllMessagesWith<ContinueGameCommand>(entityManager);
                _text.text = "Resume";
            }
            else
            {
                _isPause = !_isPause;
                _text.text = "Pause";
                MessageBroadcaster
                    .PrepareMessage()
                    .AliveForUnlimitedTime() // Set alive for whole game
                    .PostImmediate(entityManager,
                        new ContinueGameCommand()
                        {
                            Continue = true
                        });
            }
        }
    }
}