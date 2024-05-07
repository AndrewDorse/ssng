using Silversong.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryChoiceScreenController : ScreenController
{
    private readonly StoryChoiceScreenView _view;


    private Story _currentStory;


    public StoryChoiceScreenController(StoryChoiceScreenView view) : base(view)
    {
        _view = view;
        _view.OpenScreen();

        _currentStory = StoryChoiceController.GetStory();

        SetStory(_currentStory);


        EventsProvider.OnPlayersDataChanged += UpdatePlayersChoices;

    }



    private void SetStory(Story story)
    {

        _view.storyImage.sprite = story.Icon;

        _view.mainText.text = story.Text;



        SetStoryOptions(story.Options);
        ClearChoices();
    }

    private void SetStoryOptions(StoryOption[] storyOptions )
    {
        for(int i = 0; i < _view.options.Length; i++)
        {
            if (i < storyOptions.Length)
            {
                _view.options[i].Setup(storyOptions[i], RewardsClick, ChooseOptionClick, i);
            }
            else
            {
                _view.options[i].gameObject.SetActive(false);
            }



        }


    }


    private void ChooseOptionClick(int choiceId)
    {
        EventsProvider.OnStoryChoiceRpcRecieved?.Invoke(new StoryChoiceRPCData(DataController.instance.LocalData.UserId, choiceId));
        GameRPCController.instance.SendLocalChoiceToOthers(choiceId);
    }

    private void RewardsClick(int number)
    {
        StoryRewardPopup popup = Master.instance.GetPopup(Enums.PopupType.StoryRewards) as StoryRewardPopup;

        popup.Setup(_currentStory.Options[number]);
    }


    

    private void UpdatePlayersChoices(List<PlayerData> data)
    {
        ClearChoices();

        foreach (PlayerData playerData in data)
        {
            if(playerData.StoryChoice == -1)
            {
                // put to some buffer?? todo
                continue;
            }

            _view.options[playerData.StoryChoice].AddUserStoryChoice(playerData.heroData.classId, playerData.nickname);
        }
    }

    private void ClearChoices()
    {
        foreach (ChoiceSlotUI slotUI in _view.options)
        {
            slotUI.ClearChoices();
        }
    }






    public override void Dispose()
    {
        base.Dispose();

        EventsProvider.OnPlayersDataChanged -= UpdatePlayersChoices;
    }


}
