﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CharacterHead : CharacterControllable
{
    Action<float> MovementHorizontal;
    Action<float> MovementVertical;
    Action<float> RotateHorizontal;
    Action<float> RotateVertical;
    Action Dash;
    Action ChildrensUpdates;

    Action OnBlock;
    Action UpBlock;
    Action Parry;

    CharacterBlock charBlock;
    #region SCRIPT TEMPORAL, BORRAR
    Action<float> changeCDDash; public void ChangeDashCD(float _cd) => changeCDDash.Invoke(_cd);
    #endregion


    [Header("Character Head")]
    [SerializeField] bool directionalDash;

    [SerializeField] float speed;
    [SerializeField] float dashTiming;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDeceleration;
    [SerializeField] float dashCD;
    [SerializeField] float _timerOfParry;
    [SerializeField] Transform rot;

    [SerializeField] LifeSystem lifesystem;

    Func<bool> InDash;
    //el head va a recibir los inputs
    //primero pasa por aca y no directamente al movement porque tal vez necesitemos extraer la llamada
    //para visualizarlo con algun feedback visual

    [SerializeField]
    Text txt;


    private void Awake()
    {
        //        Animator anim = GetComponent<Animator>();

        var move = new CharacterMovement(GetComponent<Rigidbody>(), rot, IsDirectionalDash/*,anim*/).
            SetSpeed(speed).SetTimerDash(dashTiming).SetDashSpeed(dashSpeed).
            SetDashCD(dashCD).SetRollDeceleration(dashDeceleration);

        MovementHorizontal += move.LeftHorizontal;
        MovementVertical += move.LeftVerical;
        RotateHorizontal += move.RightHorizontal;
        RotateVertical += move.RightVerical;
        Dash += move.Roll;
        InDash += move.IsDash;
        ChildrensUpdates += move.OnUpdate;


        charBlock = new CharacterBlock(_timerOfParry);
        OnBlock += charBlock.OnBlockDown;
        UpBlock += charBlock.OnBlockUp;
        Parry += charBlock.Parry;
        ChildrensUpdates += charBlock.OnUpdate;


        #region SCRIPT TEMPORAL, BORRAR
        changeCDDash += move.ChangeDashCD;
        #endregion
    }

    bool IsDirectionalDash()
    {
        return directionalDash;
    }

    public void ChangeDashForm()
    {
        directionalDash = !directionalDash;
        txt.text = "Directional Dash = " + directionalDash.ToString();
    }
    // esto es para testing //LUEGO QUE CUMPLA SU FUNCION... BORRAR


    private void Update()
    {
        ChildrensUpdates();

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            RollDash();
    }

    public void EVENT_OnBlocking()
    {
        OnBlock();
    }
    public void EVENT_UpBlocking()
    {
        UpBlock();
    }
    public void EVENT_Parry()
    {
        Parry();
    }

    public void EVENT_OnAttackBegin() { }
    public void EVENT_OnAttackEnd() { }

    //Joystick Izquierdo, Movimiento
    public void LeftHorizontal(float axis)
    {
        if (!InDash())
            MovementHorizontal(axis);
    }

    public void LeftVerical(float axis)
    {
        if (!InDash())
            MovementVertical(axis);
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    {
        if (!InDash())
            RotateHorizontal(axis);
    }
    public void RightVerical(float axis)
    {
        if (!InDash())
            RotateVertical(axis);
    }

    public void RollDash()
    {
        if (!InDash())
            Dash();

    }

    protected override void OnUpdateEntity() { }
    public override void TakeDamage(int dmg)
    {
        if (InDash())
            return;

        if (charBlock.onParry)
        {
            Debug.Log("Parry logrado");
            return;
        }
        else if (charBlock.onBlock)
        {
            Debug.Log("Blocked but you lost resistance" + dmg);
            return;
        }
        else
        {
            lifesystem.Hit(5);
            return;
        }

    }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
}
