using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour {
    [SerializeField] private Text enemiesAliveText;
    [SerializeField] private Button levelCompletedButton;
    [SerializeField] private GameObject pausePanel; 
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ToggleOnEnemiesAlive() {
        enemiesAliveText.gameObject.SetActive(true);
    }
    
    public void ToggleOffEnemiesAlive() {
        enemiesAliveText.gameObject.SetActive(false);
    }
    
    public void SetEnemiesAlive(int value) {
        enemiesAliveText.text = value + " enemies";
    }

    public void ToggleOnLevelCompleted() {
        ToggleOffEnemiesAlive();
            
        levelCompletedButton.gameObject.SetActive(true);
        StartCoroutine(FadeText(levelCompletedButton.gameObject.GetComponentInChildren<Text>()));
    }

    public void TogglePause() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            PauseGame();
        }
        else {
            ResumeGameButton();
        }
    }
    
    public void ResumeGameButton() {
        Cursor.lockState = CursorLockMode.Locked;
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void PauseGame() {
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
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
}
