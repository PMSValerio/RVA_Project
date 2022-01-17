using UnityEngine;
using UnityEngine.UI;

public class healthCircleBar : MonoBehaviour {
    [SerializeField] private Image circleBar;

    [SerializeField] private float healthValue = 0;

    // Update is called once per frame
    void Update() {
        float amount = (healthValue / 100) * 180 / 360;
        circleBar.fillAmount = amount;
    }
}
