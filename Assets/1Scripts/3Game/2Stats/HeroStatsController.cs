using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatsController : MonoBehaviour
{
    public static HeroStatsController instance;

    

    public StatsController StatsController { get => _statsController; private set => _statsController = value; }

    private StatsController _statsController;
    private HeroDamageDealer _heroDamageDealer;

    private void Awake()
    {
        instance = this;

        EventsProvider.OnGameStart += Initialize;
    }

    private void Initialize()
    {
        HeroData heroData = DataController.instance.GetMyHeroData();

        _heroDamageDealer = new HeroDamageDealer();

        _statsController = new StatsController(
            InfoProvider.instance.GetHeroClass(heroData.classId),
            InfoProvider.instance.GetSubrace(heroData.raceId)
            );
    }










}
