using UnityEngine;

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

        public void Reset()
        {
            moveRightButtonClicked = false;
            moveLeftButtonClicked = false;
        }
    }
}