using Script.Player;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(CheckDirection)), RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private ParticleSystem dustStep;
    
    [Space(5), Header("Projectile Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectilePos;
    [SerializeField] private Transform projectileParent;
    
    private Rigidbody2D rb;
    private Animator animator;
    private CheckDirection checkDirection;
    private AudioSource jumpSound;
    private PlayerTouchController playerTouchController;

    private Vector2 movement;
    private byte cherry = 0;
    private bool _isRunning = false;
    private bool _isFacingRight = true;

    #region Properties

    public byte Cherry
    {
        get => cherry;
        set => cherry = value;
    }
    
    public bool IsRunning { 
        get{
            return _isRunning;
        } 
        set{
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        }
    }

    public bool IsFacingRight { 
        get{
            return _isFacingRight;
        } 
        private set{
            if(_isFacingRight != value){
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }
    #endregion

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        checkDirection = GetComponent<CheckDirection>();
        dustStep = GetComponentInChildren<ParticleSystem>();
        jumpSound = GetComponentInChildren<AudioSource>();
        playerTouchController = GetComponent<PlayerTouchController>();
    }

   private void FixedUpdate() {
       IsRunning = playerTouchController.MoveLeftButtonClicked ? true : (playerTouchController.MoveRightButtonClicked ? true : false);
       if (!IsRunning)
       {
           return;
       }
       else
       {
           movement = new Vector2(playerTouchController.MoveLeftButtonClicked ? -1 : (playerTouchController.MoveRightButtonClicked ? 1 : 0), movement.y);
           rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
           Debug.Log("Update Touch");
       }
        if(IsRunning && checkDirection.IsGrounded)
            dustStep.Play();
   }


    #region InputHandle
    public void OnJump(InputAction.CallbackContext ctx){
        if(ctx.started && checkDirection.IsGrounded){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpSound?.Stop();
            jumpSound?.Play();
        }
    }

    public void OnFire(InputAction.CallbackContext ctx){
        if(ctx.started && cherry > 0){
            Instantiate(projectile, projectilePos.position, projectilePos.rotation, projectileParent);
            cherry--;
        }
    }
    #endregion
     
    private void FlipDirection(Vector2 moveInput){
        if(moveInput.x > 0 && !IsFacingRight){
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight){
            IsFacingRight = false;
        }
        dustStep.Play();
    }
}
