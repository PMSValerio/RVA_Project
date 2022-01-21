using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHit : MonoBehaviour {
    
    
    private void Start() {
        StartCoroutine(FadeText(GetComponentInChildren<Text>()));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Bullet")) {
            switch (name) {
                case "Start":
                    GameManager.Instance.SetIsGamePaused(false);
                    Invoke(nameof(InvokeLevelStarting), 1f);
                    Invoke(nameof(InvokeResumeNavMeshAgent), 3f);
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                case "Resume":
                    GameManager.Instance.Overlay.ToggleOffPause();
                    break;
                case "Tutorial":
                    // TODO
                    break;
                case "MainMenu":
                    GameManager.Instance.Overlay.ToggleFader(0.5f);
                    Invoke(nameof(InvokeMainMenu), 0.5f);
                    break;
            }
            
            Destroy(other.gameObject);
        }
    }

    private void InvokeMainMenu() {
        GameManager.Instance.LoadLevel(-1);
    }
    
    private void InvokeResumeNavMeshAgent() {
        GameManager.Instance.ResumeNavMeshAgent();
    }
    
    private void InvokeLevelStarting() {
        GameManager.Instance.Overlay.ToggleOnLevelStarting(1.5f, 0.5f);
    }
    
    private IEnumerator FadeText(Text text) {
        const float timeDivisor = 3f;

        // FadeIn Text
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / timeDivisor));
            yield return null;
        }
    }
}
