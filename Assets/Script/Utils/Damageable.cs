using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Damageable : MonoBehaviour, IDamageable
{
    //Health, MaxHealth, Hit, Heal, Invincible, Die

    Animator animator;
    public GameObject restart;
    [SerializeField] private uint _maxHealth = 3;
    [SerializeField] private uint _health;
    private const uint damage = 1;

    private bool _isAlive = true;
    private bool isInvincible = false;

    [SerializeField] private float invincibleTime = 0.25f;
    [SerializeField] private float destroyTime = 1f;
    [SerializeField] private string takeDamageSentence = "Ouch";
    [SerializeField] private string dieSentence = "Huhu";
    [SerializeField] private string healSentence = "Hú";

    #region Properties

    public uint MaxHealth { 
        get{
            return _maxHealth;
        } 
        set{
            _maxHealth = value;
        }
    }
    public uint Health { 
        get{
            return _health;
        } 
        set{
            _health = value;
            if(_health <= 0){
                _isAlive = false;
            }
        } 
    }
    public bool IsAlive { 
        get{
            return _isAlive;
        } 
        set{
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);
        }
    }
    #endregion

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start(){
        _health = _maxHealth;
    }

    private IEnumerator InvincibleTimer(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void Hit(uint damage){
        if(IsAlive && !isInvincible){
            Health -= damage;
            if(Health >= 1){
                animator.SetTrigger(AnimationString.hurtTrigger);
                CharacterEvent.characterDamaged.Invoke(gameObject, takeDamageSentence);
            }
            else if (Health == 0)
            {
                IsAlive = false;
                CharacterEvent.characterDamaged.Invoke(gameObject, dieSentence);
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject, destroyTime);
        if(restart == null)
            return;
        else{
            restart.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && other.CompareTag("Enemy"))
        {
            Hit(damage);
            StartCoroutine(InvincibleTimer());
        }
    }

    public void Heal(uint healthRestore){
        if(IsAlive){
            uint healthCanRestore = (uint)Mathf.Max(MaxHealth - Health, 0);
            healthRestore = (uint)Mathf.Min(healthCanRestore, healthRestore);
            Health += healthRestore;
            CharacterEvent.characterHealed.Invoke(gameObject, healSentence);
        }
    }
}
