using System;
using UnityEngine;
using UnityEngine.UI;

public static class RedxAnimationExtensions {

    public static RedxTweener RedxTimer (this GameObject obj) {
        return RedxTimer (obj, RedxTweener.DefaultDuration);
    }

    public static RedxTweener RedxTimer (this GameObject obj, float timer) {
        return RedxTimer (obj, timer, RedxTweener.DefaultLoops);
    }

    public static RedxTweener RedxTimer (this GameObject obj, float timer, int loops) {
        return RedxTimer (obj, timer, loops, true);
    }

    public static RedxTweener RedxTimer (this GameObject obj, float timer, int loops, bool destroyOnComplete) {
        RedxTweener tweener = obj.AddComponent<RedxTimer> ();
        tweener.duration = timer;
        tweener.loopCount = loops;
        tweener.destroyOnComplete = destroyOnComplete;
        tweener.Play ();
        return tweener;
    }

    public static RedxTweener RedxMoveToGlobal (this Transform t, Vector3 position) {
        return RedxMoveToGlobal (t, position, RedxTweener.DefaultDuration);
    }

    public static RedxTweener RedxMoveToGlobal (this Transform t, Vector3 position, float duration) {
        return RedxMoveToGlobal (t, position, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener RedxMoveToGlobal (this Transform t, Vector3 position, float duration,
        Func<float, float, float, float> equation) {
        RedxTransformLocalPositionTweener tweener = t.gameObject.AddComponent<RedxTransformLocalPositionTweener> ();
        tweener.startTweenValue = t.position;
        tweener.endTweenValue = position;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Play ();
        return tweener;
    }

    public static RedxTweenValue RedxTweenValue (this GameObject obj, float startValue, float targetValue, Action<float> callback) {
        return RedxTweenValue (obj, startValue, targetValue, callback, RedxTweener.DefaultDuration);
    }

    public static RedxTweenValue RedxTweenValue (this GameObject obj, float startValue, float targetValue, Action<float> callback, float duration) {
        return RedxTweenValue (obj, startValue, targetValue, callback, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweenValue RedxTweenValue (this GameObject obj, float startValue, float targetValue, Action<float> callback, float duration, Func<float, float, float, float> equation) {
        RedxTweenValue tweener = obj.AddComponent<RedxTweenValue> ();
        tweener.stVal = startValue;
        tweener.endVal = targetValue;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Callback += callback;
        tweener.Play ();
        return tweener;

    }

    public static RedxTweener RedxMoveToLocal (this Transform t, Vector3 position) {
        return RedxMoveToLocal (t, position, RedxTweener.DefaultDuration);
    }

    public static RedxTweener RedxMoveToLocal (this Transform t, Vector3 position, float duration) {
        return RedxMoveToLocal (t, position, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener RedxMoveToLocal (this Transform t, Vector3 position, float duration,
        Func<float, float, float, float> equation) {
        RedxTransformLocalPositionTweener tweener = t.gameObject.AddComponent<RedxTransformLocalPositionTweener> ();
        tweener.startTweenValue = t.localPosition;
        tweener.endTweenValue = position;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Play ();
        return tweener;
    }

    public static RedxTweener RedxScaleTo (this Transform t, Vector3 scale) {
        return RedxMoveToLocal (t, scale, RedxTweener.DefaultDuration);
    }

    public static RedxTweener RedxScaleTo (this Transform t, Vector3 scale, float duration) {
        return RedxMoveToLocal (t, scale, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener RedxScaleTo (this Transform t, Vector3 scale, float duration,
        Func<float, float, float, float> equation) {
        RedxTransformLocalScaleTweener tweener = t.gameObject.AddComponent<RedxTransformLocalScaleTweener> ();
        tweener.startTweenValue = t.localPosition;
        tweener.endTweenValue = scale;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Play ();
        return tweener;
    }

    public static RedxTweener RTMoveTo (this RectTransform t, Vector2 position) {
        return RTMoveTo (t, position, RedxTweener.DefaultDuration);
    }

    public static RedxTweener RTMoveTo (this RectTransform t, Vector2 position, float duration) {
        return RTMoveTo (t, position, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener RTMoveTo (this RectTransform t, Vector2 position, float duration,
        Func<float, float, float, float> equation) {
        RedxRectTransformAnchoredPositionTweener anchoredPositionTweener = t.gameObject.AddComponent<RedxRectTransformAnchoredPositionTweener> ();
        anchoredPositionTweener.startTweenValue = t.anchoredPosition;
        anchoredPositionTweener.endTweenValue = position;
        anchoredPositionTweener.duration = duration;
        anchoredPositionTweener.equation = equation;
        anchoredPositionTweener.Play ();
        return anchoredPositionTweener;
    }

    public static RedxTweener RTScaleTo (this RectTransform t, Vector3 scale) {
        return RTScaleTo (t, scale, RedxTweener.DefaultDuration);
    }

    public static RedxTweener RTScaleTo (this RectTransform t, Vector3 scale, float duration) {
        return RTScaleTo (t, scale, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener RTScaleTo (this RectTransform t, Vector3 scale, float duration,
        Func<float, float, float, float> equation) {
        RedxRectTransformLocalScaleTweener anchoredPositionTweener = t.gameObject.AddComponent<RedxRectTransformLocalScaleTweener> ();
        anchoredPositionTweener.startTweenValue = t.localScale;
        anchoredPositionTweener.endTweenValue = scale;
        anchoredPositionTweener.duration = duration;
        anchoredPositionTweener.equation = equation;
        anchoredPositionTweener.Play ();
        return anchoredPositionTweener;
    }

    public static RedxTweener ImageLerpColorTo (this Image img, Color target) {
        return ImageLerpColorTo (img, target, RedxTweener.DefaultDuration);
    }

    public static RedxTweener ImageLerpColorTo (this Image img, Color target, float duration) {
        return ImageLerpColorTo (img, target, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener ImageLerpColorTo (this Image img, Color target, float duration,
        Func<float, float, float, float> equation) {
        RedxTweenImageColor tweener = img.gameObject.AddComponent<RedxTweenImageColor> ();
        tweener.startColor = img.color;
        tweener.targetColor = target;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Play ();
        return tweener;
    }

    public static RedxTweener SpriteLerpColorTo (this SpriteRenderer rend, Color target) {
        return SpriteLerpColorTo (rend, target, RedxTweener.DefaultDuration);
    }

    public static RedxTweener SpriteLerpColorTo (this SpriteRenderer rend, Color target, float duration) {
        return SpriteLerpColorTo (rend, target, duration, RedxTweener.DefaultEquation);
    }

    public static RedxTweener SpriteLerpColorTo (this SpriteRenderer rend, Color target, float duration,
        Func<float, float, float, float> equation) {
        RedxTweenSpriteColor tweener = rend.gameObject.AddComponent<RedxTweenSpriteColor> ();
        tweener.startColor = rend.color;
        tweener.targetColor = target;
        tweener.duration = duration;
        tweener.equation = equation;
        tweener.Play ();
        return tweener;
    }

    public static void FlipImage (this Image img) {
        img.transform.localEulerAngles = new Vector3 (0, img.transform.localRotation.y + 180, 0);
    }
}