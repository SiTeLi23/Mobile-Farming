using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerSowAbility : MonoBehaviour
{
    [Header(" Elements ")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;

    [Header(" Settings ")]
    private CropField currentCropField;
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        SeedParticles.onSeedsCollided += SeedsCollidedCallback;
        CropField.onFullySown += CropFieldFullySownCallback;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }


    private void OnDisable()
    {
        SeedParticles.onSeedsCollided -= SeedsCollidedCallback;
        CropField.onFullySown -= CropFieldFullySownCallback;
        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool) 
    {
        if (playerToolSelector.CanSow() == false)
            playerAnimator.StopSowAnimation();
    }

    private void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        if (currentCropField == null) return;

        currentCropField.SeedsCollidedCallback(seedPositions);
    }

    private void CropFieldFullySownCallback(CropField cropField) 
    {
        if(cropField == currentCropField) 
        {
            playerAnimator.StopSowAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CropField" && other.GetComponent<CropField>().IsEmpty()) 
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void EnteredCropField(CropField cropField) 
    {
        if (playerToolSelector.CanSow())
        {
            playerAnimator.PlaySowAnimation();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("CropField") && other.GetComponent<CropField>().IsEmpty()) 
        {
            EnteredCropField(other.GetComponent<CropField>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CropField")
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }


}
