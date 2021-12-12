using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    int hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
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
