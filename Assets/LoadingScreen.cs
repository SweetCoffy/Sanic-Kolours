using UnityEngine;
using UnityEngine.UI;
public class LoadingScreen : MonoBehaviour {
    public Animator anim;
    public Text zoneName;
    public Text zoneAct;
    public Text stageNumber;
    public Image loadingBar;
    public float progress = 0;
    public string zName;
    public string zAct;
    public int sNumber;
    void Start() {/*Debug.Log("Start");*/
        if (sNumber > -1) stageNumber.text = "STAGE " + sNumber.ToString();
        else stageNumber.text = "";
        zoneAct.text = zAct;
        zoneName.text = zName;
    }
    void Update() {
        if (!loadingBar) return;
        loadingBar.fillAmount = progress;
    }
}