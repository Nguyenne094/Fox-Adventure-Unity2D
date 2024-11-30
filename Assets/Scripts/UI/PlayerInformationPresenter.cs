using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerInformationPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text cherryTMP;
        [SerializeField] private TMP_Text heartTMP;
        private PlayerInformation _playerInformation;
        public PlayerInformation PlayerInformation => _playerInformation;

        public void Initialize(int initialCherry, int maxHealth) 
        {
            _playerInformation = new PlayerInformation(initialCherry, maxHealth);
            _playerInformation.OnHeartChanged += UpdateHeartView;
            _playerInformation.OnCherryChanged += UpdateCherryView;
        }


        private void OnDestroy()
        {
            if (_playerInformation != null)
            {
                _playerInformation.OnHeartChanged -= UpdateHeartView;
                _playerInformation.OnCherryChanged -= UpdateCherryView;
            }
        }

        public void UpdateHeartView()
        {
            heartTMP.SetText(_playerInformation.CurrentHeart.ToString());
        }

        public void UpdateCherryView()
        {
            Debug.Log(_playerInformation.Cherry);
            cherryTMP.SetText(_playerInformation.Cherry.ToString());
        }

        public void Damage() => _playerInformation.DecreaseHeart();
        public void Heal() => _playerInformation.IncreaseHeart();
        public void CollectCherry() => _playerInformation.IncreaseCherry();
        public void RemoveCherry() => _playerInformation.DecreaseCherry();
        public bool PlayerIsAlive() => _playerInformation.CurrentHeart > 0;
        public bool HaveCherry() => _playerInformation.Cherry > 0;
    }
}