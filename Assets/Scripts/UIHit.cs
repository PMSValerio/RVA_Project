using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHit : MonoBehaviour {
    private void Start() {
        if ((name.Equals("LeftHand") && GameManager.Instance.righthand) || (name.Equals("RightHand") && !GameManager.Instance.righthand)) {
            return;
        }
        StartCoroutine(FadeText(GetComponentInChildren<Text>()));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Bullet")) {
            switch (name) {
                case "Title":
                    GameManager.Instance.Overlay.ToggleFader(1f);
                    Invoke(nameof(TeleportToStart), 1f);
                    break;
                case "Start":
                    GameManager.Instance.Overlay.ToggleFader(1f);
                    Invoke(nameof(TeleportToLevels), 1f);
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                case "Resume":
                    GameManager.Instance.Overlay.ToggleOffPause();
                    break;
                case "Options":
                    GameManager.Instance.Overlay.ToggleFader(1f);
                    GameManager.Instance.previousPosition = GameManager.Instance.Player.transform.position;
                    Invoke(nameof(TeleportToOptions), 1f);
                    break;
                case "Back":
                    GameManager.Instance.Overlay.ToggleFader(1f);
                    Invoke(nameof(TeleportBack), 1f);
                    break;
                case "MainMenu":
                    GameManager.Instance.Overlay.ToggleFader(0.5f);
                    Invoke(nameof(InvokeMainMenu), 0.5f);
                    break;
                case "Easy":
                    GameManager.Instance.SetMaxLevel(3);
                    GameManager.Instance.LoadLevel(1);
                    GameManager.Instance.SetIsGamePaused(false);
                    Invoke(nameof(InvokeLevelStarting), 1f);
                    Invoke(nameof(InvokeResumeNavMeshAgent), 3f);
                    break;
                case "Medium":
                    GameManager.Instance.SetMaxLevel(7);
                    GameManager.Instance.LoadLevel(5);
                    GameManager.Instance.SetIsGamePaused(false);
                    Invoke(nameof(InvokeLevelStarting), 1f);
                    Invoke(nameof(InvokeResumeNavMeshAgent), 3f);
                    break;
                case "Hard":
                    GameManager.Instance.SetMaxLevel(11);
                    GameManager.Instance.LoadLevel(9);
                    GameManager.Instance.SetIsGamePaused(false);
                    Invoke(nameof(InvokeLevelStarting), 1f);
                    Invoke(nameof(InvokeResumeNavMeshAgent), 3f);
                    break;
                case "LeftHand":
                    SetRightHand(false);
                    break;
                case "RightHand":
                    SetRightHand(true);
                    break;
            }
            
            Destroy(other.gameObject);
        }
    }

    private void SetRightHand(bool value) {
        GameManager.Instance.righthand = value;
        Text text;
        if (value) {
            text = GameObject.Find("LeftHand").GetComponent<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
            text = GameObject.Find("RightHand").GetComponent<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        } else {
            text = GameObject.Find("RightHand").GetComponent<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
            text = GameObject.Find("LeftHand").GetComponent<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        }
    }

    private void TeleportToStart() {
        Vector3 actualPosition = GameManager.Instance.Player.transform.position;
        GameManager.Instance.Player.transform.position = new Vector3(10448.7f, actualPosition.y, actualPosition.z);
    }
    
    private void TeleportToLevels() {
        Vector3 actualPosition = GameManager.Instance.Player.transform.position;
        GameManager.Instance.Player.transform.position = new Vector3(10580.0f, actualPosition.y, actualPosition.z);
    }
    
    private void TeleportToOptions() {
        GameManager.Instance.Player.transform.position = new Vector3(-10458.7f, GameManager.Instance.previousPosition.y, -4200.651f);
    }
    
    private void TeleportBack() {
        GameManager.Instance.Player.transform.position = GameManager.Instance.previousPosition;
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
