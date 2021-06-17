using UnityEngine;
public class Game : MonoBehaviour{
    public static bool cubeEnabled = false;
    public static bool superEnabled = false;
    public static float mouseSensitivity = 8;
    public static GameObject hudStyle;
    public static float boostUpgrade = 0;
    public static int totalRings = 0;
    public static Skill[] skills;
    public static Skill[] usedSkills;
    public static Skill noneSkill;
    public static float skillPoints = 100;
    public static float maxSkillPoints = 100;
    public static void UnlockByID(string id) {
        if (id == "") return;
        for (var i = 0; i < skills.Length; i++) {
            if (skills[i].id == id) {
                skills[i].locked = false;
                break;
            }
        }
    }
    public static void Start() {
        Skill[] s = Resources.LoadAll<Skill>("Skills/");
        skills = new Skill[s.Length];
        for (var i = 0; i < s.Length; i++) {
            Skill h = s[i].Clone();
            if (s[i].useAsNone) noneSkill = h;
            skills[i] = h;
        }
        usedSkills = new Skill[10];
        for (var i = 0; i < usedSkills.Length; i++) {
            usedSkills[i] = noneSkill;
        }
    }
    public static bool EquipSkillAt(Skill s, int idx) {
        Skill old = usedSkills[idx];
        old.Unequip();
        if (skillPoints - s.skillPoints < 0) {
            usedSkills[idx] = old;
            old.Equip();
            return false;
        } else {
            usedSkills[idx] = s;
            s.Equip();
            return true;
        }
    }
    public static void UnequipSkillAt(int idx) {
        Skill old = usedSkills[idx];
        old.Unequip();
        usedSkills[idx] = noneSkill;
        noneSkill.Equip();
    }
    public static void Update() {
        if (Player.main == null) return;
        int l = usedSkills.Length;
        for (var i = 0; i < l; i++) {
            usedSkills[i].Update();
        }
    }
}