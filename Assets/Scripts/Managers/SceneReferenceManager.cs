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

        public PlayerRpc Player { get; set; }

        public override void Awake()
        {
            base.Awake();
            this.CheckNullReferences();
        }

        public void SetupPlayerReference(PlayerRpc playerRpc)
        {
            this.Player = playerRpc;
            _cinemachineCamera.Follow = playerRpc.transform;

            //UI
            _uiManager.Player = playerRpc;

            var playerPresenter = playerRpc.GetComponent<PlayerPresenter>();
            var playerTouchController = playerRpc.GetComponent<PlayerTouchController>();

            _jumpBtn.onClick.AddListener(playerRpc.OnJump);
            _fireBtn.onClick.AddListener(playerRpc.OnFire);
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

        public void ClearPlayerReference()
        {
            Player = null;
            _cinemachineCamera.Follow = null;

            //UI
            _uiManager.Player = null;

            _jumpBtn.onClick.RemoveAllListeners();
            _fireBtn.onClick.RemoveAllListeners();

            //Buttons
            _moveLeftBtn.GetComponent<EventTrigger>()?.triggers.Clear();
            _moveRightBtn.GetComponent<EventTrigger>()?.triggers.Clear();
        }
    }
}