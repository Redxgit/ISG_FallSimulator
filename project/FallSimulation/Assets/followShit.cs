using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followShit : MonoBehaviour {
	public Transform tToFollow;
	// Update is called once per frame
	void Update() {
		transform.position = tToFollow.position;
	}
}