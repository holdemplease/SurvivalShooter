using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoolTimeButton : MonoBehaviour {
	public Image img;
	public UnityEngine.UI.Button btn;
	public float cooltime = 60.0f;
	public bool disableOnStart = true;
	float leftTime = 60.0f;

	// Use this for initialization
	void Start () {
		if( img == null)
			img = gameObject.GetComponent<Image>();
		if( btn == null)
			btn = gameObject.GetComponent<UnityEngine.UI.Button>();
		if( disableOnStart)
			ResetCooltime();
	}
	
	// Update is called once per frame
	void Update () {
		if (leftTime == 0 && Input.GetKeyDown ("q")) {
			ResetCooltime ();
			return;
		}

		if (leftTime > 0) {
			leftTime -= Time.deltaTime;
			if (leftTime < 0) {
				leftTime = 0;
				if (btn)
					btn.enabled = true;
			}
			float ratio = 1.0f - (leftTime / cooltime);
			if (img)
				img.fillAmount = ratio;
		}
	}

	public bool CheckCooltime() {
		if( leftTime > 0 )
			return false;
		else
			return true;
	}

	public void ResetCooltime() {
		leftTime = cooltime;
		if( btn)
			btn.enabled = false;
	}
}
