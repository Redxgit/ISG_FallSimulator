using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedxTweenValue : RedxTweener {

    public float stVal;
    public float endVal;

    public Action<float> Callback = delegate { };

    protected override void Update() {
        switch (timeType) {
            case TimeType.Normal:
                base.Update();
                Callback.Invoke((endVal - stVal) * currentValue + stVal);
                break;
            case TimeType.Real:
                base.Update();
                Callback.Invoke((endVal - stVal) * currentValue + stVal);
                break;
            default:
                break;
        }
    }

    protected override void FixedUpdate() {
        switch (timeType) {
            case TimeType.Fixed:
                base.FixedUpdate();
                Callback.Invoke((endVal - stVal) * currentValue + stVal);
                break;
            default:
                break;
        }
    }
}