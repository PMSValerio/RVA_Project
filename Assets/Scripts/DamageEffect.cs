using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{

    private float damageRedAlpha = 0;
    private bool damageRedToggle = false;
    Texture2D damageRed;

    float red = 1;

    public bool damagedAnim = false;
    public bool dieAnim = false;

    // Start is called before the first frame update
    void Start()
    {
        damageRed = new Texture2D(1,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (dieAnim) {
            dieAnim = !DieEffect(Time.deltaTime);
        }
        else if (damagedAnim) {
            damagedAnim = !DmgEffect(Time.deltaTime);
        }
    }

    private void OnGUI() {
        if (damageRedAlpha>0) {
            GUI.color = new Color(red,0,0,damageRedAlpha);
            GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),damageRed,ScaleMode.StretchToFill);
        }
    }

    public bool DmgEffect(float delta) {
        red = 1;
        if (damageRedToggle) {
            damageRedAlpha -= delta;
        }
        else {
            damageRedAlpha += delta * 5;
        }

        if (!damageRedToggle && damageRedAlpha > 0.6) {
            damageRedToggle = true;
        }
        else if (damageRedToggle && damageRedAlpha < 0) {
            damageRedAlpha = 0;
            damageRedToggle = false;
            return true;
        }

        return false;
    }

    public bool DieEffect(float delta) {
        red = 0;
        if (damageRedAlpha < 0.7f) damageRedAlpha = GameManager.Instance.dieTimer * 0.7f/(GameManager.Instance.dieDuration*2/3);
        else damageRedAlpha = 0.7f;

        return false;
    }

}
