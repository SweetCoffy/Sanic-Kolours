using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour {
    public float sixtyFpsDeltaTime = 0.02f;
    public float updateRate = 1;
    Text t;
    void Start() {/**/
        t = GetComponent<Text>();
        UpdateCounter();
        InvokeRepeating("UpdateCounter", 1 / updateRate, 1 / updateRate);
    }
    void UpdateCounter() {
        t.text = $"{Mathf.Floor((sixtyFpsDeltaTime / Time.deltaTime) * 60)}";
    }
}