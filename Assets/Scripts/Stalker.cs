using UnityEngine;

public class Stalker : Enemy {

    private static int power = 10;

    private bool shakeDir = true;
    private float shakeToggleTimer = 0f;

    private static int setting;
    private bool startedSetting = false;

    Material m_Material;

    private State state = State.OFF;
    private State nextState = State.DETACH;

    private enum State {
        OFF,
        DETACH,
        HOLD,
        ROTATE,
        SEEK,
        STALK
    }

    float detSpeed = 1f;
    float rotSpeed = 2f;
    Vector3 detPos;

    private float holdLim = 0.5f;
    private float holdTimer;

    private float boomTime = 3.0f;

    Quaternion offsetRot;
    Vector3 offsetVec;


    // Start is called before the first frame update
    protected override void Start() {
        speed = 3.0f;
        offsetRot = Random.rotation;
        offsetVec = offsetRot * Vector3.forward;

        base.Start();
        m_Material = transform.Find("Eye").gameObject.GetComponent<Renderer>().material;
        detPos = transform.position + transform.forward;
        state = State.OFF;
        m_Material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    private void Update() {
        switch(state) {
            case State.OFF:
                if (Input.GetKeyDown(KeyCode.Space) || !GameManager.Instance.GetIsOnFirstStage()) {
                    state = State.HOLD;
                    holdLim = Random.Range(0.5f, 2f);
                    nextState = State.DETACH;
                    m_Material.EnableKeyword("_EMISSION");
                    setting++;
                    startedSetting = true;
                }
                break;
            case State.DETACH:
                float step =  detSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, detPos, step);

                if (Vector3.Distance(transform.position, detPos) < 0.05f) {
                    state = State.HOLD;
                    holdLim = 0.5f;
                    nextState = State.ROTATE;
                    setting--;
                    startedSetting = false;
                }
                break;
            case State.HOLD:
                if (speed == 0) break;
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdLim) {
                    state = nextState;
                    holdTimer = 0;
                }
                break;
            case State.ROTATE:
                if (setting == 0) {
                    Vector3 targetDirection = GameManager.Instance.Player.transform.position - transform.position;

                    float singleStep = rotSpeed * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                    transform.rotation = Quaternion.LookRotation(newDirection);

                    if (Vector3.Angle(transform.forward,targetDirection) < 0.05f) {
                        state = State.SEEK;
                    }
                }
                break;
            case State.SEEK:
                Vector3 dst = GameManager.Instance.Player.transform.position + 2 * offsetVec;
                transform.LookAt(dst, Vector3.up);
                transform.position += transform.forward * speed * Time.deltaTime;

                if (Vector3.Distance(transform.position, dst) < 0.5f) {
                    state = State.STALK;
                }
                transform.LookAt(GameManager.Instance.Player.transform);
            break;
            case State.STALK:
                float posOffset = shakeToggleTimer * 3.0f/boomTime;
                transform.position = GameManager.Instance.Player.transform.position + 2 * offsetVec + (shakeDir?posOffset:-posOffset)*transform.up;
                transform.LookAt(GameManager.Instance.Player.transform);

                shakeToggleTimer += Time.deltaTime;
                if (shakeToggleTimer >= 0.05f) {
                    shakeDir = !shakeDir;
                    shakeToggleTimer = 0;
                }

                if (speed == 0) break;
                holdTimer += Time.deltaTime;
                if (holdTimer >= boomTime) {
                    Blow();
                    holdTimer = 0;
                }
            break;
        }
    }

    private void Blow() {
        GameManager.Instance.DamagePlayer(power);
        // TODO: Explosion Animation
        Die();
    }

    protected override void Die() {
        if (startedSetting) setting--;
        startedSetting = false;
        base.Die();
    }
}
