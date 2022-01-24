using UnityEngine;

public class Enemy : MonoBehaviour {
    // Enemy attributes
    protected int hp = 1; // Enemy's health points
    protected float speed = 2; // Enemy's speed
    protected float speedOG = 2;

    private bool isDead;

    protected float droprate = 1;//0.10f;
    protected bool isBoss = false;

    public GameObject ammoPre;

    protected virtual void Start() {
        
    }

    private void Update() {
        // Enemy should only chase the player if the player isn't moving forward
        if (!GameManager.Instance.GetIsOnFirstStage()) {
            transform.LookAt(GameManager.Instance.Player.transform);
            transform.rotation *= Quaternion.Euler(0, -90, 0);
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    // Enemy is hit
    public void Damage(int damage) {
        // Don't do any damage if the game is "paused"
        if (((GameManager.Instance.NavMeshAgent.speed == 0.0f && GameManager.Instance.GetIsOnFirstStage()) || isDead) && !isBoss) return;
        
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_HIT");
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        transform.Find("Eye").gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        transform.Find("Left Eye").gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        transform.Find("Right Eye").gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        Invoke(nameof(TurnOffHit), 0.2f);
        
        // Decrease health points
        hp -= damage;
        // If health points are less than or equal to 0, enemy should die
        if (hp <= 0) {
            hp = 0;
            Invoke(nameof(Die), 0.15f);
        }
    }

    private void TurnOffHit() {
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_HIT");
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    // Destroy itself
    protected virtual void Die() {
        // Prevent double method call (e.g: enemy's body and eye both hit)
        isDead = true;
        
        GameManager.Instance.DecrementNumEnemies();

        Drops();
        
        // If the player is already on the second stage and there are no more enemies alive, then that level is completed
        if (!GameManager.Instance.GetIsOnFirstStage() && GameManager.Instance.GetNumEnemies() == 0) {
            GameManager.Instance.Overlay.ToggleOnLevelCompleted();
        }
        Destroy(gameObject);
    }

    public void PauseUnpause(bool pause) {
        if (pause) {
            speed = 0;
        }
        else {
            speed = speedOG;
        }
    }

    public void Drops() {
        if (ammoPre && Random.Range(0,1)<=droprate) {
            var ammo = Instantiate(ammoPre);
            float dist = Vector3.Distance(transform.position,GameManager.Instance.Player.transform.position);
            ammo.transform.position = transform.position;
            ammo.transform.forward = transform.forward;
            float sp = dist * (16-6/50);
            if (sp<0) sp = 0;
            else if (sp>16) sp = 16;
            ammo.GetComponent<Ammo>().speed = sp;
        }
    }
    
    public int GetHP() {
        return hp;
    }
}
