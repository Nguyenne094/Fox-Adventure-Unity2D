using Network;
using Script.Player;
using TMPro;
using UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;
using Utils;

namespace Manager
{
    public class SceneReferenceManager : Singleton<SceneReferenceManager>
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        [Header("UI")]
        [SerializeField] private Button _moveLeftBtn;
        [SerializeField] private Button _moveRightBtn;
        [SerializeField] private Button _jumpBtn;
        [SerializeField] private Button _fireBtn;
        [SerializeField] private TMP_Text _cherryTMP;
        [SerializeField] private TMP_Text _heartTMP;
        [SerializeField] private UIManager _uiManager;

        public override void Awake()
        {
            base.Awake();
            this.CheckNullReferences();
        }

        public void SetupPlayerReference(PlayerRpc player)
        {
            _cinemachineCamera.Follow = player.transform;

            //UI
            _uiManager.Player = player;

            var playerPresenter = player.GetComponent<PlayerPresenter>();
            var playerTouchController = player.GetComponent<PlayerTouchController>();

            _jumpBtn.onClick.AddListener(player.OnJump);
            _fireBtn.onClick.AddListener(player.OnFire);
            playerPresenter.CherryTMP = _cherryTMP;
            playerPresenter.HeartTMP = _heartTMP;

            //Buttons
            void AddEventTrigger(Button btn, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
            {
                var eventTrigger = btn.GetComponent<EventTrigger>();
                if (eventTrigger == null)
                    eventTrigger = btn.gameObject.AddComponent<EventTrigger>();

                var entry = new EventTrigger.Entry { eventID = eventType };
                entry.callback.AddListener(action);
                eventTrigger.triggers.Add(entry);
            }

            AddEventTrigger(_moveLeftBtn, EventTriggerType.PointerEnter, _ => playerTouchController.MoveLeftTouchEnter());
            AddEventTrigger(_moveLeftBtn, EventTriggerType.PointerExit, _ => playerTouchController.MoveLeftTouchExit());

            AddEventTrigger(_moveRightBtn, EventTriggerType.PointerEnter, _ => playerTouchController.MoveRightTouchEnter());
            AddEventTrigger(_moveRightBtn, EventTriggerType.PointerExit, _ => playerTouchController.MoveRightTouchExit());
        }
    }
}