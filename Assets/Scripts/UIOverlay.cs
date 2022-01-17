using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour {
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Text enemiesAliveText;
    [SerializeField] private Text levelCompletedText;
    [SerializeField] private Image panel; 

    private Vector3 playerPosition;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ToggleOnEnemiesAlive() {
        inGameHUD.SetActive(true);
        StartCoroutine(ScaleUp(canvasTransform, new Vector3(0.3f, 0.3f, 0.3f), 0.02f));
    }
    
    public void ToggleOffEnemiesAlive() {
        inGameHUD.SetActive(false);
    }
    
    public void SetEnemiesAlive(int value) {
        if (value > 0) {
            enemiesAliveText.text = value + "\nenemies remaining";
        } else {
            enemiesAliveText.text = "boss remaining";
        }
    }

    public void ToggleOnLevelCompleted() {
        ToggleOffEnemiesAlive();
            
        levelCompletedText.gameObject.SetActive(true);
        StartCoroutine(FadeText(levelCompletedText));
    }

    public void ToggleFader() {
        StartCoroutine(FadeTeleport());
    }

    public void ToggleOnPause() {
        Invoke(nameof(PauseGame), 1.1f);

        GameManager.Instance.SetIsGamePaused(true);
    }
    
    public void ToggleOffPause() {
        Invoke(nameof(ResumeGameButton), 1.0f);

        GameManager.Instance.SetIsGamePaused(false);
    }
    
    private void ResumeGameButton() {
        GameManager.Instance.Player.transform.position = playerPosition;
    }

    private void PauseGame() {
        playerPosition = GameManager.Instance.Player.transform.position;
        GameManager.Instance.Player.transform.position = new Vector3(-10282.7012f, 1.5f, -4200.65088f);
    }
    
    private IEnumerator FadeTeleport() {
        const float timeDivisor = 0.5f;

        // FadeIn Panel
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a);
        while (panel.color.a < 1.0f) {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + (Time.deltaTime / timeDivisor));
            yield return null;
        }

        // Show up panel for showTime seconds
        float showTime = 1.5f;
        while (showTime > 0.0f) {
            showTime -= (Time.deltaTime / timeDivisor);
            yield return null;
        }
        
        // FadeOut Panel
        while (panel.color.a > 0.0f) {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - (Time.deltaTime / timeDivisor));
            yield return null;
        }
    }

    private IEnumerator FadeText(Text text) {
        const float timeDivisor = 2f;

        // FadeIn Text
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / timeDivisor));
            yield return null;
        }

        // Show up text for showTime seconds
        float showTime = 1.5f;
        while (showTime > 0.0f) {
            showTime -= (Time.deltaTime / timeDivisor);
            yield return null;
        }
        
        // FadeOut Text
        while (text.color.a > 0.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / timeDivisor));
            yield return null;
        }
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
