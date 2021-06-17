using UnityEngine;
public class ForcesBoostGauge : BoostGauge {
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;
    public RectTransform h;
    protected override void Update() {
        base.Update();
        ParticleSystem.EmissionModule e1 = particle1.emission;
        ParticleSystem.EmissionModule e2 = particle2.emission;
        ParticleSystem.EmissionModule e3 = particle3.emission;
        e1.enabled = isBoosting;
        e2.enabled = isBoosting;
        e3.enabled = isBoosting;
        ParticleSystem.MainModule m1 = particle1.main;
        ParticleSystem.MainModule m2 = particle2.main;
        ParticleSystem.MainModule m3 = particle3.main;
        m1.startColor = hud.color + new Color(.75f, .75f, .75f, 0);
        m2.startColor = hud.color + new Color(.75f, .75f, .75f, 0);
        m3.startColor = hud.color + new Color(.75f, .75f, .75f, 0);
        Rect r = bar.rectTransform.rect;
        h.position = (Vector3)new Vector2(bar.rectTransform.position.x + r.width, bar.rectTransform.position.y - (r.height / 2));
    }
}