using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour {
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Text enemiesAliveText;
    [SerializeField] private Text levelCompletedText;

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
        enemiesAliveText.text = value + "\nenemies remaining";
    }

    public void ToggleOnLevelCompleted() {
        ToggleOffEnemiesAlive();
            
        levelCompletedText.gameObject.SetActive(true);
        StartCoroutine(FadeText(levelCompletedText));
    }

    public void TogglePause() {
        if (GameManager.Instance.GetIsGamePaused()) {
            ResumeGameButton();
        } else {
            PauseGame();
        }

        GameManager.Instance.SetIsGamePaused(!GameManager.Instance.GetIsGamePaused());
    }
    
    private void ResumeGameButton() {
        GameManager.Instance.Player.transform.position = playerPosition;
    }

    private void PauseGame() {
        playerPosition = GameManager.Instance.Player.transform.position;
        GameManager.Instance.Player.transform.position = new Vector3(-10282.7012f, 1.5f, -4200.65088f);
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
