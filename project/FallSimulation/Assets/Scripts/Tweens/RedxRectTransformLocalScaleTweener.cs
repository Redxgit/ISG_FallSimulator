using UnityEngine;

public class RedxRectTransformLocalScaleTweener : RedxTweener {
    private RectTransform rt;
    public Vector3 startTweenValue;
    public Vector3 endTweenValue;

    private void Awake() { rt = transform as RectTransform; }

    protected override void Update() {
        switch (timeType) {
            case TimeType.Normal:
                base.Update();
                rt.localScale = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            case TimeType.Real:
                base.Update();
                rt.localScale = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            default:
                break;
        }
    }

    protected override void FixedUpdate() {
        switch (timeType) {
            case TimeType.Fixed:
                base.FixedUpdate();
                rt.localScale = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            default:
                break;
        }
    }
}