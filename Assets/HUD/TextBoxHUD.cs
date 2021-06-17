using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class TextBoxHUD : HUDElement {
    public Text text;
    public static TextBoxHUD main;
    public CanvasGroup canvasGroup;
    public Vector2 startOffset;
    public float textSpeed = 15;
    public float animSpeed = 5;
    RectTransform rt;
    float t = 0;
    Vector2 startPos;
    Vector2 targetPos;
    public float textDuration = 8;
    Queue<string> queue;
    protected override void Start() {
        queue = new Queue<string>();
        rt = GetComponent<RectTransform>();
        targetPos = startPos = rt.anchoredPosition;
        startPos += startOffset;
        main = this;
    }
    public void Show(string txt) {
        bool show = false;
        queue.Enqueue(txt);
        if (queue.Count == 1) show = true;
        if (show) StartCoroutine(_Show(queue.Peek()));
    }
    IEnumerator _Show(string txt) {
        text.text = "";
        t = 0;
        while (t < 1) {
            t += animSpeed * Time.deltaTime;
            yield return null;
        }
        t = 1;
        int charnum = 0;
        float c = 1 / textSpeed;
        string s = "";
        WaitForSeconds wait = new WaitForSeconds(c);
        foreach (char ch in txt) {
            s += ch + "";
            text.text = s;
            yield return wait;
        }
        float timer = textDuration;
        while (timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(_Hide());
        queue.Dequeue();
        if (queue.Count > 0) {
            yield return StartCoroutine(_Show(queue.Peek()));
        }
    }
    IEnumerator _Hide() {
        while (t > 0) {
            t -= animSpeed * Time.deltaTime;
            yield return null;
        }
        t = 0;
    }
    public void Hide() {
        StartCoroutine(_Hide());
    }
    protected override void Update() {
        canvasGroup.alpha = t;
        rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
    }
}