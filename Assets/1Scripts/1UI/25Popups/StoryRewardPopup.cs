using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Silversong.UI
{

    public class StoryRewardPopup : Popup
    {
        [SerializeField] private Image _icon;

        [SerializeField] private TMPro.TMP_Text _optionText;


        [SerializeField] private RewardSlotUI[] _rewardsSlots;



        public void Setup(StoryOption option)
        {
            _icon.sprite = option.Icon;
            _optionText.text = option.Text;



            SetupRewardSlots(option.RewardSlot);
        }


        private void SetupRewardSlots(RewardSlot[] rewards)
        {
            for (int i = 0; i < _rewardsSlots.Length; i++)
            {
                if (i < rewards.Length)
                {
                    _rewardsSlots[i].Setup(rewards[i]);
                }
                else
                {
                    _rewardsSlots[i].gameObject.SetActive(false);
                }



            }
        }



    }


}