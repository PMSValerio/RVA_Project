using System;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private int hp;
    protected GameObject Player;

    // Start is called before the first frame update
    protected virtual void Start() {
        Player = GameObject.Find("Player");
        hp = 1;
    }

    protected virtual void Update() {
        
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
