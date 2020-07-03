using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mimicBone : MonoBehaviour {

    /*
     * Limites sup
     * Left arm :
     *  -46.65,     58.743, 13.298
     * Fore arm:
     *  11.814,     81.319, -21.83
     * Hand:
     *  13.579,     -9.259001,  -18.798
     *  
     *  Limites inf:
     *  Left arm :
     *  -11.271,    22.237, 63.054
     *  Fore arm:
     *  18.253, 87.97601,   -4.6
     *  Hand:
     *  25.462, 14.719, -54.537
     */


    /*
         * Limites sup
         * Right arm :
         *  -46.65,     -54.769, -11.474
         * Fore arm:
         *  36.3,     -66.484, 32.109
         * Hand:
         *  42.009,     9.565001,  7.218
         *  
         *  Limites inf:
         *  Right arm :
         *  -11.271,    22.237, 63.054
         *  Fore arm:
         *  18.253, 87.97601,   -4.6
         *  Hand:
         *  25.462, 14.719, -54.537
         */

    [SerializeField] private Transform[] originTransforms;
    [SerializeField] private Rigidbody[] bodiesToNull;

    [SerializeField] private bool useFixed = true;
    [SerializeField] private float timeToCopy = 1f;

    [SerializeField] private FromAnimToRagdoll fotr;
    [SerializeField] private Vector3[] dirs;
    [SerializeField] private float[] forces;
    [SerializeField] private Vector3[] Tdirs;
    [SerializeField] private float[] Tforces;
    [SerializeField] private Transform[] targetPoses;

    [SerializeField] private Vector3[] bonesLimitsMax;
    [SerializeField] private Vector3[] bonesLimitsMin;

    private float eTime;
    private bool iscopying;

    [SerializeField] private ForceMode forceToUse;
    [SerializeField] private float angleLimit = 100f;

    [SerializeField] private bool allowAnglesOverLimit;

    public bool Debugging;

    public bool zerorb;
    // Start is called before the first frame update
    void Start() {
        fotr.GoingRagdoll += StartCopying;
    }

    // Update is called once per frame
    void Update() {
        if (!iscopying) return;
        if (useFixed) return;
        eTime += Time.deltaTime;
        ForceOnBone();
        if (eTime > timeToCopy) {
            iscopying = false;
        }

    }

    private void FixedUpdate() {
        if (!iscopying) return;
        if (!useFixed) return;
        eTime += Time.fixedDeltaTime;
        ForceOnBone();
        if (eTime > timeToCopy) {
            iscopying = false;
        }
    }

    private void ForceOnBone() {
        for (int i = 0; i < originTransforms.Length; i++) {
            //bodiesToNull[i].AddRelativeTorque(Tdirs[i] * Tforces[i], forceToUse);
            //bodiesToNull[i].AddRelativeForce(dirs[i] * forces[i], forceToUse);    
            if ((Quaternion.Angle(bodiesToNull[i].transform.localRotation, Quaternion.Euler(bonesLimitsMax[i])) < 100 &&
                Quaternion.Angle(bodiesToNull[i].transform.localRotation, Quaternion.Euler(bonesLimitsMin[i])) < 100) || allowAnglesOverLimit) {
                bodiesToNull[i].AddForceAtPosition(targetPoses[i].transform.TransformDirection(dirs[i]) * forces[i], targetPoses[i].position, forceToUse);                
                bodiesToNull[i].AddRelativeTorque(Tforces[i] * Tdirs[i], forceToUse);
            } else {
                Debug.Log(bodiesToNull[i].transform.name + " pasado de rotacion");
            }
        }
    }    

    public void StartCopying() {
        if (Debugging) Debug.Break();
        iscopying = true;
        eTime = 0f;
    }
}
