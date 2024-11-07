using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(CheckDirection))]
public class WalkableEnemy : MonoBehaviour
{
    Rigidbody2D rb;
    CheckDirection checkDirection;
    DetectionZone detectGround;
    Damageable damageable;
    [SerializeField] private float speed = 8f;

    private Vector2 walkDirectionVector = Vector2.right;
    public enum WalkableDirection {Right, Left};
    private WalkableDirection _walkDirection = WalkableDirection.Right;

    #region Properties
    public WalkableDirection WalkDirection { 
        get{
            return _walkDirection;
        } 
        set{
            if(_walkDirection != value){
                //flip sprite
                transform.localScale *= new Vector2(-1, 1);

                //handle vector
                if(value == WalkableDirection.Right){
                    walkDirectionVector = Vector2.right;
                }
                else if(value == WalkableDirection.Left){
                    walkDirectionVector = Vector2.left;
                }
            _walkDirection = value;
            }
        } 
    }

    #endregion

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        checkDirection = GetComponent<CheckDirection>();
        detectGround = GetComponentInChildren<DetectionZone>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate() {
        FlipDirection();
        Movement();    
    }

    //Handle WalkDirection Property
    private void FlipDirection(){
        if(checkDirection.IsGrounded && checkDirection.IsOnWall || checkDirection.IsGrounded && !detectGround.HaveGround){
            if(WalkDirection == WalkableDirection.Right){
                WalkDirection = WalkableDirection.Left;
            }
            else if(WalkDirection == WalkableDirection.Left){
                WalkDirection = WalkableDirection.Right;
            }
            else Debug.LogError("The current walk direction is not valid !!!");
        }
    }

    private void Movement(){
        if(damageable.IsAlive)
            rb.linearVelocity = new Vector2(walkDirectionVector.x * speed, rb.linearVelocity.y);
        else rb.linearVelocity = Vector2.zero;
    }
}
