/*
Title screen's very limited functionality
2017/4/23
@author jdmazz
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	void Update () {
		if (Input.GetAxisRaw("Submit") == 1) {
			SceneManager.LoadScene("Level 1");
		}
	}
}
