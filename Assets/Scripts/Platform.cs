using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 dst;
    public Vector3 src;

    float speed = 2;

    bool forward = true;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        src = transform.position;

        dst = src + new Vector3(0,0,10);
    }

    // Update is called once per frame
    void Update()
    {
        int dir = 1;
        if (!forward) dir = -1;

        transform.position += new Vector3(0,0,dir * speed) * Time.deltaTime;

        if (forward) {
            if (transform.position.z > dst.z) {
                transform.position = dst;
                forward = false;
            }
        }
        else { 
            if (transform.position.z < src.z) {
                transform.position = src;
                forward = true;
            }
        }

        if (player) player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }
}
