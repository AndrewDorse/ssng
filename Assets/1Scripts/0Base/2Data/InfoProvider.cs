using Silversong.Data.Providers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoProvider : MonoBehaviour
{
    public static InfoProvider instance;

    [SerializeField] private HeroClassProviderData _heroClassProviderData;
    [SerializeField] private RaceProviderData _raceProviderData;
    [SerializeField] private InventoryItemsProviderData _inventoryItemsProviderData;





    [SerializeField] private Sprite _emptyItemSlot;



    private void Awake()
    {
        instance = this;
    }


    // hero classes
    public HeroClass GetHeroClass(int id)
    {
        return _heroClassProviderData.GetHeroClass(id);
    }

    public HeroClass[] GetStartHeroClasses()
    {
        return _heroClassProviderData.GetStartHeroClasses();
    }

    // races

    public Subrace GetSubrace(int id)
    {
        return _raceProviderData.GetSubrace(id);
    }

    public Race[] GetRaces()
    {
        return _raceProviderData.GetRaces();
    }

    // ITEMS


    public InventoryItem GetItem(int id)
    {
        return _inventoryItemsProviderData.GetItem(id);
    }

    public List<InventoryItem> GetItemsByTier(int id)
    {
        return _inventoryItemsProviderData.GetItemsByTier(id);
    }

















    public Sprite GetEmptySlotSprite()
    {
        return _emptyItemSlot;
    }
}
