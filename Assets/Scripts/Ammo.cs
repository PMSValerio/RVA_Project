using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    private int weapon;
    private int level;

    private int state = 0;

    public float speed;
    private float maxSpeed = 16;

    private float accel = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        weapon = Random.Range(0,4);
        level = Random.Range(1,4);
        if (level == 1) {
            transform.Find("Model").localScale = new Vector3(0.7f,0.7f,0.7f);
        }
        if (level == 2) {
            transform.Find("Model").localScale = new Vector3(0.7f,1.2f,0.7f);
        }
        else if (level == 3) {
            transform.Find("Model").localScale = new Vector3(1.7f,1.2f,1.7f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case 0:
                if (speed<0) {
                    speed = 0;
                    state = 1;
                }
                else {
                    transform.position += transform.forward * speed * Time.deltaTime;
                    speed -= 0.6f;
                }
            break;
            case 1:
                if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) < 1) {
                    GameManager.Instance.Player.GetComponent<ControllerParent>().AddAmmo(weapon,level);
                    Destroy(gameObject);
                }
                else {
                    transform.LookAt(GameManager.Instance.Player.transform.position, Vector3.up);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    if (speed<maxSpeed) {
                        speed += accel;
                    }
                    else {
                        speed = maxSpeed;
                    }
                }
            break;
        }
    }
}
