using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestScene.DotProduct{
    public class DP_Canvas : MonoBehaviour
    {
        public Transform _player;
        public Transform _enemy;
        public TextMeshProUGUI dot;

        // Update is called once per frame
        void Update()
        {
            dot.text = "Dot = " + Vector3.Dot((Vector2)_player.position + Vector2.up, _enemy.position).ToString();
        }
    }
}