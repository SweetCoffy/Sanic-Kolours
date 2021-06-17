using UnityEngine.UI;
using UnityEngine;
[ExecuteAlways]
public class GradientColor : MonoBehaviour {
    public Image target;
    public Gradient gradient;
    public float speed = 1;
    float t = 0;
    void Start() {
        t = Random.Range(0, 1);
    }
    void Update() {
        if (gradient == null) return;
        t += Time.deltaTime * speed;
        if (t > 1) t = 0;
        target.color = gradient.Evaluate(t);
    }
}