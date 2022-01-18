using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{

    public GameObject arrowPre;

    Transform bow;
    Transform arrow;

    float cooldown = 1.5f;
    float cooltimer = 0f;

    bool readied;

    float pullStr;
    float pullLimit = 0.8f;
    bool pulling;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();

        bow = transform.Find("Bow");
        arrow = transform.Find("Arrow");
        pullStr = 0;
    }

    // Update is called once per frame
    void Update() {
        if (cooltimer>0) {
            cooltimer-=Time.deltaTime;
            if (cooltimer<=0) arrow.gameObject.SetActive(true);
        }
        else {
            if (readied) {
                float dist = (bow.position - arrow.position).magnitude;
                if (!pulling) {
                    if (dist<0.3f) {
                        pulling = true;
                    }
                }

                bool misfire = DistToPull(dist);

                if (!action || misfire) {
                    readied = false;
                    if (pulling) Fire(misfire);
                }
            }
            else {
                if (action) {
                    readied = true;
                }
            }
        }
        ApplyPose();
    }

    bool DistToPull(float dist) {
        if (!pulling) return false;
        float aux = dist; // the value of dist after conversion to a 0-1 value

        pullStr = aux;
        if (pullStr>1) return true;

        return false;
    }

    void Fire(bool misfire) {
        if (misfire) {
        }
        else {
            GameObject bulletObj = Instantiate(arrowPre);
            bulletObj.transform.position = bow.position + bow.forward;
            bulletObj.GetComponent<Arrow>().move = bow.forward*0.8f*pullStr;
        }
        cooltimer = cooldown;
        arrow.gameObject.SetActive(false);
        pullStr = 0;
        pulling = false;
    }

    public override void ApplyPose() {
        bow.localPosition = p2;
        if (readied) {

            var dist = (p1 - p2).magnitude;
            Debug.Log(dist);
            if (pulling && dist >= 0.6) {
                arrow.localPosition += arrow.TransformVector(new Vector3(0,0,-0.005f));
                var newdist = (arrow.localPosition - p2).magnitude;
                if (newdist>dist) arrow.localPosition = p1;
            }
            else {
                arrow.localPosition = p1;
            }

            var rot = Quaternion.LookRotation(p2 - p1);
            arrow.rotation = rot;
            bow.rotation = rot;
        }
        else {
            arrow.localPosition = p1;
            arrow.rotation = r1;
            bow.rotation = r2;
        }
        if (pulling) {
            transform.Find("String").localPosition = arrow.localPosition;
        }
        else {
            transform.Find("String").localPosition = bow.localPosition;
        }
    }
}
