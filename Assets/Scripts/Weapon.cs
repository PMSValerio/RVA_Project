using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject[] weapons;
    private int current = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject weapon in weapons) {
            Instantiate(weapon);
        }
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        current = i;
        weapons[current].gameObject.SetActive(true);
    }
}
