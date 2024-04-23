using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreenController : ScreenController
{
    private readonly InventoryScreenView _view;

    public InventoryScreenController(InventoryScreenView view) : base(view)
    {
        _view = view;
        _view.OpenScreen();

        SubcribeTopButtons();

        OpenItemsByTier(1);
    }

    

   
    private void OpenItemsByTier(int tier) // 1 - 4
    {
        SetItems(InfoProvider.instance.GetItemsByTier(tier));
    }

    private void SetItems(List<InventoryItem> items)
    {
        for(int i = 0; i < _view.shopLootSlots.Length; i++)
        {
            if(i < items.Count)
            {
                _view.shopLootSlots[i].gameObject.SetActive(true);
                _view.shopLootSlots[i].Setup(items[i], ItemInShopClick);
            }
            else
            {
                _view.shopLootSlots[i].gameObject.SetActive(false);
            }
        }



    }

    private void ItemInShopClick(int id)
    {

    }






    private void SubcribeTopButtons()
    {
        _view.topButtons[0].onClick.AddListener(OpenCamp);
    }


    private void OpenCamp()
    {
        Master.instance.ChangeGameStage(Enums.GameStage.camp);
    }


}
