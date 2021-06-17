using UnityEngine;
[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject {
    public float skillPoints = 0;
    public Sprite icon;
    public string id = "none";
    public string skillName = "Nothing";
    public string description = "";
    public bool locked = false;
    public bool useAsNone = false;
    public virtual void Equip() {
        Game.skillPoints -= skillPoints;
    }
    public virtual void Update() {

    }
    public virtual Skill Clone() {
        return (Skill)this.MemberwiseClone();
    }
    public virtual void Unequip() {
        Game.skillPoints += skillPoints;
    }
}