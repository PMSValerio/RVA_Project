using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerParent : MonoBehaviour {
    public GameObject[] weaponsPrefab;
    protected List<GameObject> weapons;
    protected int current;
    
    // Start is called before the first frame update
    protected void Start() {
        weapons = new List<GameObject>();
        foreach (GameObject weapon in weaponsPrefab) {
            GameObject w = Instantiate(weapon, transform, true);
            weapons.Add(w);
        }
        SwitchWeapon(0);
    }

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            GameManager.Instance.Overlay.TogglePause();
        }
    }

    protected void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        current = i % weapons.Count;
        if (current<0) current = weapons.Count + i;
        weapons[current].gameObject.SetActive(true);
    }
}
