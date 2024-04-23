using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampHeroSlotUI : MonoBehaviour
{
    [SerializeField] private Image _classIcon;
    [SerializeField] private TMPro.TMP_Text _nicknameText;


    [SerializeField] private GameObject _readyMark;


    private Action _callback; // ??  ?? 

    public void Setup(PlayerData data, Action callback)
    {
        _classIcon.sprite = InfoProvider.instance.GetHeroClass(data.heroData.classId).icon;
        _nicknameText.text = data.nickname;

        _readyMark.SetActive(data.active);

    }

}
