using UnityEngine;
[System.Serializable]
public class Wisp {
    public Sprite icon;
    public Player player;
    public float duration = 5;
    public float timeLeft;
    private Vector2 originalPos;
    private Vector2 originalHolderPos;
    public bool beingUsed = false;
    public Color barColor = Color.white;
    public float consumeRate = 1;
    public Color outlineColor = Color.white;
    public virtual void Start() {/**/
        //player.wispIcon.sprite = icon;
        timeLeft = duration;
        player.barColor = barColor;
        beingUsed = true;
        //originalPos = player.h.anchoredPosition;
        //originalHolderPos = player.iconHolder.anchoredPosition;
    }
    public virtual void Update() {
        //player.outlineThing.color = outlineColor;
        timeLeft -= Time.deltaTime * consumeRate;
        //player.boostThing.sizeDelta = new Vector2(3, 25);
        float shakeIntensity = Wisps.main.shakeIntensity * consumeRate;
        //player.barPercent = timeLeft / duration;
        //player.h.anchoredPosition = originalPos + new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
        //player.iconHolder.anchoredPosition = originalHolderPos + new Vector2(Random.Range(-shakeIntensity / 2, shakeIntensity / 2), Random.Range(-shakeIntensity / 2, shakeIntensity / 2));
        if (player.inputEnabled) {
            if (Input.GetButtonDown("Wisp Power")) WispPowerButtonDown();
            if (Input.GetButton("Wisp Power")) WispPowerButton();
        }
    }
    public Wisp Clone() {
        Wisp clone = (Wisp)this.MemberwiseClone();
        return clone;
    }
    public virtual void WispPowerButtonDown() {}
    public virtual void WispPowerButton() {}
    public virtual void End() {
        //player.wispIcon.sprite = player.placeholderIcon;;
        //player.outlineThing.color = new Color(1, 1, 1, 0);
        //player.h.anchoredPosition = originalPos;
        //player.iconHolder.anchoredPosition = originalHolderPos;
    }
}