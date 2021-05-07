using UnityEngine;
using UnityEngine.UI;
public class RingCounter : Text {
    public static RingCounter main;
    protected override void Start() {/*Debug.Log("Start");*/
        base.Start();
        main = this;
    }
}