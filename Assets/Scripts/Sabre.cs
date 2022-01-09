using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabre : Weapon {
    
    bool swing;
    TrailRenderer trail;
    Gradient grNeutral;
    Gradient grSwing;

    void Start() {
        trail = transform.Find("Model/Trail").gameObject.GetComponent<TrailRenderer>();
        grNeutral = new Gradient();
        grNeutral.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(0.4f, 1.0f) }
        );
        grSwing = new Gradient();
        grSwing.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(0.7f, 1.0f) }
        );
    }

    // Update is called once per frame
    void Update() {
        swing = action; // TODO: change

        if (swing) {
            trail.time = 0.15f;
            trail.colorGradient = grSwing;
        }
        else {
            trail.time = 0.1f;
            trail.colorGradient = grNeutral;
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (swing) {
            switch (col.gameObject.tag) {
                case "Enemy":
                    // Collision with Drone
                    if (col.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
                        enemy.Damage(5);
                    } 
                    // Collision with Drone's Eye
                    else {
                        col.gameObject.GetComponentInParent<Enemy>().Damage(5);
                    }
                    break;
                case "Goal":
                    Destroy(col.gameObject);
                    break;
                default:
                    if (col.gameObject.name.Equals("Platform")) return;
                    break;
            }
        }
    }
}
