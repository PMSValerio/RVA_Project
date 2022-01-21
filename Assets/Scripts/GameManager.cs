using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    // Public variables to access through other scripts
    public GameObject Player { get; private set; } // Player GameObject Singleton
    public NavMeshAgent NavMeshAgent { get; private set; } // NavMeshAgent Singleton
    public UIOverlay Overlay { get; private set; } // UI Overlay Singleton

    [SerializeField] private AudioSource tensionAudio;
    [SerializeField] private Material tensionSkybox;

    private const int maxLevel = 3;
    
    private const float runningNavMeshAgentSpeed = 2.0f; // NavMeshAgent speed while moving

    private bool isOnFirstStage = false;
    private Vector3[] pathCheckpoints = null;
    private int numEnemies = 0;
    
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
    
    // Per Level Stats
    private int level = 1;
    private float towerSpawnProbability = 0.02f; // chance of spawning tower at each step
    private float droneSpawnProbability = 0.0f; // chance of spawning drone at each step
    private int numBridges = 0; // number of bridges aka path length

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        playerHP = playerHPCap;
        Player = GameObject.Find("Player");
        NavMeshAgent = GameObject.Find("Platform").GetComponent<NavMeshAgent>();
        Overlay = GameObject.Find("HUD").GetComponent<UIOverlay>();
        
        NavMeshAgent.updateRotation = false;
        SetIsGamePaused(true);
        
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void Update() {
        if (isDead) {
            dieTimer += Time.deltaTime;
            if (dieTimer >= dieDuration) {
                dieTimer = 0;
            }
        }
    }
         
    private void OnDisable() {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
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

    public void SetHasGameStarted(bool value) {
        hasGameStarted = value;
    }
    
    public void PauseAll(bool pause) {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (enemy.TryGetComponent<Enemy>(out Enemy en)) {
                en.PauseUnpause(pause);
            }
        }
        NavMeshAgent.speed = pause?0:runningNavMeshAgentSpeed;
    }

    public void TensionUp() {
        tensionAudio.Play();
        RenderSettings.skybox = tensionSkybox;
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

    public int GetLevel() {
        return level;
    }

    public int GetMaxLevel() {
        return maxLevel;
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
    
    //
    // Level Configuration
    //

    public void LoadLevel(int level) {
        this.level = level;
        
        isOnFirstStage = false;
        pathCheckpoints = null;
        numEnemies = 0;
        
        isGamePaused = true;
        wasAgentRunning = false;
        hasGameStarted = false;
        previousCurrentWeaponIndex = 0;
        dieDuration = 5.0f;
        dieTimer = 0;
        isDead = false;
        
        LevelSettings();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LevelSettings() {
        switch (level) {
            case 2:
                towerSpawnProbability = 0.02f;
                droneSpawnProbability = 0.01f;
                numBridges = 1;
                break;
            case 3:
                towerSpawnProbability = 0.03f;
                droneSpawnProbability = 0.02f;
                numBridges = 2;
                break;
            /*case 4:
                towerSpawnProbability = 0.02f;
                droneSpawnProbability = 0.0f;
                numBridges = 2;
                break;
            case 5:
                towerSpawnProbability = 0.02f;
                droneSpawnProbability = 0.0f;
                numBridges = 3;
                break;*/
            default: // -1, maxLevel and level 1 aka back to menu 
                towerSpawnProbability = 0.02f;
                droneSpawnProbability = 0.0f;
                numBridges = 0;
                break;
        }
    }
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        playerHP = playerHPCap;
        Player = GameObject.Find("Player");
        NavMeshAgent = GameObject.Find("Platform").GetComponent<NavMeshAgent>();
        Overlay = GameObject.Find("HUD").GetComponent<UIOverlay>();
        
        NavMeshAgent.updateRotation = false;
        
        if (level != -1 && level != maxLevel+1) {
            Overlay.ToggleOnLevelStarting(1.5f, 0.5f);
            SetIsGamePaused(false);
            Invoke(nameof(ResumeNavMeshAgent), 3f);
        }
        else {
            level = 1;
            SetIsGamePaused(true);
        }
    }

    public float GetTowerSpawnProbability() {
        return towerSpawnProbability;
    }

    public float GetDroneSpawnProbability() {
        return droneSpawnProbability;
    }

    public int GetNumBridges() {
        return numBridges;
    }
}
