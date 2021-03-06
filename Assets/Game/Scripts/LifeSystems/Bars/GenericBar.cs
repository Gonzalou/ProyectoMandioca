﻿using UnityEngine;
using UnityEngine.UI;
public abstract class GenericBar : MonoBehaviour
{
    protected int maxValue;
    protected float scaler;

    [Header("para visualizar la barra")]
    [SerializeField] protected Text porcentaje;
    [SerializeField] protected bool realvalue;

    public void Configure(int maxValue, float scaler)
    {
        this.maxValue = maxValue;
        this.scaler = scaler;
    }

    public void SetValue(float val) => OnSetValue(val);
    protected abstract void OnSetValue(float val);
}
