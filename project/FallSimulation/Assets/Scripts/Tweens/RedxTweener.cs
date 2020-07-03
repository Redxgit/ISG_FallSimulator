using System;
using UnityEngine;

public abstract class RedxTweener : MonoBehaviour {
    public static float DefaultDuration = 1f;
    public static Func<float, float, float, float> DefaultEquation = EasingEquations.Linear;

    public static int DefaultLoops = 1;

    public Action OnStarted = delegate { };
    public Action OnCompleted = delegate { };
    public Action OnLooped = delegate { };

    public enum TimeType {
        Normal,
        Real,
        Fixed
    }

    public enum PlayState {
        Stopped,
        Paused,
        Playing,
        Reversing
    }

    public enum LoopType {
        Repeat,
        PingPong
    }

    public enum EndBehaviour {
        Constant,
        Reset
    }

    public TimeType timeType = TimeType.Normal;
    public PlayState playState { get; private set; }
    public PlayState previousPlayState { get; private set; }
    public EndBehaviour endBehaviour = EndBehaviour.Constant;
    public LoopType loopType = LoopType.Repeat;

    public bool IsPlaying {
        get { return playState == PlayState.Playing || playState == PlayState.Reversing; }
    }

    public float startValue = 0f;
    public float endValue = 1.0f;
    public float duration = 1.0f;
    public int loopCount = 0;
    public Func<float, float, float, float> equation = EasingEquations.Linear;

    public float currentPercentage { get; private set; }
    public float currentValue { get; private set; }
    public int loops { get; private set; }

    private void OnEnable() {
        OnStarted.Invoke();
        Resume();
    }

    private void OnDisable() { Pause(); }

    public bool destroyOnComplete = true;

    public void Play() { SetPlayState(PlayState.Playing); }

    public void Pause() {
        if (IsPlaying) SetPlayState(PlayState.Paused);
    }

    public void Resume() {
        Play();
    }

    public void Stop() {
        SetPlayState(PlayState.Stopped);
        previousPlayState = PlayState.Stopped;
        loops = 0;
        if (endBehaviour == EndBehaviour.Reset)
            SeekToBeginning();
        if (destroyOnComplete)
            Destroy(this);
    }

    public void SeekToTime(float time) {
        currentPercentage = Mathf.Clamp01(time / duration);
        currentValue = (endValue - startValue) * currentPercentage + startValue;
    }

    public void SeekToBeginning() { SeekToTime(0.0f); }

    public void SeekToEnd() { SeekToTime(duration); }

    private void SetPlayState(PlayState newState) {
        if (isActiveAndEnabled) {
            if (playState == newState)
                return;

            previousPlayState = playState;
            playState = newState;
        } else {
            previousPlayState = newState;
            playState = PlayState.Paused;
        }
    }

    protected virtual void Update() {
        switch (timeType) {
            case TimeType.Normal:
                Tick(Time.deltaTime);
                break;
            case TimeType.Real:
                Tick(Time.unscaledDeltaTime);
                break;
            default:
                break;
        }
    }

    protected virtual void FixedUpdate() {
        switch (timeType) {
            case TimeType.Fixed:
                Tick(Time.fixedDeltaTime);
                break;
        }
    }

    private void Tick(float time) {
        bool finished = false;

        switch (playState) {
            case PlayState.Playing:
                currentPercentage = Mathf.Clamp01(currentPercentage + (time / duration));
                finished = Mathf.Approximately(currentPercentage, 1.0f);
                break;
            case PlayState.Reversing:
                currentPercentage = Mathf.Clamp01(currentPercentage - (time / duration));
                finished = Mathf.Approximately(currentPercentage, 0.0f);
                break;
            default:
                return;
        }

        currentValue = (endValue - startValue) * equation(0.0f, 1.0f, currentPercentage) + startValue;

        if (finished) {
            ++loops;
            if (loopCount < 0 || loopCount >= loops) {
                if (loopType == LoopType.Repeat)
                    SeekToBeginning();
                else
                    SetPlayState(playState == PlayState.Playing ? PlayState.Reversing : PlayState.Playing);
                OnLooped.Invoke();
            } else {
                OnCompleted.Invoke();
                Stop();
            }
        }
    }
}