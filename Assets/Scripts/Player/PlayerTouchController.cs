using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Player
{
    public class PlayerTouchController : MonoBehaviour
    {
        [SerializeField] private bool moveRightButtonClicked;
        [SerializeField] private bool moveLeftButtonClicked;

        public bool MoveRightButtonClicked => moveRightButtonClicked;
        public bool MoveLeftButtonClicked => moveLeftButtonClicked;

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