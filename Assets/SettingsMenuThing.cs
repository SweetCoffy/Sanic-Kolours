using UnityEngine;
using UnityEngine.UI;
public class SettingsMenuThing : MonoBehaviour {
    public Slider sensitivitySlider;
    void Start() {
        sensitivitySlider.value = Game.mouseSensitivity;
    }
    void Update() {
        Game.mouseSensitivity = sensitivitySlider.value;
    }
    public void Exit() {
        StageLoader.main.GoToMainMenu();
    }
}