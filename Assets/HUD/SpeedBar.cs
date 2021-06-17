using UnityEngine.UI;
using UnityEngine;

public class SpeedBar : HUDElement {
    public Image[] bars;
    public bool[] changeWidth;
    Color[] defaultColors;
    public float maxSpeed = 300;
    public Image background;
    public float testSpeed;
    public Gradient boostModeGradient;
    public float gradientSpeed = 5;
    Color defaultColor;
    float width = 0;
    float t = 0;
    float shake = 0f;
    public Image container;
    public float shakeIntensity;
    Vector2 containerPos;
    protected override void Start() {
        defaultColors = new Color[bars.Length];
        containerPos = container.rectTransform.anchoredPosition;
        int i = 0;
        foreach(Image bar in bars) {
            defaultColors[i] = bar.color;
            i++;
        }
    }
    [ContextMenu("Updat")]
    protected override void Update() {
        int i = 0;
        if (hud && hud.player) {
            testSpeed = hud.player.CurrSpeed;
            if (hud.player.enteredBoostMode) {
                shake = 0.15f;
            }
            if (hud.player.boostMode) {
                t += Time.deltaTime * gradientSpeed;
                i = 0;
                if (t > 1) t -= 1;
                Color c = boostModeGradient.Evaluate(t);
                foreach(Image bar in bars) {
                    bar.color = c;
                    i++;
                }
            } else {
                t = 0;
                i = 0;
                foreach(Image bar in bars) {
                    bar.color = defaultColors[i];
                    i++;
                }
            }
        }
        if (shake > 0) {
            container.rectTransform.anchoredPosition = new Vector2(containerPos.x + Random.Range(-shakeIntensity, shakeIntensity), containerPos.y);
            shake -= Time.deltaTime;
        } else {
            container.rectTransform.anchoredPosition = containerPos;
        }
        i = 0;
        width = background.rectTransform.rect.width;
        foreach(Image bar in bars) {        
            if (i > changeWidth.Length - 1 || !changeWidth[i]) continue;
            bar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp01(testSpeed / maxSpeed) * width, bar.rectTransform.sizeDelta.y);
            i++;
        }
    }
}