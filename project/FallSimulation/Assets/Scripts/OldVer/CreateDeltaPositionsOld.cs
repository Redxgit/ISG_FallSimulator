using System.Collections.Generic;
using UnityEngine;

public class CreateDeltaPositionsOld : MonoBehaviour {
    //[SerializeField] private PositionList posList;
    [SerializeField] private List<DeltaPositionsStruct> posList;
    [SerializeField] private bool useFixedUpdate = true;
    [SerializeField] private Transform transformToTrack;
    [SerializeField] private AccelerometerFromDeltaPosition accelerometer;
    [SerializeField] private FromAnimToRagdoll anim;

    [SerializeField] private Transform origin;
    [SerializeField] private Transform target;
    [SerializeField] private float duration;
    [SerializeField] private int reps;
    [SerializeField] private float distanceTh;
    [SerializeField] private int countThFall;

    private int countNotFall;
    private Vector3 lastPos;
    private bool fallEnded;

    private int count;

    private void Start() {
        posList = new List<DeltaPositionsStruct>();

        if (anim != null) {
            anim.ReturnToAnimation += ResetThings;
        }

        if (origin != null) {

            transformToTrack.localPosition = origin.position;

            RedxTweener tweener = transformToTrack.RedxMoveToLocal(target.position, duration, EasingEquations.Linear);
            tweener.loopCount = reps;
            tweener.loopType = RedxTweener.LoopType.PingPong;
            tweener.destroyOnComplete = true;
            tweener.timeType = RedxTweener.TimeType.Fixed;
            tweener.OnCompleted += EndRecording;
        }
    }

    public void ResetThings() {
        posList = new List<DeltaPositionsStruct>();
        accelerometer.ResetThings();
        count = 0;
    }

    private void EndRecording() {
        enabled = false;
        accelerometer.WriteInfoToFile();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.X)) {
            EndRecording();
        }

        if (useFixedUpdate)
            return;
        if (!fallEnded)
            posList.Add(new DeltaPositionsStruct(count, Time.time, transformToTrack.position, transformToTrack.InverseTransformDirection(Vector3.down), anim.Status));
        else
            posList.Add(new DeltaPositionsStruct(count, Time.time, transformToTrack.position, transformToTrack.InverseTransformDirection(Vector3.down), ConstantsMovements.after_fall));
        ++count;
        if (count > 1) {
            accelerometer.CreateAccelerometerListFromDeltaPos(posList, count - 2);
        }
        float currDist = Vector3.Distance(transformToTrack.position, lastPos);
        if (anim.Status.Equals(ConstantsMovements.fall)) {
            if (currDist < distanceTh) {
                ++countNotFall;
                if (countNotFall > countThFall) {
                    fallEnded = true;
                }
            } else {
                countNotFall = countThFall;
            }
        }
        lastPos = transformToTrack.position;
    }

    private void FixedUpdate() {
        if (!useFixedUpdate)
            return;

        if (!fallEnded)
            posList.Add(new DeltaPositionsStruct(count, Time.time, transformToTrack.position, transformToTrack.InverseTransformDirection(Vector3.down), anim.Status));
        else
            posList.Add(new DeltaPositionsStruct(count, Time.time, transformToTrack.position, transformToTrack.InverseTransformDirection(Vector3.down), ConstantsMovements.after_fall));
        ++count;
        if (count > 1) {
            accelerometer.CreateAccelerometerListFromDeltaPos(posList, count - 2);
        }

        float currDist = Vector3.Distance(transformToTrack.position, lastPos);
        if (anim.Status.Equals(ConstantsMovements.fall)) {
            if (currDist < distanceTh) {
                ++countNotFall;
                if (countNotFall > countThFall) {
                    fallEnded = true;
                }
            } else {
                countNotFall = countThFall;
            } 
        }
        lastPos = transformToTrack.position;
    }

    [ContextMenu("wth")]
    private void wth() {
        for (int i = 0; i < 10; i++) {
            Debug.Log(posList[i].values.ToString("F8"));
        }

    }
}