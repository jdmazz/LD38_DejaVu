using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float moveSpeed;

	[HideInInspector]
	public int dejavuPts;

	float h,v;

	void Awake () {
		dejavuPts = 0;
	}
	
	void Update () {
		h = Input.GetAxisRaw("Horizontal");
		v = Input.GetAxisRaw("Vertical");
		if (h == 1) transform.localPosition += new Vector3(moveSpeed*Time.deltaTime,0);
		if (h == -1) transform.localPosition += new Vector3(-moveSpeed*Time.deltaTime,0);
		if (v == 1) transform.localPosition += new Vector3(0,moveSpeed*Time.deltaTime);
		if (v == -1) transform.localPosition += new Vector3(0,-moveSpeed*Time.deltaTime);
	}
}
