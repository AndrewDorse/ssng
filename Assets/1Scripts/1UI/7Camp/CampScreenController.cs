using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampScreenController : ScreenController
{
    private readonly CampScreenView _view;

    public CampScreenController(CampScreenView view) : base(view)
    {
        _view = view;
        _view.OpenScreen();

        _view.readyButton.onClick.AddListener(ReadyToNextLevel);

       
        

        SubcribeTopButtons();




        EventsProvider.OnPlayersDataChanged += SetPartyMembersUI;
        DataController.instance.AskToUpdatePlayersData();
    }

    private void ReadyToNextLevel()
    {


    }

    private void SetPartyMembersUI(List<PlayerData> data)
    {
        for (int i = 0; i < _view.campHeroSlots.Length; i++)
        {
            if(i < data.Count)
            {
                _view.campHeroSlots[i].gameObject.SetActive(true);
                _view.campHeroSlots[i].Setup(data[i], OnPartyMemberClick);
            }
            else
            {
                _view.campHeroSlots[i].gameObject.SetActive(false);
            }

        }


    }

    private void OnPartyMemberClick() // open popup with player??? / hero!!! info
    {

    }




    private void SubcribeTopButtons()
    {
        _view.topButtons[1].onClick.AddListener(OpenInventory);
    }


    private void OpenInventory()
    {
        Master.instance.ChangeGameStage(Enums.GameStage.inventory);
    }



    public override void Dispose()
    {
        base.Dispose();
        EventsProvider.OnPlayersDataChanged -= SetPartyMembersUI;
    }

}
