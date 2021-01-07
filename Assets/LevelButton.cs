using UnityEngine;
public class LevelButton : MonoBehaviour {
    public Level level;
    void Start() {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }
    void OnClick() {
        level.Start();
    }
}