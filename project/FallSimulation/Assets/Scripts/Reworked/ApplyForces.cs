using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForces : MonoBehaviour {

    [SerializeField] private Rigidbody[] bodiesToApplyForce;
    [SerializeField] private Transform[] posToApplyForce;

    [SerializeField] private float timeToCopy = 0.8f;

    [SerializeField] private AnimController animController;
    [SerializeField] private Vector3[] forcesDirs;
    [SerializeField] private float[] forces;

    [SerializeField] private Vector3[] tdirs;
    [SerializeField] private float[] tforces;

    [SerializeField] private ForceMode forceToUse;

    [SerializeField] private AnimationCurve[] forceCurves;

    public bool Debugging;
    public bool useFixed;

    private float eTime;
    private bool isApplying;

    // Start is called before the first frame update
    void Start() {
        //animController.ActionGoingRagdoll += StartApplying;
        GameManager.Instance.OnActivateRagdoll += StartApplying;
    }

    // Update is called once per frame
    void Update() {
        if (!isApplying) return;
        if (useFixed) return;
        eTime += Time.deltaTime;
        ApplyForce();
        if (eTime > timeToCopy) {
            isApplying = false;
        }
    }

    private void FixedUpdate() {
        if (!isApplying) return;
        if (!useFixed) return;
        eTime += Time.fixedDeltaTime;
        ApplyForce();
        if (eTime > timeToCopy) {
            isApplying = false;
        }
    }


    private void StartApplying(bool applyForces) {
        if (Debugging) Debug.Break();
        isApplying = applyForces;
        eTime = 0f;
    }

    private void ApplyForce() {

        float pct = eTime / timeToCopy;

        

        for (int i = 0; i < bodiesToApplyForce.Length; i++) {
            float mult = 1;
            if (forceCurves[i] != null) 
                mult = forceCurves[i].Evaluate(pct);
            bodiesToApplyForce[i].AddForceAtPosition(posToApplyForce[i].TransformDirection(forcesDirs[i]) * (forces[i] * mult), posToApplyForce[i].position, forceToUse);
            bodiesToApplyForce[i].AddRelativeTorque(tdirs[i] * (tforces[i] * mult), forceToUse);
        }
    }
}
