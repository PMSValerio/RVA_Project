using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : Enemy
{

    private int tensionHP = 10;

    private static int power = 5;

    private bool shakeDir = true;
    private float shakeToggleTimer = 0f;

    Material[] m_Material;

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

    private float proximity = 10;

    Quaternion offsetRot;
    Vector3 offsetVec;

    // Start is called before the first frame update
    protected override void Start() {
        speed = 8.0f;
        offsetRot = Random.rotation;
        offsetVec = offsetRot * Vector3.forward;

        base.Start();
        //m_Material = GetComponent<Renderer>().material;m.forward;
        detPos = transform.position + transform.forward;
        state = State.OFF;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) {
            case State.OFF:
                if (Input.GetKeyDown(KeyCode.Space) || !GameManager.Instance.GetIsOnFirstStage()) {
                    state = State.HOLD;
                    holdLim = Random.Range(0.5f, 2f);
                    hp = tensionHP;
                    nextState = State.DETACH;
                }
                break;
            case State.DETACH:
                if (Vector3.Distance(transform.position, detPos) < 0.05f) {
                    float pz = GameManager.Instance.Player.transform.position.z;
                    if (Mathf.Abs(transform.position.z - pz) < proximity || transform.position.z > pz) state = State.ROTATE;
                    Debug.Log("Detach");
                }
                else {
                    float step =  detSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, detPos, step);
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
                Vector3 targetDirection = GameManager.Instance.Player.transform.position - transform.position;

                float singleStep = rotSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDirection);

                if (Vector3.Angle(transform.forward,targetDirection) < 0.05f) {
                    state = State.SEEK;
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
                float posOffset = shakeToggleTimer * 5.0f/boomTime;
                transform.position = GameManager.Instance.Player.transform.position + 2 * offsetVec + (shakeDir?posOffset:-posOffset)*transform.right;
                transform.LookAt(GameManager.Instance.Player.transform);

                shakeToggleTimer += Time.deltaTime;
                if (shakeToggleTimer >= 0.1f) {
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
}
