using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForcesSyncope : MonoBehaviour {
	[SerializeField] private Rigidbody[] bodiesToApplyForce;
	[SerializeField] private Vector3[] forcesDirs;
	[SerializeField] private float[] forces;
	[SerializeField] private float timeToApply = 0.4f;
	[SerializeField] private ForceMode forceToUse;
	[SerializeField] private AnimationCurve[] forceCurves;

	[SerializeField] private Collider[] collidersToDeactivate;

	[SerializeField] private Vector2 minMaxFactorForce = new Vector2(0.9f, 1.1f);
	[SerializeField] private Vector2 minMaxFactorDir = new Vector2(0.8f, 1.2f);

	public bool Debugging;
	public bool useFixed = true;
	public bool deactivateColliders;

	private float[] currForces;
	private Vector3[] currDirs;

	private bool isApplying;
	private float eTime;

	void Start() {
		GameManager.Instance.OnActivateRagdoll += StartApplying;
		currForces = new float[forces.Length];
		currDirs = new Vector3[forcesDirs.Length];
	}

	void Update() {
		if (!isApplying) return;
		if (useFixed) return;
		eTime += Time.deltaTime;
		ApplyForce();
		if (eTime > timeToApply) {
			isApplying = false;
			for (int i = 0; i < collidersToDeactivate.Length; i++) {
				collidersToDeactivate[i].enabled = true;
			}
		}
	}

	private void FixedUpdate() {
		if (!isApplying) return;
		if (!useFixed) return;
		eTime += Time.fixedDeltaTime;
		ApplyForce();
		if (eTime > timeToApply) {
			isApplying = false;
			if (deactivateColliders) {
				for (int i = 0; i < collidersToDeactivate.Length; i++) {
					collidersToDeactivate[i].enabled = true;
				}
			}
		}
	}


	private void StartApplying(bool applyForces) {
		if (Debugging) Debug.Break();
		//Debug.Log("apply forces" + applyForces);
		isApplying = !applyForces;
		eTime = 0f;
		if (deactivateColliders) {
			for (int i = 0; i < collidersToDeactivate.Length; i++) {
				collidersToDeactivate[i].enabled = false;
			}
		}

		for (int i = 0; i < forces.Length; i++) {
			currForces[i] = forces[i] * Random.Range(minMaxFactorForce.x, minMaxFactorForce.y);
			currDirs[i] =
				(forcesDirs[i] + Random.insideUnitSphere * Random.Range(minMaxFactorForce.x, minMaxFactorDir.y))
				.normalized;
		}
	}

	private void ApplyForce() {
		float pct = eTime / timeToApply;
		for (int i = 0; i < bodiesToApplyForce.Length; i++) {
			float mult = 1;
			if (forceCurves[i] != null)
				mult = forceCurves[i].Evaluate(pct);
			bodiesToApplyForce[i]
				.AddForce(bodiesToApplyForce[i].transform.TransformDirection(currDirs[i]) * (currForces[i] * mult),
					forceToUse);
			//bodiesToApplyForce[i].AddForceAtPosition(posToApplyForce[i].TransformDirection(forcesDirs[i]) * forces[i] * mult, posToApplyForce[i].position, forceToUse);
			//bodiesToApplyForce[i].AddRelativeTorque(tforces[i] * tdirs[i] * mult, forceToUse);
		}
	}
}