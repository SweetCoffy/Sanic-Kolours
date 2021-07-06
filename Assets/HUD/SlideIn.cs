using UnityEngine.UI;
using UnityEngine;
public class SlideIn : MonoBehaviour {
    float delay = 1;
    public CanvasGroup targetCanvasGroup;
    public RectTransform target;
    Vector2 targetPos;
    public float startAlpha = 0;
    public float endAlpha = 1;
    public Vector2 startAtOffset;
    Vector3 startAt;
    public float speed = 1.5f;
    float t = 0;
    bool done = false;
    void Start() {
        targetPos = target.anchoredPosition;
        startAt = target.anchoredPosition + startAtOffset;
        target.anchoredPosition = startAt;
        targetCanvasGroup.alpha = startAlpha;
    }
    void Update() {
        if (done) return;
        if (delay > 0) {
            delay -= Time.deltaTime;
            return;
        }
        targetCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
        target.anchoredPosition = Vector3.Lerp(startAt, targetPos, t);
        if (t >= 1) done = true;
        if (t < 1) t += Time.deltaTime * speed;
    }
}