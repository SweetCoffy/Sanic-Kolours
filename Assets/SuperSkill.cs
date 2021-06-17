using UnityEngine;
[CreateAssetMenu(fileName = "Super Skill", menuName = "Super Skill")]
public class SuperSkill : Skill {
    public override void Equip() {
        base.Equip();
        Game.superEnabled = true;
    }
    public override void Unequip() {
        base.Unequip();
        Game.superEnabled = false;
    }
}