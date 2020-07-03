using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedxTransformLocalPositionTweener : RedxTweener {

    public Vector3 startTweenValue;
    public Vector3 endTweenValue;

    protected override void Update() {
        switch (timeType) {
            case TimeType.Normal:
                base.Update();
                transform.localPosition = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            case TimeType.Real:
                base.Update();
                transform.localPosition = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            default:
                break;
        }
    }

    protected override void FixedUpdate() {
        switch (timeType) {
            case TimeType.Fixed:
                base.FixedUpdate();
                transform.localPosition = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            default:
                break;
        }
    }
}