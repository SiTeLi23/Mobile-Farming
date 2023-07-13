using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] Animator animator;

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
}
