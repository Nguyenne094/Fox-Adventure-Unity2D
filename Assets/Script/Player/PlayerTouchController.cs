using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Player
{
    public class PlayerTouchController : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float jumpForce = 10f;
        
        [SerializeField] private bool moveRightButtonClicked;
        [SerializeField] private bool moveLeftButtonClicked;

        public bool MoveRightButtonClicked => moveRightButtonClicked;
        public bool MoveLeftButtonClicked => moveLeftButtonClicked;

        private bool isMoving;
        private Vector2 movement;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void MoveLeftTouchEnter()
        {
            Debug.Log("Touch Left Enter");
            moveLeftButtonClicked = true;
        }

        public void MoveLeftTouchExit()
        {
            moveLeftButtonClicked = false;
        }
        
        public void MoveRightTouchEnter()
        {
            moveRightButtonClicked = true;
        }

        public void MoveRightTouchExit()
        {
            moveRightButtonClicked = false;
        }
    }
}