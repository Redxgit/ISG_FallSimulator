using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMovement : MonoBehaviour {


    [SerializeField] private float shakeStrength;

    //[SerializeField] private float shakeRotationStrength;
    private Vector3 targetPos = Vector3.zero;

    //private Vector3 offsetRotation = Vector3.zero;
    [SerializeField] private Vector2 shakeDuration;
    [SerializeField] private AnimController anim;
    [SerializeField] private Transform objectToMove;
    private float intShakeDuration;
    public bool shaking;
    private float eTimeShaking;
    private Vector3 initPos;

    void Start() {
        //anim.ActionStartWalking += StartShaking;
        //anim.ActionReturnToAnimation += StopShaking;
        GameManager.Instance.OnStartMoving += StartShaking;
        GameManager.Instance.OnResetThings += StopShaking;
    }

    public void StartShaking() {
        shaking = true;
        intShakeDuration = Random.Range(shakeDuration.x, shakeDuration.y);
        targetPos = Random.insideUnitSphere * shakeStrength;
        initPos = objectToMove.localPosition;
        eTimeShaking = 0f;
    }

    public void StopShaking() {
        shaking = false;
        eTimeShaking = 0f;
    }

    // Update is called once per frame
    void Update() {
        if (shaking) {
            if (eTimeShaking < intShakeDuration) {
                eTimeShaking += Time.deltaTime;
            } else {
                StartShaking();
            }
            objectToMove.localPosition = Vector3.Lerp(initPos, targetPos, eTimeShaking / intShakeDuration);
        }
    }
}
