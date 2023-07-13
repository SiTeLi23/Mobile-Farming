using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header(" Settings ")]
    [SerializeField] float moveFactor;
    private Vector3 clickedPosition;
    private Vector3 move;
    private bool canControl;
    void Start()
    {
        HideJoystick();
    }

  
    void Update()
    {
        if (canControl) 
        {
            ControlJoystick();
        }
         
    }

    public void ClickedOnJoystickZoneCallback() 
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;

        if(joystickOutline.gameObject.activeInHierarchy == false) 
        {
            ShowJoystick();
        }
    }

    private void ShowJoystick() 
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }

    private void HideJoystick() 
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;
        move = Vector3.zero;
    }

    private void ControlJoystick() 
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPosition;

        //move factor should be hald the width of the screen size
        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;
        //make sure the knob won't move outside of the outline
        moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2);

        move = direction.normalized * moveMagnitude;
        Vector3 targetPosition = clickedPosition + move;
        //moving knob
        joystickKnob.position = targetPosition;

        if (Input.GetMouseButtonUp(0)) 
        {
            HideJoystick();
        }
    }

    public Vector3 GetMoveVector() 
    {
        return move;
    }
}
