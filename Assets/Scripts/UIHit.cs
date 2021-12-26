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
                    GameManager.Instance.ResumeNavMeshAgent();
                    break;
            }
            
            Destroy(other.gameObject);
            gameObject.SetActive(false);
        }
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
