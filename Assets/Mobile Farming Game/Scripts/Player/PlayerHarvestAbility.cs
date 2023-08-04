using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerHarvestAbility : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform harvestSphere;
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;

    [Header(" Settings ")]
    private CropField currentCropField;
    private bool canHarvest;
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        //WaterParticles.onWaterCollided += WaterCollidedCallback;
        CropField.onFullyHarvested += CropFieldFullyHarvestedCallback;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }


    private void OnDisable()
    {
       // WaterParticles.onWaterCollided -= WaterCollidedCallback;
        CropField.onFullyHarvested -= CropFieldFullyHarvestedCallback;
        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (playerToolSelector.CanHarvest() == false)
            playerAnimator.StopHarvestAnimation();
    }


    private void CropFieldFullyHarvestedCallback(CropField cropField)
    {
        if (cropField == currentCropField)
        {
            playerAnimator.StopHarvestAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CropField" && other.GetComponent<CropField>().IsWatered())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanHarvest())
        {
            if (currentCropField == null)
            {
                currentCropField = cropField;
            }
            playerAnimator.PlayHarvestAnimation();

            if (canHarvest)
                currentCropField.Harvest(harvestSphere);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("CropField") && other.GetComponent<CropField>().IsWatered())
        {
            EnteredCropField(other.GetComponent<CropField>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CropField")
        {
            playerAnimator.StopHarvestAnimation();
            currentCropField = null;
        }
    }

    public void HarvestingStartedCallback() 
    {
        canHarvest = true;
    }

    public void HarvestingStoppedCallback() 
    {
        canHarvest = false;
    }


}

