﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterInput : MonoBehaviour
{
    public enum InputType { Joystick, Mouse, Other }
    public InputType input_type;

    public Text txt;

    [Header("Movement")]
    public UnityEvFloat LeftHorizontal;
    public UnityEvFloat LeftVertical;
    public UnityEvFloat RightHorizontal;
    public UnityEvFloat RightVertical;
    public UnityEvent Dash;

    public UnityEvent OnBlock;
    public UnityEvent UpBlock;
    public UnityEvent Parry;



    private void Update()
    {
        LeftHorizontal.Invoke(Input.GetAxis("Horizontal"));
        LeftVertical.Invoke(Input.GetAxis("Vertical"));
        if (input_type == InputType.Joystick) JoystickInputs();
        else if (input_type == InputType.Mouse) MouseInputs();
        if (Input.GetButtonDown("Dash")) Dash.Invoke();

        if (Input.GetButtonDown("Block")) OnBlock.Invoke();
        if (Input.GetButtonUp("Block")) UpBlock.Invoke();
        if (Input.GetButtonDown("Parry")) Parry.Invoke();

    }

    public void MouseInputs()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mousePos = (mouseOnScreen - positionOnScreen).normalized;
        RightHorizontal.Invoke(mousePos.x);
        RightVertical.Invoke(mousePos.y);
    }

    public void JoystickInputs()
    {
        RightHorizontal.Invoke(Input.GetAxis("RightHorizontal"));
        RightVertical.Invoke(Input.GetAxis("RightVertical"));
    }


    public void ChangeRotationInput()
    {
        if (input_type == InputType.Mouse) input_type = InputType.Joystick;
        else if (input_type == InputType.Joystick) input_type = InputType.Mouse;
        txt.text = "inputType = " + input_type;
    }

    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}


