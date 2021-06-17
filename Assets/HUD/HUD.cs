using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public BoostGauge boostGauge;
    public RingCounterHUD ringCounter;
    public ScoreCounterHUD scoreCounter;
    public TimeCounter timeCounter;
    public Color color;
    public Player player;
    public RectTransform rt;
    public HUDOptionalElements optionalElements;
    public static HUD cur;
    [System.Serializable]
    public class HUDOptionalElements {
        public HUDElement[] elements;
        HUD hud;
        public void SetUp(HUD hud) {
            this.hud = hud;
            foreach (HUDElement el in elements) {
                if (el == null) continue;
                el.hud = hud;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        optionalElements.SetUp(this);
        boostGauge.hud = this;
        ringCounter.hud = this;
        scoreCounter.hud = this;
        timeCounter.hud = this;
        cur = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 e = rt.localPosition;
        e.Scale(Vector3.right + Vector3.up);
        rt.localPosition = e;
        if (!player) return;
        boostGauge.isBoosting = player.isBoosting;
    }
}
