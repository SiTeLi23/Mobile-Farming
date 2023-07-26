using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem waterParticles;

    [Header(" Settings ")]
    [SerializeField] float moveSpeedMultiplier = 1f;
    void Start()
    {
        
    }

 
    void Update()
    {
        
    }

    public void ManageAnimation(Vector3 moveVector) 
    {
       if(moveVector.magnitude > 0) 
        {
            animator.SetFloat("moveSpeed", moveVector.magnitude * moveSpeedMultiplier);
            PlayRunAnimation();

            animator.transform.forward = moveVector.normalized;
        }
        else 
        {
            PlayIdleAnimation();
        }
    }

    private void PlayRunAnimation() 
    {
        animator.Play("Run");
    }

    private void PlayIdleAnimation() 
    {
        animator.Play("Idle");
    }

    public void PlaySowAnimation()
    {
        animator.SetLayerWeight(1, 1);
    }

    public void StopSowAnimation()
    {
        animator.SetLayerWeight(1, 0);
    }

    public void PlayWaterAnimation()
    {
        animator.SetLayerWeight(2, 1);
    }

    public void StopWaterAnimation()
    {
        animator.SetLayerWeight(2, 0);
        waterParticles.Stop();
    }
}
