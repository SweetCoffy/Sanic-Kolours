using UnityEngine;
using UnityEngine.UI;
public class LevelSelect : MonoBehaviour {
    public GameObject levelPrefab;
    public Level[] levels;
    void Start() {/**/
        foreach (Level l in levels) {
            GameObject h = Instantiate(levelPrefab, transform.position, Quaternion.identity, transform);
            LevelButton lb = h.GetComponent<LevelButton>();
            Text name = h.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
            Image icon = h.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            lb.level = l;
            /**/
            name.text = l.fullName;
            icon.sprite = l.icon;
        }
    }
}