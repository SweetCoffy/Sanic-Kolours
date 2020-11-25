using UnityEngine;
public class WispBox : MonoBehaviour {
    public enum WispThings {
        Cube, Rocket, Hover, Boost, Slowmo
    }
    public WispThings wisp = WispThings.Hover;
    Wisp w;
    void Start() {
        
        if (wisp == WispThings.Hover) w = Wisps.main.hover;
        if (wisp == WispThings.Rocket) w = Wisps.main.rocket;
        if (wisp == WispThings.Cube) w = Wisps.main.cube;
        if (wisp == WispThings.Slowmo) w = Wisps.main.slowmo;
    }
    void OnDamage(Player p) {
        if (wisp == WispThings.Boost) {
            p.boost += 25;
            if (p.boost > p.maxBoost) p.boost = p.maxBoost;
            return;
        }
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