using CortexDeveloper.ECSMessages.Service;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UIScript
{
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(StartGame);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            // There are 3 lifetimes of message: AliveForOneFrame(), AliveForSeconds(60f), AliveForUnlimitedTime()
            MessageBroadcaster
                .PrepareMessage()
                .AliveForUnlimitedTime() // Set alive for whole game
                .PostImmediate(entityManager,
                    new StartGameCommand
                    {
                        Start = true
                    });
            MessageBroadcaster
                .PrepareMessage()
                .AliveForUnlimitedTime() // Set alive for whole game
                .PostImmediate(entityManager,
                    new ContinueGameCommand()
                    {
                        Continue = true
                    });
            this.gameObject.SetActive(false);
        }
    }
}