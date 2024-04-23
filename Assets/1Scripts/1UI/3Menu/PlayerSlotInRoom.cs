using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotInRoom : MonoBehaviour
{
    [SerializeField] private Image _classIcon;
    [SerializeField] private Image _raceIcon;
    [SerializeField] private Image _originIcon;
    [SerializeField] private Image _startItemIcon;

    [SerializeField] private TMPro.TMP_Text _nickNameText;
    [SerializeField] private TMPro.TMP_Text _ratingText;



    public void Setup(PlayerData playerData)
    {
        _nickNameText.text = playerData.nickname;


        if (playerData.heroData.classId == -1)
        {
            _classIcon.sprite = InfoProvider.instance.GetEmptySlotSprite();
        }
        else
        {
            _classIcon.sprite = InfoProvider.instance.GetHeroClass(playerData.heroData.classId).icon;

        }

        if (playerData.heroData.raceId == -1)
        {
            _classIcon.sprite = InfoProvider.instance.GetEmptySlotSprite();
        }
        else
        {
            _raceIcon.sprite = InfoProvider.instance.GetSubrace(playerData.heroData.raceId).icon;
        }



        
        

    }
}
