using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerParent : MonoBehaviour {
    public GameObject[] weaponsPrefab;
    [SerializeField] private GameObject[] weaponsHUD;
    protected List<GameObject> weapons;
    protected int current;
    protected bool canAction;
    
    // Start is called before the first frame update
    protected void Start() {
        canAction = true;
        weapons = new List<GameObject>();
        foreach (GameObject weapon in weaponsPrefab) {
            var w = Instantiate(weapon, transform, true);
            weapons.Add(w);
            w.transform.SetParent(transform,false);
        }
        weapons[current].GetComponent<Weapon>().selected = true;
        weapons[current].gameObject.SetActive(true);
    }

    protected void Update() {
        //if (OVRInput.GetDown(OVRInput.Button.Start) && !GameManager.Instance.GetIsGamePaused() && GameManager.Instance.GetHasGameStarted()) {
        if (Input.GetKeyDown(KeyCode.P) && !GameManager.Instance.GetIsGamePaused() && GameManager.Instance.GetHasGameStarted()) {
            GameManager.Instance.Overlay.ToggleOnPause();
        }

        if (weapons[current].gameObject.GetComponent<Weapon>().ammoMax == -1) {
            weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = "\u221E";
            weaponsHUD[current].gameObject.GetComponentInChildren<Text>().fontSize = 30;
        } else {
            weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = GameManager.Instance.GetPlayerWeaponAmmo().ToString();
        }
    }

    public void SwitchWeapon(int i) {
        // Do NOT let players switch weapons on Menus
        if (GameManager.Instance.GetIsGamePaused()) {
            return;
        }
        weapons[current].gameObject.SetActive(false);
        weaponsHUD[current].gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(179, 246, 255, 50);
        weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = "";
        
        current = i % weapons.Count;
        if (current<0) current = weapons.Count + i;
        
        weapons[current].GetComponent<Weapon>().selected = true;;
        weapons[current].gameObject.SetActive(true);
        weaponsHUD[current].gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(179, 246, 255, 255);
    }

    public void SetCanFire(bool yes) {
        canAction = yes;
    }

    public int GetAmmo() {
        return weapons[current].GetComponent<Weapon>().ammo;
    }

    public int GetCurrent() {
        return current;
    }

    public int GetMaxAmmo() {
        return weapons[current].GetComponent<Weapon>().ammoMax;
    }
}
