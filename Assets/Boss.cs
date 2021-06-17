using UnityEngine;
using System.Collections.Generic;
public class Boss : Enemy {
    public static int bossCount = 0;
    public List<BossMovement> movementComponents;
    public T GetMovement<T>() where T : BossMovement {
        T m = null;
        foreach (BossMovement mov in movementComponents) {
            if (mov as T) m = (T)mov;
        }
        return m;
    }
    public List<T> GetAllMovements<T>() where T : BossMovement {
        List<T> m = new List<T>();
        foreach (BossMovement mov in movementComponents) {
            if (mov as T) m.Add((T)mov);
        }
        return m;
    }
    protected override void Start() {/**/
        base.Start();
        bossCount++;
        movementComponents = new List<BossMovement>();
        StageLoader.main.AddBoss(this);
    }
    void OnDie() {
        StageLoader.main.RemoveBoss(this);
        bossCount--;
        if (bossCount <= 0) StageLoader.main.EndStage();
    }
}