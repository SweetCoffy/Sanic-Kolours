using UnityEngine;
using UnityEngine.UI;
public class SkillSelect : MonoBehaviour {
    public SkillButton[] buttons;
    public GameObject buttonPrefab;
    public Image skillPointsBar;
    public Text skillNameText;
    public Image skillIcon;
    public Text skillDescText;
    public Skill[] list;
    public SkillSelect fullSkillList;
    public bool usedSkills = true;
    void Start() {
        if (usedSkills) list = Game.usedSkills;
        else list = Game.skills;
        buttons = new SkillButton[list.Length];
        for (int i = 0; i < buttons.Length; i++) {
            SkillButton b = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<SkillButton>();
            b.idx = i;
            b.container = this;
            buttons[i] = b;
        }
        if (buttonPrefab.scene != null) {
            Destroy(buttonPrefab);
        }
        if (fullSkillList) fullSkillList.gameObject.SetActive(false);
    }
    public void Unequip() {
        if (!selected) return;
        Game.UnequipSkillAt(selected.idx);
    }
    public void Swap() {
        if (!selected) return;
        doingSwap = true;
        fullSkillList.selected = null;
    }
    bool doingSwap = false;
    void Update() {
        if (selected) {
            Skill s = list[selected.idx];
            if (skillNameText) skillNameText.text = s.skillName;
            if (skillDescText) skillDescText.text = s.description;
            if (skillIcon) skillIcon.sprite = s.icon;
        }
        if (skillPointsBar) skillPointsBar.fillAmount = 1 - (Game.skillPoints / Game.maxSkillPoints);
        if (fullSkillList) {
            fullSkillList.gameObject.SetActive(doingSwap);
            if (doingSwap && fullSkillList.selected) {
                Game.EquipSkillAt(fullSkillList.list[fullSkillList.selected.idx], selected.idx);
                doingSwap = false;
            }
        }
    }
    public SkillButton selected = null;
    public void SelectSkill(SkillButton btn) {
        if (list[btn.idx].locked) return;
        for (var i = 0; i < buttons.Length; i++) {
            buttons[i].selected = false;
        }
        btn.selected = true;
        selected = btn;
    }
}