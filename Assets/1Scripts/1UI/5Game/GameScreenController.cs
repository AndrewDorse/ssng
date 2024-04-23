using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenController : ScreenController
{
    private readonly GameScreenView _view;

    public GameScreenController(GameScreenView view) : base(view)
    {
        _view = view;
        _view.OpenScreen();

    }




}
