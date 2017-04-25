/*
Triggers saying Hi! when encountering another witch. Also, updates points.
2017/4/23
@author jdmazz
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DejavuTrigger : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Witch") {
			if (!other.GetComponent<Witch>().dejavu) {
	        	other.GetComponent<Animator>().SetTrigger("Hi");
				other.GetComponent<Witch>().dejavu = true; // Turns off collisions with this witch
        		transform.GetComponentInParent<Animator>().SetTrigger("Hi");
        		--transform.GetComponentInParent<Player>().dejavuPts;
        	}
        }
    }
}
