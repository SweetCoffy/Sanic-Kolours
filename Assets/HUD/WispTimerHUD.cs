using UnityEngine;
using UnityEngine.UI;
public class WispTimerHUD : HUDElement {
    public Image bar;
    public Image background;
    public Image container;
    float width;
    float containerWidth;
    float tim = 0;
    Vector3 originalScale;
    public RectTransform scaledObject;
    public ParticleSystem[] particles;
    public Image icon;
    public GameObject iconHolder;
    protected override void Start() {
        base.Start();
        originalScale = scaledObject.localScale;
    }
    protected override void Update() {
        base.Update();
        iconHolder.SetActive(hud.player.wispsEnabled);
        width = background.rectTransform.rect.width; 
        if (hud.player.UsingWisp && tim < 1) tim += Time.deltaTime * 5;
        if ((!hud.player.UsingWisp || !hud.player.wispsEnabled) && tim > 0) tim -= Time.deltaTime * 5;
        scaledObject.localScale = Vector3.Lerp(Vector3.zero, originalScale, tim);
        if (hud.player.currWisp != null && hud.player.wispsEnabled) {
            Wisp w = hud.player.currWisp;
            if ((w.beingUsed && w.timeLeft > 0) || !w.beingUsed) {
                RectTransform r = bar.rectTransform;
                float amt = w.timeLeft;
                float max = w.duration;
                if (w.beingUsed) {
                    foreach (ParticleSystem p in particles) {
                        var m = p.main;
                        var e = p.emission;
                        e.enabled = true;
                        m.startColor = icon.color;
                    }
                } else {
                    foreach (ParticleSystem p in particles) {
                        var e = p.emission;
                        e.enabled = false;
                    }
                }
                r.sizeDelta = new Vector2(Mathf.Clamp01(amt / max) * width, r.rect.height);
                bar.color = w.barColor;
                icon.color = Color.white;
                icon.sprite = w.icon;
            } else {
                icon.color = Color.clear;
                foreach (ParticleSystem p in particles) {
                    var e = p.emission;
                    e.enabled = false;
                }
            }
        } else {
            icon.color = Color.clear;
            foreach (ParticleSystem p in particles) {
                var e = p.emission;
                e.enabled = false;
            }
        }
    }
}