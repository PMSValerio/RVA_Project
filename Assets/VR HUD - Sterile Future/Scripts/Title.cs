using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {
    [SerializeField] private float speed;
    
    private Text text;

    private Color textColor;
    // Start is called before the first frame update
    private void Start() {
        text = GetComponent<Text>();
        textColor = text.color;
    }

    // Update is called once per frame
    void Update() {
        float alpha = Mathf.PingPong(speed * Time.time, 0.75f);
        textColor = new Color(textColor.r, textColor.g, textColor.b, alpha);
        text.color = textColor;
    }
}
