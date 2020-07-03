using UnityEngine;

public class RedxRectTransformAnchoredPositionTweener : RedxTweener {

    private RectTransform rt;
    public Vector2 startTweenValue;
    public Vector2 endTweenValue;

    private void Awake() {
        rt = transform as RectTransform;
    }

    protected override void Update() {
        switch (timeType) {
            case TimeType.Normal:
                base.Update();
                rt.anchoredPosition = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            case TimeType.Real:
                base.Update();
                rt.anchoredPosition = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            default:
                break;
        }
    }

    protected override void FixedUpdate() {
        switch (timeType) {
            case TimeType.Fixed:
                base.FixedUpdate();
                rt.anchoredPosition = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
                break;
            default:
                break;
        }
    }

}