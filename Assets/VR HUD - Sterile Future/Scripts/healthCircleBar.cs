using UnityEngine;
using UnityEngine.UI;

public class healthCircleBar : MonoBehaviour {
    [SerializeField] private Image circleBar;

    [SerializeField] private float healthValue = 0;
    private const float maxHealthPointsBar = 951f;
    private const float minHealthPointsBar = 155f;
    private float smallestBar;

    private bool update = false;
    private float previousAmount;

    private void Start() {
        smallestBar = (maxHealthPointsBar - minHealthPointsBar) / GameManager.Instance.GetPlayerMaxHP();
        circleBar.fillAmount = GameManager.Instance.GetPlayerMaxHP() * smallestBar / 1000 + minHealthPointsBar / 1000;
    }

    // Update is called once per frame
    private void Update() {
        float amount = GameManager.Instance.GetPlayerHP() * smallestBar / 1000 + minHealthPointsBar / 1000;
        if (previousAmount != amount) {
            update = true;
        }

        previousAmount = amount;
        
        if (update) {
            if (circleBar.fillAmount > amount) {
                circleBar.fillAmount = Mathf.MoveTowards(circleBar.fillAmount, amount, 0.5f * Time.deltaTime);
            } else update = false;
        }
    }
}
