using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromAnimToRagdoll : MonoBehaviour {
    [SerializeField] private Animator anim;

    [SerializeField] private Rigidbody mainRBody;
    [SerializeField] private RagdollShit[] ragdollShits;
    private Vector3 initPos;

    private Quaternion initRot;

    [SerializeField] private float forceToAdd;
    private bool isOnRagdoll;

    public Action GoingRagdoll = delegate { };

    public Action ReturnToAnimation = delegate { };

    [SerializeField] private float triggerMultiplier = 1.05f;

    private List<CollisionDetector> generatedColliders = new List<CollisionDetector>();

    public LayerMask notifierLayer;

    [SerializeField] private CollisionDetector colliderPrefab;

    [SerializeField] private ikthing ikshit;

    [SerializeField] private int walkAnims;

    private string status = ConstantsMovements.idle;
    private string prevStatus;

    private int transitionCount;

    private bool onTransition;

    public bool OnTransition {  get { return onTransition; } }

    public string Status {  get { return status; } }

           

    // Use this for initialization
    void Start() {

        //StartWalking();
        CreateCollisionDetectors();

        initPos = transform.position;
        initRot = transform.rotation;
    }

    

    private void CreateCollisionDetectors() {

        foreach (RagdollShit rs in ragdollShits) {
            Collider c = rs.ragdollCollider;
            CollisionDetector newcol = Instantiate(colliderPrefab, c.transform);
            //newcol.layer = notifierLayer;
            newcol.gameObject.layer = LayerMask.NameToLayer("NUTHIN");
            if (c is CapsuleCollider) {
                CapsuleCollider ogcol = c as CapsuleCollider;
                CapsuleCollider col = newcol.gameObject.AddComponent<CapsuleCollider>();
                col.radius = ogcol.radius * triggerMultiplier;
                col.center = ogcol.center;
                col.height = ogcol.height * triggerMultiplier;
                col.direction = ogcol.direction;
                col.isTrigger = true;
                newcol.col = col;
            } else if (c is BoxCollider) {
                BoxCollider ogcol = c as BoxCollider;
                BoxCollider col = newcol.gameObject.AddComponent<BoxCollider>();
                col.center = ogcol.center;
                col.size = ogcol.size * triggerMultiplier;
                col.isTrigger = true;
                newcol.col = col;
            } else if (c is SphereCollider) {
                SphereCollider ogcol = c as SphereCollider;
                SphereCollider col = newcol.gameObject.AddComponent<SphereCollider>();
                col.radius = ogcol.radius * triggerMultiplier;
                col.isTrigger = true;
                newcol.col = col;
            }
            newcol.owner = this;
            generatedColliders.Add(newcol);
        }
    }

    [ContextMenu("NotifyCollision")]
    public void NotifyCollision() {
        if (isOnRagdoll) return;
        foreach (CollisionDetector g in generatedColliders) {
            g.gameObject.SetActive(false);
        }

        Debug.Log("Entering ragdoll mode");
        GoRagdoll();
        
    }

    public void ResetThings() {
        isOnRagdoll = false;
        ikshit.StartIK();
        transform.position = initPos;
        transform.rotation = initRot;
        anim.SetInteger("TransitionWalk", 0);
        anim.Play("Idle");
        status = ConstantsMovements.transition;

        foreach (CollisionDetector g in generatedColliders) {
            g.gameObject.SetActive(true);
        }

        anim.enabled = true;
        onTransition = true;
        prevStatus = ConstantsMovements.idle;
        transitionCount = 2;
        ReturnToAnimation.Invoke();
    }

    public void GoRagdoll() {
        if (!enabled) return;
        if (isOnRagdoll) return;
        isOnRagdoll = true;
        mainRBody.useGravity = false;
        anim.enabled = false;
        mainRBody.velocity = Vector3.zero;

        for (int i = 0; i < ragdollShits.Length; i++) {
            ragdollShits[i].ragdollCollider.isTrigger = false;
            ragdollShits[i].rbody.velocity = UnityEngine.Random.Range(ragdollShits[i].minMaxForceToApplyOnRagdoll.x, 
                ragdollShits[i].minMaxForceToApplyOnRagdoll.y) * ragdollShits[i].direction;
        }
        status = ConstantsMovements.fall;
        GoingRagdoll.Invoke();
    }

    public void StartWalking() {

        for (int i = 0; i < ragdollShits.Length; i++) {
            ragdollShits[i].ragdollCollider.isTrigger = true;
            ragdollShits[i].rbody.velocity = Vector3.zero;
        }

        anim.enabled = true;
        ikshit.StopIk();
        status = ConstantsMovements.transition;
        onTransition = true;
        prevStatus = ConstantsMovements.walking;
        transitionCount = 2;
        anim.SetInteger("TransitionWalk", UnityEngine.Random.Range(1, walkAnims + 1));

    }

    public void StartIdle() {
        anim.SetTrigger("Idle");
        anim.SetInteger("TransitionWalk", 0);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.M) && !Input.GetKey(KeyCode.LeftShift)) {
            GoRagdoll();
            Debug.Break();
        }

        if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift)) {
            NotifyCollision();
            Debug.Break();
        }        

        if (Input.GetKeyDown(KeyCode.C)) {
            ResetThings();
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            StartWalking();
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            StartIdle();
        }
    }

    private void FixedUpdate() {
        if (onTransition) {
            --transitionCount;
            if (transitionCount == 0) {
                onTransition = false;
                status = prevStatus;
            }
        }
    }
}

[System.Serializable]
public struct RagdollShit {
    public string name;
    public Rigidbody rbody;
    public Collider ragdollCollider;
    public Vector2 minMaxForceToApplyOnRagdoll;
    public Vector3 direction;
}