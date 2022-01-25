using UnityEngine;

public class MGun : Weapon {

    LineRenderer pointer;
    LineRenderer shots;

    [SerializeField] private AudioSource shootAudio;
    [SerializeField] private AudioSource shootAudioAux;
    float soundCooldown = 0.1f;
    float soundTimer;
    bool auxPlay;

    int power = 2;

    bool showShots;
    float rumble = 0.05f;

    public override void Start() {
        base.Start();

        auxPlay = true;

        ammoMax = 300;
        ResetAmmo();

        pointer = transform.Find("Pointer").gameObject.GetComponent<LineRenderer>();
        shots = transform.Find("Shots").gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        ApplyPose();
        
        pointer.SetPosition(0,transform.InverseTransformPoint(transform.position));

        var direction = transform.forward;
        RaycastHit hit;
        Vector3 pointerEnd = transform.InverseTransformPoint(transform.position + 300*transform.forward);

        bool rayHit = Physics.Raycast(transform.position, direction, out hit, 300);
        if (rayHit && (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Obstacle") || hit.collider.gameObject.CompareTag("Goal"))) {
            pointerEnd = transform.InverseTransformPoint(hit.point);
        }
        if (ammo > 0 && action) {
            if (soundTimer <= 0) {
                auxPlay = true;
                shootAudio.Play();
                soundTimer = soundCooldown;
            }
            else {
                soundTimer -= Time.deltaTime;
                if (auxPlay && soundTimer <= soundCooldown / 2) {
                    auxPlay = false;
                    shootAudioAux.Play();
                }
            }
            ammo--;
            Debug.DrawRay (transform.position, direction, Color.cyan, Time.deltaTime, false);
            if (rayHit) {
                switch (hit.collider.gameObject.tag) {
                    case "Enemy":
                        // Collision with Drone
                        if (hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
                            enemy.Damage(power);
                        } 
                        // Collision with Drone's Eye
                        else {
                            hit.collider.gameObject.GetComponentInParent<Enemy>().Damage(power);
                        }
                        break;
                    case "Goal":
                        Destroy(hit.collider.gameObject);
                        break;
                    default:
                        if (hit.collider.gameObject.name.Equals("Platform")) return;
                        break;
                }
            }
            showShots = !showShots;
            if (showShots) {
                transform.Find("Shots").gameObject.SetActive(true);
                transform.localPosition = transform.localPosition + new Vector3(0,rumble,0);
            }
            else {
                transform.Find("Shots").gameObject.SetActive(false);
            }
            shots.SetPosition(0,transform.InverseTransformPoint(transform.position)+new Vector3(Random.Range(-1f,1f),Random.Range(-0.3f,0.3f),0));
            shots.SetPosition(1,pointerEnd);
        }
        else {
            transform.Find("Shots").gameObject.SetActive(false);
        }

        pointer.SetPosition(1,pointerEnd);
    }

    public override void ApplyPose() {
        transform.localPosition = p1;
        transform.rotation = Quaternion.LookRotation(p2 - p1);
    }
}
