using UnityEngine.UI;
using UnityEngine;
public class TotalRingCounter : MonoBehaviour {
    public Text txt;
    public string format = "000 000 000";
    int shownAmount = 0;
    void Update() {
        int dif = Game.totalRings - shownAmount;
        int cdif = Game.totalRings - shownAmount;
        cdif = Mathf.Clamp(dif, -10, 10);
        shownAmount += cdif;
        txt.text = shownAmount.ToString(format);
    }
}