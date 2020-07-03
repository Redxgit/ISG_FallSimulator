using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugDrawRays : MonoBehaviour {
	[SerializeField] private Transform origin;
	[SerializeField] private float str;
	[SerializeField] private float pct;
	[SerializeField] private Vector3 dir;
	[SerializeField] private float gordoLinea = 1;

	[SerializeField] private Vector3 randomizedPoint;

	private void OnDrawGizmos() {
		var position = origin.position;
		//Gizmos.color = ;
		Gizmos.color = new Color(1,0,1,0.5f);
		
		Gizmos.DrawSphere(position, pct*.5f);
		Gizmos.color = Color.cyan;

		Gizmos.DrawLine(position, position + randomizedPoint * (pct * str));
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(position, position + dir * str);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(position, position + (dir + randomizedPoint * pct).normalized * str);
	}

	[ContextMenu("CalculateRandomized")]
	private void CalculateRandomized() {
		randomizedPoint = Random.insideUnitSphere;
	}

	// Start is called before the first frame update
	void Start() {
		CalculateRandomized();
	}

	// Update is called once per frame
	void Update() {
	}
}