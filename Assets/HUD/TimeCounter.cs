using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : HUDElement
{
    public UnityEngine.UI.Text txt;
    protected override void Update()
    {
        txt.text = $"{Mathf.Floor(Player.time / 60).ToString().PadLeft(2, '0')}:{(Mathf.Floor(Player.time % 60)).ToString().PadLeft(2, '0')}.{Mathf.Floor((Player.time % 1) * 100).ToString().PadLeft(2, '0')}";
    }
}
