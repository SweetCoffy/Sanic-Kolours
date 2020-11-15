using UnityEngine;
[System.Serializable]
public class SlowmoWisp : Wisp {
    public float newTimeScale = 0.5f;
    public override void Start() {
        base.Start();
        Time.timeScale = newTimeScale;
    }
    public override void End() {
        base.End();
        Time.timeScale = 1;
    }
}
