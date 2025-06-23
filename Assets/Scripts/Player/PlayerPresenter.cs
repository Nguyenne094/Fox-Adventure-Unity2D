using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text cherryTMP;
        [SerializeField] private TMP_Text heartTMP;
        private PlayerModel _playerModel;
        public PlayerModel PlayerModel => _playerModel;

        public void Initialize(int initialCherry, int maxHealth) 
        {
            _playerModel = new PlayerModel(initialCherry, maxHealth);
            _playerModel.OnHeartChanged += UpdateHeartView;
            _playerModel.OnCherryChanged += UpdateCherryView;
        }


        private void OnDestroy()
        {
            if (_playerModel != null)
            {
                _playerModel.OnHeartChanged -= UpdateHeartView;
                _playerModel.OnCherryChanged -= UpdateCherryView;
            }
        }

        public void UpdateHeartView()
        {
            heartTMP.SetText(_playerModel.CurrentHeart.ToString());
        }

        private void UpdateCherryView()
        {
            Debug.Log(_playerModel.Cherry);
            cherryTMP.SetText(_playerModel.Cherry.ToString());
        }

        public void Damage() => _playerModel.DecreaseHeart();
        public void Heal() => _playerModel.IncreaseHeart();
        public void CollectCherry() => _playerModel.IncreaseCherry();
        public void RemoveCherry() => _playerModel.DecreaseCherry();
        public bool PlayerIsAlive() => _playerModel.CurrentHeart > 0;
        public bool HaveCherry() => _playerModel.Cherry > 0;
    }
}