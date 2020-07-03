using System;
using UnityEngine;

public class TweenToPos : MonoBehaviour {
    [SerializeField] private Transform transformToMove;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform target;
    [SerializeField] private float duration;
    [SerializeField] private int reps;
    
    public Action thingEnded;
    

    private Vector3 originV3;
    private Vector3 targetV3;

    private void Start() {
        
        originV3 = origin.localPosition;
        targetV3 = target.localPosition;

        transformToMove.localPosition = originV3;
        
        RedxTweener tweener = transformToMove.RedxMoveToLocal(targetV3, duration, EasingEquations.EaseInOutQuad);
        tweener.loopCount = reps;
        tweener.loopType = RedxTweener.LoopType.PingPong;
        tweener.destroyOnComplete = true;
        tweener.OnCompleted += thingEnded.Invoke;
    }
}