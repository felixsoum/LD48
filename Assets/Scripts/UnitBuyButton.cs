using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitBuyButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text costText;
    [SerializeField] Image background;
    public bool isTower = true;
    public int cost = 1;
    public Action<UnitBuyButton> OnBuyClick;

    void OnValidate()
    {
        costText.text = $"x{cost}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBuyClick?.Invoke(this);
    }

    internal void Deselect()
    {
        background.color = Color.grey;
    }

    internal void Select()
    {
        background.color = Color.white;
    }
}
