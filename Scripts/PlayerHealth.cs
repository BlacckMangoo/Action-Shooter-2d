using BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts;
    PlayerMovement mov;
    Rigidbody2D rb;
    public Animator hit_flash;
    bool isInvincible = false;
    [SerializeField] float iFrames = 0.2f;
    [SerializeField] AudioClip HitSfx;
    [SerializeField] GameObject[] heartObjects;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHearts = maxHearts;
        mov = GetComponent<PlayerMovement>();
    
    }
    private void Update()
    {
        if ((currentHearts < 1))
        {
            mov.PlayerDie();
        }
    }
    public void TakeDamage()
    {
        TriggerHeartBreakAnimation(currentHearts );
        currentHearts--;
        isInvincible = true;
        DOVirtual.DelayedCall(iFrames, SetInvincibility);

    }

    void SetInvincibility()
    {
        isInvincible = false;
    }

    public void OnHit(int pushback)
    {
        if (!isInvincible)
        {
            rb.AddForce((new Vector2(-1, 1) - rb.velocity.normalized) * pushback, ForceMode2D.Impulse);
            hit_flash.Play(0);
            SoundManager.instance.PlaySoundFX(0.5f, HitSfx, transform);
        
        }
        TakeDamage();
    }


    void TriggerHeartBreakAnimation(int heartIndex)
    {
        
            Animator heartAnimator = heartObjects[heartIndex -1].GetComponent<Animator>();
            if (heartAnimator != null)
            {
                heartAnimator.SetTrigger("heartbreak");
            }
            
     }

    
}
