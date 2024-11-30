using System;
using UnityEngine;

namespace UI
{
    public class PlayerInformation
    {
        private int _cherry;
        private int _currentHeart;
        private int _maxHeart;

        public event Action OnHeartChanged;
        public event Action OnCherryChanged;

        public PlayerInformation(int cherry, int maxHeart)
        {
            _cherry = cherry;
            _maxHeart = maxHeart;
            _currentHeart = maxHeart;
        }

        public int Cherry 
        { 
            get => _cherry; 
            set 
            {
                _cherry = Mathf.Clamp(value, 0, int.MaxValue);
                OnCherryChanged?.Invoke(); 
            } 
        }
        public int CurrentHeart 
        { 
            get => _currentHeart; 
            set 
            {
                _currentHeart = Mathf.Clamp(value, 0, _maxHeart);
                OnHeartChanged?.Invoke(); 
            } 
        }
        public int MaxHeart => _maxHeart;


        public void IncreaseHeart(int amount = 1) => CurrentHeart += amount;
        public void DecreaseHeart(int amount = 1) => CurrentHeart -= amount;
        public void IncreaseCherry(int amount = 1) => Cherry += amount;
        public void DecreaseCherry(int amount = 1) => Cherry -= amount;
    }
}