using UnityEngine;
using UnityEngine.UI;
public class ControlThing : MonoBehaviour {
    public SpriteAndColor[] bg;
    public Image image;
    RectTransform rect;
    public Text bname;
    public string actionID = "";
    public Text atext;
    int useBtn = 0;
    public string[] buttonName;
    public string action;
    void Start() {
        if (bg.Length < 1) bg = new SpriteAndColor[1];
        if (buttonName.Length < 1) buttonName = new string[1];
    }
    void Update() {
        string t = buttonName[(int)Mathf.Clamp(useBtn, 0, buttonName.Length - 1)];
        SpriteAndColor s = bg[(int)Mathf.Clamp(useBtn, 0, bg.Length - 1)];
        image.color = s.color;
        if (s.sprite == null) image.color *= new Color(1, 1, 1, 0);
        bname.text = t;
        atext.text = action;
    }
    [System.Serializable]
    public struct SpriteAndColor {
        public Color color;
        public Sprite sprite;
    }
}