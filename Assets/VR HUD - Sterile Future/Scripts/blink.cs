using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blink : MonoBehaviour
{

    public bool _blinking;
    private Image _image;
    private Shadow _shadow;
	private Color _color;
	public float _speed;
	[SerializeField] private bool isImage = false;

    // Use this for initialization
    void Start()
    {
	    if (isImage) {
		    _image = GetComponent<Image>();
		    _color = _image.color;
	    } else {
		    _shadow = GetComponent<Shadow>();
		    _color = _shadow.effectColor;
	    }
	    
    }

    // Update is called once per frame
    void Update()
    {
		if(_blinking) {
			float _blinkerAlpha;
			if (isImage) {
				_blinkerAlpha = Mathf.PingPong(_speed * Time.time, 0.2f);
				_color = new Color(_color.r, _color.g, _color.b, _blinkerAlpha);
				_image.color = _color;
			} else {
				_blinkerAlpha = Mathf.PingPong(_speed * Time.time, 1f);
				_color = new Color(_color.r, _color.g, _color.b, _blinkerAlpha);
				_shadow.effectColor = _color;
			}
		} else {
			_color = new Color(_color.r, _color.g, _color.b, 0);
			if (isImage) {
				_image.color = _color;
			} else {
				_shadow.effectColor = _color;
			}
		}
    }
}
