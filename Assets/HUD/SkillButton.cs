using UnityEngine;
using UnityEngine.UI;
public class SkillButton : MonoBehaviour {
    public SkillSelect container;
    public Text skillName;
    public Image skillIcon;
    Button btn;
    public bool selected = false;
    public int idx = 0;
    void Start() {
        btn = GetComponent<Button>();
        if (!skillName) {
            skillName = transform.GetChild(0).GetComponent<Text>();
        }
        if (!skillIcon) {
            skillIcon = transform.GetChild(1).GetComponent<Image>();
        }
    }
    public void Select() {
        container.SelectSkill(this);
    }
    void Update() {
        if (container == null) return;
        Skill h = container.list[idx];
        if (h == null) {
            h = Game.noneSkill;
        }
        btn.interactable = !h.locked;
        skillName.text = h.skillName;
        skillIcon.sprite = h.icon;
    }
}