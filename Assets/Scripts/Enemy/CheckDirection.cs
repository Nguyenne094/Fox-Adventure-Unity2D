using UnityEngine;

public class CheckDirection : MonoBehaviour
{
    Animator animator;
    Collider2D touchingCollider;
    [SerializeField] ContactFilter2D contactFilter2D;

    RaycastHit2D[] groundHits;
    RaycastHit2D[] wallHits;
    RaycastHit2D[] ceilingHits;

    [SerializeField] private float groundDistannce = 0.05f;
    [SerializeField] private float wallDistannce = 0.05f;
    [SerializeField] private float ceilingDistannce = 0.1f;
    private bool _isGrounded = true;
    private bool _isOnWall = false;
    private bool _isOnCeiling = false;

    private Vector2 wallDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    #region Properties

    public bool IsGrounded { 
        get{
            return _isGrounded;
        } 
        set
        {
            _isGrounded = value;
            animator.SetBool(AnimationString.isGrounded, value);
        } 
    }

    public bool IsOnWall { 
        get{
            return _isOnWall;
        } 
        set
        {
            _isOnWall = value;
            animator.SetBool(AnimationString.isOnWall, value);
        } 
    }

    public bool IsOnCeiling { 
        get{
            return _isOnCeiling;
        } 
        set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationString.isOnCeiling, value);
        } 
    }

    #endregion

    private void Awake() {
        groundHits = new RaycastHit2D[5];
        wallHits = new RaycastHit2D[5];
        ceilingHits = new RaycastHit2D[5];
        touchingCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        IsGrounded = touchingCollider.Cast(Vector2.down, contactFilter2D, groundHits, groundDistannce) > 0;
        IsOnWall = touchingCollider.Cast(wallDirection, contactFilter2D, wallHits, wallDistannce) > 0;
        IsOnCeiling = touchingCollider.Cast(Vector2.up, contactFilter2D, ceilingHits, ceilingDistannce) > 0;
    }
}
