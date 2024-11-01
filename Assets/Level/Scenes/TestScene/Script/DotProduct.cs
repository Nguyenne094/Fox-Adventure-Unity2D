using UnityEngine;

namespace TestScene.DotProduct{
    public class DotProduct : MonoBehaviour
    {
        public Transform _enemy;
        Vector2 normalizePlayer;
        Vector2 normalizeEnemy;

        private void Start() {
            Debug.Log(transform.position.normalized);
        }
    
        /// <summary>
        /// Simulate dot product for detecting enemey
        /// </summary>
        void OnDrawGizmos()
        {
            normalizePlayer = transform.position.normalized;
            normalizeEnemy = _enemy.position.normalized;

            //Draw normalized Circle (radius = 1)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + new Vector3(0, -1, 0),transform.position + new Vector3(0, 1, 0) );

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _enemy.position);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position , normalizeEnemy);
        }
    }
}