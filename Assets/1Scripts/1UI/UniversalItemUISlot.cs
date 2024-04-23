using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalItemUISlot : MonoBehaviour
{
    [SerializeField] private Image _icon;

    [SerializeField] private Image _frame;


    [SerializeField] private Button _button;

    private int _id;



    public void Setup(InventoryItem item, System.Action<int> callback)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => callback.Invoke(_id));

        _icon.sprite = item.Icon;



    }

}
