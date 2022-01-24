using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Image bar;

    [SerializeField] private Enemy sentinel;
    [SerializeField] private Enemy drone;
    
    private float maxHealthPoints;
    private bool isSentinel;

    private bool update = false;
    private float previousAmount;

    private void Start() {
        isSentinel = sentinel.isActiveAndEnabled;
        maxHealthPoints = (isSentinel ? sentinel.GetHP() : drone.GetHP());
        bar.fillAmount = 1;
    }

    // Update is called once per frame
    private void Update() {
        float amount = (isSentinel ? sentinel.GetHP() : drone.GetHP()) / maxHealthPoints;
        if (previousAmount != amount) {
            update = true;
        }

        previousAmount = amount;
        
        if (update) {
            if (bar.fillAmount > amount) {
                bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, amount, Time.deltaTime);
            } else update = false;
        }
    }
}