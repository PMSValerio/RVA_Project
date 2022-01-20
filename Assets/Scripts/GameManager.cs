using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    // Public variables to access through other scripts
    public GameObject Player { get; private set; } // Player GameObject Singleton
    public NavMeshAgent NavMeshAgent { get; private set; } // NavMeshAgent Singleton
    public UIOverlay Overlay { get; private set; } // UI Overlay Singleton

    private bool isOnFirstStage;
    private Vector3[] pathCheckpoints;

    private int numEnemies;
    
    private const float runningNavMeshAgentSpeed = 2.0f; // NavMeshAgent speed while moving

    private bool isGamePaused = true;
    private bool wasAgentRunning = false;
    private bool hasGameStarted = false;
    private int previousCurrentWeaponIndex = 0;
    public float dieDuration = 5.0f;
    public float dieTimer = 0;
    private bool isDead = false;

    // --- Player Stats (included in GameManager for ease of access) ---

    [SerializeField] private int playerHP;
    private const int playerHPCap = 100;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        playerHP = playerHPCap;
        Player = GameObject.Find("Player");
        NavMeshAgent = GameObject.Find("Platform").GetComponent<NavMeshAgent>();
        Overlay = GameObject.Find("HUD").GetComponent<UIOverlay>();
        
        NavMeshAgent.updateRotation = false;
        SetIsGamePaused(true);
    }

    private void Update() {
        if (isDead) {
            dieTimer += Time.deltaTime;
            if (dieTimer >= dieDuration) {
                dieTimer = 0;
            }
        }
    }

    public void SetIsOnFirstStage(bool value) {
        isOnFirstStage = value;
    }

    public bool GetIsOnFirstStage() {
        return isOnFirstStage;
    }

    public void SetIsGamePaused(bool value, float fadeTime = 0.5f, float solidTime = 0.5f) {
        StartCoroutine(TeleportFade(value, fadeTime, solidTime));
    }

    private IEnumerator TeleportFade(bool toPause, float fadeTime = 0.5f, float solidTime = 0.5f) {
        if (toPause) {
            previousCurrentWeaponIndex = Player.GetComponent<ControllerParent>().GetCurrent();
            //SetWeaponToPistol();
            Invoke(nameof(SetWeaponToPistol), 0.15f);
            Overlay.ToggleFader(fadeTime, solidTime);
            yield return new WaitForSeconds(0.16f);
            Overlay.ToggleOffEnemiesAlive();
            if (NavMeshAgent.speed > 0.0f) {
                wasAgentRunning = true;
            } else {
                wasAgentRunning = false;
            }
            StopNavMeshAgent();
            isGamePaused = true;
            yield break;
        }
        Overlay.ToggleFader(fadeTime, solidTime);
        yield return new WaitForSeconds(fadeTime);
        //yield return new WaitForSeconds(0.3f);
        if (wasAgentRunning) {
            ResumeNavMeshAgent();
        }
        isGamePaused = false;
        Player.GetComponent<ControllerParent>().SwitchWeapon(previousCurrentWeaponIndex);
    }

    private void SetWeaponToPistol() {
        Player.GetComponent<ControllerParent>().SwitchWeapon(0);
    }

    public bool GetIsGamePaused() {
        return isGamePaused;
    }
    
    public void SetPathCheckpoints(Vector3[] value) {
        pathCheckpoints = value;
    }

    public Vector3[] GetPathCheckpoints() {
        return pathCheckpoints;
    }

    public void IncrementNumEnemies() {
        numEnemies++;
        Overlay.SetEnemiesAlive(numEnemies);
    }
    
    public void DecrementNumEnemies() {
        numEnemies--;
        Overlay.SetEnemiesAlive(numEnemies);
    }

    public int GetNumEnemies() {
        return numEnemies;
    }

    public bool GetHasGameStarted() {
        return hasGameStarted;
    }
    
    public void PauseAll(bool pause) {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (enemy.TryGetComponent<Enemy>(out Enemy en)) {
                en.PauseUnpause(pause);
            }
        }
        NavMeshAgent.speed = pause?0:runningNavMeshAgentSpeed;
    }

    //
    // Player
    //

    public void DamagePlayer(int damage) {
        gameObject.GetComponent<DamageEffect>().damagedAnim = true;
        playerHP -= damage;
        if (playerHP < 0) {
            playerHP = 0;
            DiePlayer();
        }
    }

    public int GetPlayerHP() {
        return playerHP;
    }

    public int GetPlayerMaxHP() {
        return playerHPCap;
    }

    public int GetPlayerWeaponAmmo() {
        return Player.GetComponent<ControllerParent>().GetAmmo();
    }

    public int GetPlayerWeaponMaxAmmo() {
        return Player.GetComponent<ControllerParent>().GetMaxAmmo();
    }

    private void DiePlayer() {
        gameObject.GetComponent<DamageEffect>().dieAnim = true;
        Player.GetComponent<ControllerParent>().SetCanFire(false);
        PauseAll(true);
        isDead = true;
    }

    //
    // NavMeshAgent Speed
    //

    // Resume NavMeshAgent
    public void ResumeNavMeshAgent() {
        hasGameStarted = true;
        Overlay.ToggleOnEnemiesAlive();
        NavMeshAgent.speed = runningNavMeshAgentSpeed;
    }
    
    // Stop NavMeshAgent
    public void StopNavMeshAgent() {
        NavMeshAgent.speed = 0.0f;
    }
}
