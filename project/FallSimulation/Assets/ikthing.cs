using System;
using UnityEngine;

public class ikthing : MonoBehaviour {
    [SerializeField] private Animator anim;

    [SerializeField] private Transform targetLeft;
    [SerializeField] private Transform targetRight;

    [SerializeField] private float rotationW;
    [SerializeField] private float positionW;

    private float currRotW;
    private float currPosW;

    [SerializeField] private bool useRight;
    [SerializeField] private bool useLeft;

    private void Start() {
        currPosW = positionW;
        currRotW = rotationW;
    }

    // Start is called before the first frame update
    private void OnAnimatorIK(int layerIndex) {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, currPosW); 
        anim.SetIKPosition(AvatarIKGoal.LeftHand, targetLeft.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, targetLeft.rotation);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, currRotW);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, currPosW);
        anim.SetIKPosition(AvatarIKGoal.RightHand, targetRight.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, targetRight.rotation);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, currRotW);

    }

    [ContextMenu("StartIK")]
    internal void StartIK() {
        currPosW = positionW;
        currRotW = rotationW;
    }

    [ContextMenu("StopIK")]
    internal void StopIk() {
        currPosW = 0;
        currRotW = 0;
    }
}