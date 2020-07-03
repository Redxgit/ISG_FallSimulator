using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPath : MonoBehaviour {

    [SerializeField] private Transform[] pathToFollow;
    [SerializeField] private float timePerStep;

    [SerializeField] private Transform transformToMove;

    private float eTime;
    private int pathIndex;
    private Vector3 startingPos;
    private Quaternion startingRot;
    private bool done = false;
    private bool replaying = false;
    private int replayIndex;
    private int replayCount;

    [SerializeField] private bool useFixed = true;
    [SerializeField] private Animator anim;

    [System.Serializable]
    public struct LerpBoneStruct {
        public Transform transToTrack;
        public Rigidbody rb;
        public List<Vector3> localPos;
        public List<Quaternion> localRot;     
        
        public void ResetThings() {
            localPos.Clear();
            localRot.Clear();            
        }

        public void AddValue() {
            this.localPos.Add(transToTrack.localPosition);
            this.localRot.Add(transToTrack.localRotation);
        }

        public void SetRb() {
            rb = transToTrack.GetComponent<Rigidbody>();
        }
    }

    [ContextMenu("cacherb")]
    private void CacheRb() {
        for (int i = 0; i < bonesPos.Count; i++) {
            bonesPos[i].SetRb();
        }
    }

    Rigidbody rb;

    public List<LerpBoneStruct> bonesPos = new List<LerpBoneStruct>();

    void Start() {
        transformToMove.SetPositionAndRotation(pathToFollow[0].position, pathToFollow[0].rotation);
        startingPos = pathToFollow[0].position;
        startingRot = pathToFollow[0].rotation;
        pathIndex = 1;        
    }

    // Update is called once per frame
    private void Update() {
        if (!useFixed && !done) {
            eTime += Time.deltaTime;
            DoMovement();
        }

        if (!useFixed && replaying) {
            DoReplay();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetPos();
        }
        if (Input.GetKeyDown(KeyCode.T) && !Input.GetKey(KeyCode.LeftShift)) {
            Resetear();
        }

        if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift)) {
            Resetear();
            Debug.Break();
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            StartReplay();
        }
    }

    private void DoReplay() {
        ++replayIndex;
        if (replayIndex >= replayCount) {
            replaying = false;
        } else {
            for (int i = 0; i < bonesPos.Count; i++) {
                bonesPos[i].transToTrack.localPosition = bonesPos[i].localPos[replayIndex];
                bonesPos[i].transToTrack.localRotation = bonesPos[i].localRot[replayIndex];
            }
        }
    }

    private void StartReplay() {
        replaying = true;
        eTime = 0f;
        replayIndex = 0;
        replayCount = bonesPos[0].localPos.Count;

        for (int i = 0; i < bonesPos.Count; i++) {
            bonesPos[i].transToTrack.localPosition = bonesPos[i].localPos[0];
            bonesPos[i].transToTrack.localRotation = bonesPos[i].localRot[0];
        }
    }

    private void FixedUpdate() {
        if (useFixed && !done) {
            eTime += Time.fixedDeltaTime;
            DoMovement();
        }

        if (useFixed && replaying) {
            DoReplay();
        }
    }

    private void DoMovement() {
        float percentage = eTime / timePerStep;
        if (percentage >= 1.0f) {
            percentage = 1.0f;
            transformToMove.SetPositionAndRotation(pathToFollow[pathIndex].position, pathToFollow[pathIndex].rotation);
            startingRot = pathToFollow[pathIndex].rotation;
            startingPos = pathToFollow[pathIndex].position;
            pathIndex += 1;
            eTime = 0f;


            for (int i = 0; i < bonesPos.Count; i++) {
                bonesPos[i].AddValue();
            }

            if (pathIndex >= pathToFollow.Length) done = true;
        } else {

            transformToMove.SetPositionAndRotation(Vector3.Lerp(startingPos, pathToFollow[pathIndex].position, percentage), 
                Quaternion.Slerp(startingRot, pathToFollow[pathIndex].rotation, percentage));
            
            for (int i = 0; i < bonesPos.Count; i++) {
                bonesPos[i].AddValue();
            }
        }

    }    

    [ContextMenu("Resetear")]
    public void Resetear() {
        eTime = 0f;
        pathIndex = 1;
        transformToMove.SetPositionAndRotation(pathToFollow[0].position, pathToFollow[0].rotation);
        startingPos = pathToFollow[0].position;
        startingRot = pathToFollow[0].rotation;
        done = false;
        Debug.Log("WTH");

        for (int i = 0; i < bonesPos.Count; i++) {
            bonesPos[i].ResetThings();
        }
    }

    [ContextMenu("ResetPos")]
    private void ResetPos() {
        eTime = 0f;
        pathIndex = 1;
        transformToMove.SetPositionAndRotation(pathToFollow[0].position, pathToFollow[0].rotation);
        startingPos = pathToFollow[0].position;
        startingRot = pathToFollow[0].rotation;
        done = true;
    }
}
