using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Boss : Enemy {
    private void Awake() {
        isBoss = true;
        droprate = 1.0f;
        hp = 25 * GameManager.Instance.GetLevel() * GameManager.Instance.GetLevel();
    }

    private void Update() {
        transform.LookAt(GameManager.Instance.Player.transform);
    }

    public bool GetIsDead() {
        return isDead;
    }
    
    protected override void Die() {
        if (isDead) return;
        isDead = true;
        GameManager.Instance.Overlay.ToggleBossTimer(false);
        StartCoroutine(WaitTillDestroy());
    }

    private IEnumerator WaitTillDestroy() {
        yield return new WaitForSeconds(0.25f);
        
        GameManager.Instance.DecrementNumEnemies();

        Drops();
        
        // If the player is already on the second stage and there are no more enemies alive, then that level is completed
        if (!GameManager.Instance.GetIsOnFirstStage() && GameManager.Instance.GetNumEnemies() == 0) {
            GameManager.Instance.Overlay.ToggleOnLevelCompleted();
        }

        GameManager.Instance.Overlay.ToggleBossTimer(false);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
