using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{

    Material m_Material;

    private State state = State.OFF;
    private State nextState = State.DETACH;

    private enum State {
        OFF,
        DETACH,
        HOLD,
        ROTATE,
        STALK
    }

    float speed = 2f;
    float detSpeed = 1f;
    float rotSpeed = 2f;
    Vector3 detPos;

    float holdLim = 0.5f;
    float holdTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_Material = transform.Find("Eye").gameObject.GetComponent<Renderer>().material;
        detPos = transform.position + transform.forward;
        state = State.OFF;
        m_Material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) {
            case State.OFF:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    state = State.HOLD;
                    nextState = State.DETACH;
                    m_Material.EnableKeyword("_EMISSION");
                }
            break;
            case State.DETACH:
                float step =  detSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, detPos, step);

                if (Vector3.Distance(transform.position, detPos) < 0.05f) {
                    state = State.HOLD;
                    nextState = State.ROTATE;
                }
            break;
            case State.HOLD:
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdLim) {
                    state = nextState;
                }
            break;
            case State.ROTATE:
                Vector3 targetDirection = Player.transform.position - transform.position;

                float singleStep = rotSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDirection);

                if (Vector3.Angle(transform.forward,targetDirection) < 0.05f) {
                    state = State.STALK;
                }
            break;
            case State.STALK:
                transform.LookAt(Player.transform);
                transform.position += transform.forward * speed * Time.deltaTime;
            break;
        }
        
    }
}
