using UnityEngine;
[System.Serializable]
public class Wisp {
    public Sprite icon;
    public Player player;
    public float duration = 5;
    public float timeLeft;
    private Vector2 originalPos;
    public bool beingUsed = false;
    public Color barColor = Color.white;
    public Color outlineColor = Color.white;
    public virtual void Start() {
        player.wispIcon.sprite = icon;
        timeLeft = duration;
        player.wispBar.fillAmount = 1;
        player.wispBar.color = barColor;
        beingUsed = true;
        originalPos = player.h.anchoredPosition;
    }
    public virtual void Update() {
        player.outlineThing.color = outlineColor;
        timeLeft -= Time.deltaTime;
        float shakeIntensity = Wisps.main.shakeIntensity * (timeLeft / duration);
        player.wispBar.fillAmount = timeLeft / duration;
        player.h.anchoredPosition = originalPos + new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
    }
    public Wisp Clone() {
        Wisp clone = (Wisp)this.MemberwiseClone();
        return clone;
    }
    public virtual void End() {
        player.wispIcon.sprite = player.placeholderIcon;
        player.wispBar.fillAmount = 0;
        player.wispBar.color = player.defaultBarColor;
        //player.outlineThing.color = new Color(1, 1, 1, 0);
        player.h.anchoredPosition = originalPos;
    }
}