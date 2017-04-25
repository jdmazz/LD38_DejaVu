using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour {
	public float moveSpeed;

	[HideInInspector]
	public bool dejavu = false;

	float h,v;

	void Start () {
		++FindObjectOfType<Player>().dejavuPts;
	}
	
	void Update ()
	{
		int rng = Random.Range (0, 10);
		if (rng > 8) {
			Dir[] dirs = { Dir.LEFT, Dir.UP, Dir.RIGHT, Dir.DOWN };
			Dir rngDir = dirs [Random.Range (0, dirs.Length)];
			switch (rngDir) {
			case Dir.LEFT:
				transform.localPosition += new Vector3 (-moveSpeed * Time.deltaTime, 0);
				break;
			case Dir.UP:
				transform.localPosition += new Vector3 (0, moveSpeed * Time.deltaTime);
				break;
			case Dir.RIGHT:
				transform.localPosition += new Vector3 (moveSpeed * Time.deltaTime, 0);
				break;
			case Dir.DOWN:
				transform.localPosition += new Vector3 (0, -moveSpeed * Time.deltaTime);
				break;
			}
		}
	}
}
