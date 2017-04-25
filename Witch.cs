/*
Stupid Witch behavior. Surely YOU can do better than a randomly moving witch!!!
2017/4/23
@author jdmazz
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour {
	public float moveSpeed;

	[HideInInspector]
	public bool dejavu = false;

	float h,v;

	void Start () {
		++FindObjectOfType<Player>().dejavuPts; // update players points with each created witch
	}

	// Must be fixedupdate for proper collision detection
	// Also, if witch moves fast, set her to continuous (particularly for web build)
	void FixedUpdate ()
	{
		// move the witch in a random direction without checking walls. Good AI, yes/no...
		// Definitely a no. :-(
		// 48h is so harsh...
		int rng = Random.Range (0, 10);
		if (rng > 7) {
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
