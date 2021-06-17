using UnityEngine;
[CreateAssetMenu(fileName = "Boost Skill", menuName = "Boost Skill")]
public class BoostSkill : Skill {
    public float boostAdd = 1;
    public override void Equip() {
        base.Equip();
        Game.boostUpgrade += boostAdd;
    }
    public override void Unequip() {
        base.Unequip();
        Game.boostUpgrade -= boostAdd;
    }
}