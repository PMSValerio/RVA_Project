using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerParent : MonoBehaviour {
    public GameObject[] weaponsPrefab;
    [SerializeField] private GameObject[] weaponsHUD;
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
        if (Input.GetKeyDown(KeyCode.P) && !GameManager.Instance.GetIsGamePaused() && GameManager.Instance.GetHasGameStarted()) {
            GameManager.Instance.Overlay.TogglePause();
        }
    }

    protected void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        weaponsHUD[current].gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(179, 246, 255, 50);
        weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = "";
        
        current = i % weapons.Count;
        if (current<0) current = weapons.Count + i;
        
        weapons[current].gameObject.SetActive(true);
        weaponsHUD[current].gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color32(179, 246, 255, 255);
        weaponsHUD[current].gameObject.GetComponentInChildren<Text>().text = "5/50";
    }
}
