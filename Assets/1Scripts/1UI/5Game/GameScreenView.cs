using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenView : ScreenView
{
    public Joystick joystick;







    public override ScreenController Construct()
    {
        return new GameScreenController(this);
    }

}
