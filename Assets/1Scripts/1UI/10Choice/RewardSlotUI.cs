using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlotUI : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private Image _frame;


    [SerializeField] private TMPro.TMP_Text _text;


    public void Setup(RewardSlot rewardSlot)
    {

        // _icon.sprite = rewardSlot.Value;

        _text.text = rewardSlot.Value + "  " + rewardSlot.RewardType + "   " + rewardSlot.RequiredTag; 
    }

}
