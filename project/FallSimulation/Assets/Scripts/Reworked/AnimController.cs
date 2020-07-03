using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {
	[SerializeField] private Animator anim;
	[SerializeField] private Rigidbody mainRBody;
	[SerializeField] private RagdollShit[] ragdollShits;
	[SerializeField] private float triggerMultiplier = 1.05f;
	[SerializeField] private string notifierLayerName;
	[SerializeField] private CollisionDetector colliderPrefab;
	[SerializeField] private int walkAnimsCount;
	[SerializeField] private float forceMultiplier = 0.3f;
	[SerializeField] private int forceAnim = -1;

	//public Action ActionGoingRagdoll = delegate { };
	//public Action ActionReturnToAnimation = delegate { };
	//public Action ActionStartWalking = delegate { };
	public LayerMask notifierLayer;

	private Quaternion initRot;
	private Vector3 initPos;
	private bool isOnRagdoll;

	private string prevStatus;
	private int transitionCount;
	private bool onTransition;


	private AnimationClip ann;

	public bool OnTransition {
		get { return onTransition; }
	}

	public string Status {
		get { return GameManager.Instance.Status; }
	}

	// Por algun motivo con el trigger enter por defecto se vuelve gili la ragdoll asiq hay que hacerlo a mano
	private List<CollisionDetector> generatedColliders = new List<CollisionDetector>();


	// Start is called before the first frame update
	void Start() {
		CreateCollisionDetectors();
		initPos = transform.position;
		initRot = transform.rotation;
		GameManager.Instance.OnActivateRagdoll += GoRagdoll;
		GameManager.Instance.OnResetThings += ResetThings;
		GameManager.Instance.OnStartMoving += StartWalking;
	}

	// Update is called once per frame
	void Update() {
		/*
		if (Input.GetKeyDown(KeyCode.M) && !Input.GetKey(KeyCode.LeftShift)) {
		    RecieveCollision();
		}

		if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift)) {
		    RecieveCollision();
		    Debug.Break();
		}

		if (Input.GetKeyDown(KeyCode.C)) {
		    ResetThings();
		}

		if (Input.GetKeyDown(KeyCode.W)) {
		    StartWalking();
		}

		if (Input.GetKeyDown(KeyCode.S)) {
		    GoIdle();
		}*/
	}

	private void FixedUpdate() {
		if (onTransition) {
			--transitionCount;
			if (transitionCount == 0) {
				onTransition = false;
				GameManager.Instance.Status = prevStatus;
			}
		}
	}

	public void RecieveCollision() {
		GameManager.Instance.InvokeActivateRagdoll(true);
	}

	public void GoRagdoll(bool addForces) {
		if (isOnRagdoll ) return;
		foreach (CollisionDetector g in generatedColliders) {
			g.gameObject.SetActive(false);
		}

		if (!enabled) return;
		if (isOnRagdoll) return;
		isOnRagdoll = true;
		mainRBody.useGravity = false;
		anim.enabled = false;
		mainRBody.velocity = Vector3.zero;

		for (int i = 0; i < ragdollShits.Length; i++) {
			ragdollShits[i].ragdollCollider.isTrigger = false;
			ragdollShits[i].rbody.velocity = Vector3.zero;
		}

		if (addForces) {

			for (int i = 0; i < ragdollShits.Length; i++) {
				//ragdollShits[i].rbody.velocity = ragdollShits[i].rbody.transform.TransformDirection(UnityEngine.Random.Range(ragdollShits[i].minMaxForceToApplyOnRagdoll.x,ragdollShits[i].minMaxForceToApplyOnRagdoll.y) * ragdollShits[i].direction);
				ragdollShits[i].rbody.AddForce(
					forceMultiplier * ragdollShits[i].rbody.transform.TransformDirection(
						UnityEngine.Random.Range(ragdollShits[i].minMaxForceToApplyOnRagdoll.x,
							ragdollShits[i].minMaxForceToApplyOnRagdoll.y) * ragdollShits[i].direction),
					ForceMode.Force);
			}
		}

		GameManager.Instance.Status = ConstantsMovements.fall;
		//ActionGoingRagdoll.Invoke();
	}

	public void ResetThings() {
		isOnRagdoll = false;
		transform.position = initPos;
		transform.rotation = initRot;
		anim.enabled = true;
		onTransition = true;
		prevStatus = ConstantsMovements.idle;
		anim.SetInteger(ConstantsMovements.animTransitionWalk, 0);
		anim.Play(ConstantsMovements.idle);
		GameManager.Instance.Status = ConstantsMovements.transition;

		foreach (CollisionDetector g in generatedColliders) {
			g.gameObject.SetActive(true);
		}

		transitionCount = 2;
		//ActionReturnToAnimation.Invoke();
	}

	public void StartWalking() {
		for (int i = 0; i < ragdollShits.Length; i++) {
			ragdollShits[i].ragdollCollider.isTrigger = true;
			ragdollShits[i].rbody.velocity = Vector3.zero;
		}

		anim.enabled = true;
		GameManager.Instance.Status = ConstantsMovements.transition;
		onTransition = true;
		prevStatus = ConstantsMovements.walking;
		transitionCount = 2;
		if (forceAnim >= 0) {
			anim.SetInteger(ConstantsMovements.animTransitionWalk, forceAnim);
		}
		else {
			anim.SetInteger(ConstantsMovements.animTransitionWalk, UnityEngine.Random.Range(1, walkAnimsCount + 1));
		}

		//ActionStartWalking.Invoke();
	}

	public void GoIdle() {
		anim.SetTrigger(ConstantsMovements.idle);
		anim.SetInteger(ConstantsMovements.animTransitionWalk, 0);
	}

	private void CreateCollisionDetectors() {
		foreach (RagdollShit rs in ragdollShits) {
			Collider c = rs.ragdollCollider;
			CollisionDetector newCol = Instantiate(colliderPrefab, c.transform);

			newCol.gameObject.layer = LayerMask.NameToLayer("NUTHIN");
			if (c is CapsuleCollider) {
				CapsuleCollider ogcol = c as CapsuleCollider;
				CapsuleCollider col = newCol.gameObject.AddComponent<CapsuleCollider>();
				col.radius = ogcol.radius * triggerMultiplier;
				col.center = ogcol.center;
				col.height = ogcol.height * triggerMultiplier;
				col.direction = ogcol.direction;
				col.isTrigger = true;
				newCol.col = col;
			}
			else if (c is BoxCollider) {
				BoxCollider ogcol = c as BoxCollider;
				BoxCollider col = newCol.gameObject.AddComponent<BoxCollider>();
				col.center = ogcol.center;
				col.size = ogcol.size * triggerMultiplier;
				col.isTrigger = true;
				newCol.col = col;
			}
			else if (c is SphereCollider) {
				SphereCollider ogcol = c as SphereCollider;
				SphereCollider col = newCol.gameObject.AddComponent<SphereCollider>();
				col.radius = ogcol.radius * triggerMultiplier;
				col.isTrigger = true;
				newCol.col = col;
			}

			newCol.owner2 = this;
			generatedColliders.Add(newCol);
		}
	}
}