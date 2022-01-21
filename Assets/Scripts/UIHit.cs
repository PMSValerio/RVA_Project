using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                    Invoke(nameof(InvokeResumeNavMeshAgent), 3f);
                    break;
                case "Levels":
                    // TODO
                    break;
                case "Resume":
                    GameManager.Instance.Overlay.ToggleOffPause();
                    break;
                case "Retry":
                    GameManager.Instance.LoadLevel(GameManager.Instance.GetLevel());
                    break;
                case "MainMenu":
                    GameManager.Instance.LoadLevel(-1);
                    break;
            }
            
            Destroy(other.gameObject);
        }
    }

    private void InvokeResumeNavMeshAgent() {
        GameManager.Instance.ResumeNavMeshAgent();
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
