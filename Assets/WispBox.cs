using UnityEngine;
public class WispBox : MonoBehaviour {
    public enum WispThings {
        Cube, Rocket, Hover, Boost, Slowmo, Rings
    }
    public WispThings wisp = WispThings.Hover;
    Wisp w;
    bool didTheThing = false;
    void Update() {
        if (!Wisps.main || didTheThing) return;
        if (wisp == WispThings.Hover) w = Wisps.main.hover;
        if (wisp == WispThings.Rocket) w = Wisps.main.rocket;
        if (wisp == WispThings.Cube) w = Wisps.main.cube;
        if (wisp == WispThings.Slowmo) w = Wisps.main.slowmo;
        didTheThing = true;
    }
    void OnDamage(Player p) {
        if (wisp == WispThings.Boost) {
            if (p.boost >= p.maxBoost) return;
            p.boost += 25;
            if (p.boost >= p.maxBoost) p.boost = p.maxBoost;
            return;
        }
        if (wisp == WispThings.Rings) {
            p.rings += 10;
        }
        if (w == null) return;
        p.ChangeWisp(w);  
    }
    /*
    void OnTriggerEnter(Collider col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            if (wisp == WispThings.Boost) {
                p.boost += 25;
                if (p.boost > p.maxBoost) p.boost = p.maxBoost;
                return;
            }
            p.ChangeWisp(w);
        }
    }
    */
}