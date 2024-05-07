using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSlotUI : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private TMPro.TMP_Text _optionText;

    [SerializeField] private PlayerChoiceIconUI[] _playersChoices;

    [SerializeField] private Button _rewardsButton;
    [SerializeField] private Button _chooseOptionButton;



    public void Setup(StoryOption storyOption, Action<int> rewardsCallback, Action<int> choiceCallback, int number )
    {
        gameObject.SetActive(true);
       

        _rewardsButton.onClick.AddListener(() => rewardsCallback?.Invoke(number));
        _chooseOptionButton.onClick.AddListener(() => choiceCallback?.Invoke(number));


        _icon.sprite = storyOption.Icon;
        _optionText.text = storyOption.Text;




    }

    public void ClearChoices()
    {
        foreach(PlayerChoiceIconUI choiceIcon in _playersChoices)
        {
            choiceIcon.gameObject.SetActive(false);
        }
    }

    public void AddUserStoryChoice(int heroClassId, string nickName)
    {
        for (int i = 0; i < _playersChoices.Length; i++)
        {
            PlayerChoiceIconUI choiceIcon = _playersChoices[i];

            if(choiceIcon.gameObject.activeInHierarchy)
            {
                continue;
            }


            Sprite classIcon = InfoProvider.instance.GetHeroClass(heroClassId).icon;

            choiceIcon.Setup(nickName, classIcon);

            break; 
        }
    }
}
