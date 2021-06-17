using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostGauge : HUDElement
{
    public Image bar;
    public Image extendedBar;
    public Image otherBar;
    public Image background;
    public Image extension;
    public Image container;
    public bool isBoosting = false;
    float width;
    float containerWidth;
    public float testMaxBoost;
    public float testBoost;
    public Color testColor;
    public Color lowColor = Color.red;
    public float lowThreshold = 20;
    public Color actualColor;
    public Gradient boostEffectGradient;
    public float gradientSpeed = 1.2f;
    public float extensionLengthAdd = 16;
    float t = 0;
    public Color emptyColorMultiplier = Color.gray;
    Color originalColor;
    public float debugDist;
    public float otherBarOffset = 3.8583999999999605f;
    Color displayColor;
    public float referenceMaxBoost = 250;
    // Start is called before the first frame update
    [ContextMenu("Start")]
    protected override void Start()
    {
        displayColor = testColor;
        originalColor = container.color;
        containerWidth = container.rectTransform.rect.width;
    }

    // Update is called once per frame
    [ContextMenu("Update")]
    protected override void Update()
    {
        if (hud && hud.player) {
            testBoost = hud.player.boost;
            testMaxBoost = hud.player.MaxBoost;
            testColor = hud.color;
            extension.enabled = testBoost > (hud.player.MaxBoost + extensionLengthAdd);
            if (hud.player.isSuper && hud.player.rb.constraints != RigidbodyConstraints.FreezeAll) testBoost = ((hud.player.rings + hud.player.ringCooldown) / hud.player.superStartRings) * testMaxBoost;
            if ((testBoost / testMaxBoost) > lowThreshold * 0.01f) {
                actualColor = testColor;
            } else {
                actualColor = lowColor;
            }
        }
        if (testBoost <= 0) {
            container.color = originalColor * emptyColorMultiplier;
        } else {
            container.color = originalColor;
        }
        RectTransform r = bar.rectTransform;
        r.sizeDelta = new Vector2(Mathf.Clamp01(testBoost / testMaxBoost) * width, r.rect.height);
        extendedBar.rectTransform.sizeDelta = new Vector2(((testBoost / testMaxBoost) - 1) * width, extendedBar.rectTransform.rect.height);
        RectTransform or = otherBar.rectTransform;
        float target = 0;
        if (extendedBar.rectTransform.sizeDelta.x > .1f) {
            float dist = (extendedBar.rectTransform.rect.xMax) - (bar.rectTransform.rect.xMin);
            debugDist = dist;
            target = dist + width - otherBarOffset;
        } else {
            target = (testBoost / testMaxBoost) * width;
        }
        float diff = target - or.sizeDelta.x;
        if (Mathf.Abs(diff) > .25f) {
            or.sizeDelta += new Vector2(Mathf.Clamp(diff, -.5f, .5f) * Time.deltaTime * 85, 0);
        }
        if (diff > 0) {
            or.sizeDelta = new Vector2(target, or.sizeDelta.y);
        }
        RectTransform cr = container.rectTransform;
        Vector2 targetSize = new Vector2(Mathf.Clamp((testMaxBoost / referenceMaxBoost) * containerWidth, 50, Screen.safeArea.width - 16), container.rectTransform.sizeDelta.y);
        float sdiff = targetSize.x - cr.sizeDelta.x;
        if (Mathf.Abs(sdiff) > .25f) {
            container.rectTransform.sizeDelta += new Vector2(Mathf.Clamp(sdiff, -.5f * Time.deltaTime * 500, .5f * Time.deltaTime * 500), 0);
        }
        extension.rectTransform.sizeDelta = new Vector2(Mathf.Clamp((((testBoost / testMaxBoost) - 1) * width) + extensionLengthAdd, 0, float.PositiveInfinity), extension.rectTransform.rect.height);
        width = background.rectTransform.rect.width; 
        bar.color = displayColor;
        if (isBoosting) {
            t += Time.deltaTime * gradientSpeed;
            if (t > 1) t = 0;
            Color c = boostEffectGradient.Evaluate(t);
            Color targetColor = Color.Lerp(actualColor, c + Color.black, c.a);
            displayColor = Color.Lerp(displayColor, targetColor, 10 * Time.deltaTime);
        } else {
            t = 0;
            displayColor = Color.Lerp(displayColor, actualColor, 10 * Time.deltaTime);
        }
    }
}
