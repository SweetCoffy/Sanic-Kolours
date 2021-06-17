using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour {
    public static float seconds = 0;
    public static float milliseconds = 0;
    public static float minutes = 0;
    public static Timer main;
    public Text timeText;
    void Start() {/**/
        if (!main) main = this;
    }
    void Update() {
        if (this == main) {
            milliseconds += Time.deltaTime * 1000;
            if (milliseconds > 1000) {
                milliseconds -= 1000;
                seconds++;
            }
            if (seconds > 60) {
                seconds -= 60;
                minutes++;
            }
        }
        if (timeText) timeText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}.{(milliseconds + "").Substring(0, 2)}";
    }
}