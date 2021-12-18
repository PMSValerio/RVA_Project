using System;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private int hp;
    private GameObject Player;

    // Start is called before the first frame update
    private void Start() {
        Player = GameObject.Find("Player");
        hp = 1;
    }

    private void Update() {
        transform.LookAt(Player.transform);
        transform.position += transform.forward * 2 * Time.deltaTime;
    }

    public void Damage(int damage) {
        hp -= damage;
        if (hp <= 0) {
            hp = 0;
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
