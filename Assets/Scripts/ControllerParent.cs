using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerParent : MonoBehaviour {
    public GameObject[] weaponsPrefab;
    private GameObject[] weaponsHUD;
    public List<GameObject> weapons;
    protected int current;
    protected bool canAction;
    
    private static ControllerParent Instance { get; set; }
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
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

        InitializeHUD();
    }

    protected void Update() {
        if (Input.GetKeyDown(KeyCode.P) && !GameManager.Instance.GetIsGamePaused() && GameManager.Instance.GetHasGameStarted()) {
        //if (OVRInput.GetDown(OVRInput.Button.Start) && !GameManager.Instance.GetIsGamePaused() && GameManager.Instance.GetHasGameStarted()) {
            GameManager.Instance.Overlay.ToggleOnPause();
            GameManager.Instance.Overlay.ToggleOffLevelMessages();
        }

        if (weapons[current].gameObject.GetComponent<Weapon>().ammoMax == -1) {
            weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = "\u221E";
            weaponsHUD[current].gameObject.GetComponentInChildren<Text>().fontSize = 30;
        } else {
            weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = GetAmmo().ToString();
        }
    }

    public void InitializeHUD() {
        weaponsHUD = new[] { GameObject.Find("Pistol Object"), GameObject.Find("MGun Object"), GameObject.Find("Sabre Object"), GameObject.Find("Bow Object")};
    }

    public void SwitchWeapon(int i) {
        // Do NOT let players switch weapons on Menus
        if (GameManager.Instance.GetIsGamePaused() || !GameManager.Instance.GetHasGameStarted()) {
            return;
        }
        int newCurr = i % weapons.Count;
        if (newCurr<0) newCurr = weapons.Count + i;
        bool upwards = newCurr-current==1 || newCurr-current==-(weapons.Count-1);

        bool found = false;
        if (!weapons[newCurr].GetComponent<Weapon>().acquired) {
            for (int ix = 1; ix<4; ix++) {
                if (upwards) {
                    newCurr = (newCurr+ix)%weapons.Count;
                }
                else {
                    newCurr = ((current-ix)%weapons.Count+weapons.Count)%weapons.Count;
                }
                if (newCurr!=current) {
                    var wep = weapons[newCurr].GetComponent<Weapon>();
                    if (wep.acquired) {
                        found = true;
                        break;
                    }
                }
            }
        }
        else found = true;

        if (!found) return;

        weapons[current].gameObject.SetActive(false);
        weaponsHUD[current].gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(179, 246, 255, 50);
        weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = "";
        
        current = newCurr;
        
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

    public void AddAmmo(int ix, int level) {
        if (weapons[2].GetComponent<Weapon>().acquired) {
            ix = Random.Range(0,3); // shamelessly hardcoding against regaining sabre ammo
            if (ix==2) ix = 3;
        }

        int divisor;
        if (weapons[ix].GetComponent<Weapon>().acquired) {
            divisor = 50;
        }
        else {
            divisor = 10;
        }
        float ammo = (weapons[ix].GetComponent<Weapon>().ammoMax / divisor) * level;
        if (ammo == 0) ammo = level;
        if (weapons[ix].GetComponent<Weapon>().AddAmmo((int)(ammo))) {
            
            AcquiredWeapon(ix);
        }
        else {
            AcquiredAmmo(ix, (int) ammo);
        }
    }

    private void AcquiredWeapon(int index) {
        GameManager.Instance.Overlay.SetWeaponAcquired(weapons[index].GetComponent<Weapon>().weaponName);
    }

    private void AcquiredAmmo(int weapon, int ammo) {
        GameManager.Instance.Overlay.SetAmmoAcquired(weapon, ammo);
    }
}
