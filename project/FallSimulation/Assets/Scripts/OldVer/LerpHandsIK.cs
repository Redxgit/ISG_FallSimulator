using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpHandsIK : MonoBehaviour {
    public FromAnimToRagdoll animToRagdoll;
    public LerpPath handsLerper;

    public float torq;

    private bool isLerpingHands;
    private int lerpIndex;

    public bool applyShit;

    private void Start() {
        animToRagdoll.GoingRagdoll += StartLerping;
    }

    public void StartLerping() {
        isLerpingHands = true;
        lerpIndex = 0;
    }

    private void FixedUpdate() {
        if (!isLerpingHands) return;
        if (!applyShit) return;
        if (lerpIndex >= handsLerper.bonesPos[0].localPos.Count) {
            isLerpingHands = false;
            return;
        }

        Debug.Log("Applying shit");

        for (int i = 0; i < handsLerper.bonesPos.Count; i++) {
            //    handsLerper.bonesPos[i].rb.MovePosition(handsLerper.bonesPos[i].rb.position + handsLerper.bonesPos[i].localPos[lerpIndex]);
                handsLerper.bonesPos[i].rb.MoveRotation(handsLerper.bonesPos[i].rb.rotation * handsLerper.bonesPos[i].localRot[lerpIndex]);
            //handsLerper.bonesPos[i].rb.AddTorque(handsLerper.bonesPos[i].transToTrack.up * torq, ForceMode.VelocityChange);
        }
        ++lerpIndex;
    }
}
