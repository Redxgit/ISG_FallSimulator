using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSlave : MonoBehaviour {

    [SerializeField] private Animator anim;

    private Vector3 startingPos;
    private Quaternion startingRot;

    [SerializeField] private Transform ikTransform;

    [SerializeField] private float baseIkW;

    [SerializeField] private LerpPath lerpPath;

    private float currIkW;

    private bool toggleIK;

    [SerializeField] private FromAnimToRagdoll fatr;
    // Start is called before the first frame update
    void Start() {
        startingPos = transform.position;
        startingRot = transform.rotation;
        fatr.GoingRagdoll += StartIK;
        StopIK();
        fatr.ReturnToAnimation += StopIK;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            anim.SetTrigger("Walk");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            transform.SetPositionAndRotation(startingPos, startingRot);
            anim.SetTrigger("Idle");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            if (toggleIK) {
                toggleIK = false;
                StopIK();
            } else {
                toggleIK = true;
                StartIK();
            }
        }
    }

    

    public void StartIK() {
        lerpPath.Resetear();
        currIkW = baseIkW;
        
    }

    public void StopIK() {
        currIkW = 0;
    }

    private void OnAnimatorIK(int layerIndex) {
        anim.SetIKPosition(AvatarIKGoal.LeftHand, ikTransform.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, ikTransform.rotation);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, currIkW);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, currIkW);
    }
}
