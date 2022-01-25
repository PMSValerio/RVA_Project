using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour {
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject inGameMessages;
    [SerializeField] private Text enemiesAliveText;
    [SerializeField] private Text levelCompletedText;
    [SerializeField] private Text levelStartingText;
    [SerializeField] private Text weaponAcquiredText;
    [SerializeField] private Text bossTimerText;
    [SerializeField] private Text[] ammoAcquiredText;
    [SerializeField] private Image panel; 

    private Vector3 playerPosition;

    private Boss boss;

    private float bossTimer = 10f;
    // Start is called before the first frame update
    void Start() {
        boss = GameObject.Find("Goal").GetComponentInChildren<Boss>();
    }

    // Update is called once per frame
    void Update() {
        if (bossTimerText.isActiveAndEnabled) {
            if (bossTimer > 0) {
                bossTimer -= Time.deltaTime;
            }
            else {
                bossTimer = 0;
                GameManager.Instance.DamagePlayer(GameManager.Instance.GetPlayerMaxHP());
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(bossTimer);
            bossTimerText.text = string.Format("Time Left\n{0:D2}:{1:D2}", timeSpan.Seconds, (timeSpan.Milliseconds > 0 ? timeSpan.Milliseconds.ToString().Substring(0, timeSpan.Milliseconds.ToString().Length-1) : "00"));
        }
        
    }
    
    public void ToggleOnEnemiesAlive() {
        Debug.Log("B");
        if (inGameHUD != null) {
            Debug.Log("C");
            inGameHUD.SetActive(true);
            StartCoroutine(ScaleUp(inGameHUD.transform, new Vector3(0.3f, 0.3f, 0.3f), 0.02f));
        }
        if (inGameMessages != null) {
            Debug.Log("D");
            inGameMessages.SetActive(true);
        }
    }

    public void ToggleBossTimer(bool value) {
        if (value && !boss.GetIsDead()) {
            TextToBossFight();
            bossTimerText.gameObject.SetActive(true);
            bossTimerText.text = "Time Left:\n" + bossTimer;
        }
        else {
            bossTimerText.gameObject.SetActive(false);
        }
    }
    
    public void ToggleOffEnemiesAlive() {
        inGameHUD.SetActive(false);
    }

    public void ToggleOffLevelMessages() {
        inGameMessages.SetActive(false);
    }

    public void SetAmmoAcquired(int weapon, int amount) {
        ammoAcquiredText[weapon].gameObject.SetActive(false);
        ammoAcquiredText[weapon].gameObject.SetActive(true);
        ammoAcquiredText[weapon].text = "+" + amount;
        StartCoroutine(FadeText(ammoAcquiredText[weapon], 0.5f, 0.5f));
    }

    public void SetWeaponAcquired(string weaponName) {
        weaponAcquiredText.gameObject.SetActive(false);
        weaponAcquiredText.gameObject.SetActive(true);
        weaponAcquiredText.text = "Weapon Acquired\n" + weaponName + "!";
        StartCoroutine(FadeText(weaponAcquiredText, 1.5f, 0.5f));
    }
    
    public void SetEnemiesAlive(int value) {
        if (GameManager.Instance.GetIsOnFirstStage()) {
            if (value > 1) {
                enemiesAliveText.gameObject.transform.parent.gameObject.GetComponentInChildren<blink>()._blinking = false;
                enemiesAliveText.text = value + "\nenemies remaining";
            } else {
                TextToBossFight();
            }
        }
        else {
            enemiesAliveText.gameObject.transform.parent.gameObject.GetComponentInChildren<blink>()._blinking = false;
            if (value > 1) {
                enemiesAliveText.text = value + "\nenemies remaining";
            } else {
                enemiesAliveText.text = value + "\nenemy remaining";
            }
        }
        
    }

    public bool AnyImportantMessageOn() {
        return levelCompletedText.isActiveAndEnabled || levelStartingText.isActiveAndEnabled;
    }

    public void TextToBossFight() {
        enemiesAliveText.gameObject.transform.parent.gameObject.GetComponentInChildren<blink>()._blinking = true;
        enemiesAliveText.text = "boss fight";
    }

    public void ToggleOnLevelStarting(float fadeTime, float screenTime) {
        levelStartingText.gameObject.SetActive(true);
        levelStartingText.text = "Level " + GameManager.Instance.GetLevel().ToString("D2");
        StartCoroutine(FadeText(levelStartingText, fadeTime, screenTime));
    }

    public void ToggleOnLevelCompleted() {
        ToggleOffEnemiesAlive();
        if (GameManager.Instance.GetLevel() == GameManager.Instance.GetMaxLevel()) {
            GameManager.Instance.SetWeaponToPistol();
            levelCompletedText.text = "Thanks for\n playing!";
        }
        GameManager.Instance.SetHasGameStarted(false);

        GameManager.Instance.TensionDown();
        levelCompletedText.gameObject.SetActive(true);
        StartCoroutine(FadeText(levelCompletedText));
        Invoke(nameof(AuxToggleFader), 4f);
        Invoke(nameof(InvokeNextLevel), 6f);
    }

    // Can't use Invoke with parameters...
    private void AuxToggleFader() {
        ToggleFader(1.5f);
    }

    private void InvokeNextLevel() {
        if (GameManager.Instance.GetLevel() == GameManager.Instance.GetMaxLevel()) {
            GameManager.Instance.LoadLevel(-1);
        } else {
            GameManager.Instance.LoadLevel(GameManager.Instance.GetLevel()+1);
        }
    }

    public void ToggleFader(float fadeTime = 0.5f, float solidTime = 0.5f) {
        StartCoroutine(FadeTeleport(fadeTime, solidTime));
    }

    public void ToggleOnPause() {
        Invoke(nameof(PauseGame), 0.2f);

        GameManager.Instance.SetIsGamePaused(true, 0.15f, 0.05f);
    }
    
    public void ToggleOffPause() {
        Invoke(nameof(ResumeGameButton), 0.2f);

        GameManager.Instance.SetIsGamePaused(false, 0.15f, 0.05f);
    }
    
    private void ResumeGameButton() {
        GameManager.Instance.Player.transform.position = playerPosition;
    }

    private void PauseGame() {
        playerPosition = GameManager.Instance.Player.transform.position;
        GameManager.Instance.Player.transform.position = new Vector3(-10282.7012f, 1.5f, -4205.65088f);
    }
    
    private IEnumerator FadeTeleport(float fadeTime = 0.5f, float solidTime = 0.5f) {
        // FadeIn Panel
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a);
        while (panel.color.a < 1.0f) {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }

        // Show up panel for showTime seconds
        float showTime = 1.5f;
        while (showTime > 0.0f) {
            showTime -= (Time.deltaTime / solidTime);
            yield return null;
        }
        
        // FadeOut Panel
        while (panel.color.a > 0.0f) {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }

    private IEnumerator FadeText(Text text, float fadeTime = 2.0f, float screenTime = 2.0f) {
        // FadeIn Text
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }

        // Show up text for showTime seconds
        float showTime = 1.5f;
        while (showTime > 0.0f) {
            showTime -= (Time.deltaTime / screenTime);
            yield return null;
        }
        
        // FadeOut Text
        while (text.color.a > 0.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
    
    private IEnumerator ScaleUp(Transform objectTransform, Vector3 scale, float timeScale){
        float progress = 0;
     
        while(progress <= 1){
            objectTransform.localScale = Vector3.Lerp(objectTransform.localScale, scale, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
        objectTransform.localScale = scale;
     
    } 
}
