using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounterHUD : HUDElement
{
    public UnityEngine.UI.Text txt;
    protected override void Update()
    {
        txt.text = Player.score + "";
    }
}
