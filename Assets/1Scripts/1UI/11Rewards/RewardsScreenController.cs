using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsScreenController : ScreenController
{
    private readonly RewardsScreenView _view;

    public RewardsScreenController(RewardsScreenView view) : base(view)
    {
        _view = view;
        _view.OpenScreen();


    }



}
