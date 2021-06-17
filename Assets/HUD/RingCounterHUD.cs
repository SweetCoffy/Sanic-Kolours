using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingCounterHUD : HUDElement
{
    public Text txt;
    protected override void Update()
    {
        
        txt.text = hud.player.rings + "";
    }
}
