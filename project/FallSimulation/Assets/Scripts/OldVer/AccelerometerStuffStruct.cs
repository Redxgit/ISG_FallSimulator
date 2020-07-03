using UnityEngine;

[System.Serializable]
public struct AccelerometerStuffStruct {
    public float timestamp;
    public float deltaTime;
    public Vector3 values;

    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 gforce;

    public string movementType;

    public AccelerometerStuffStruct(float timestamp, float deltaTime, Vector3 values, Vector3 pos1, Vector3 pos2, Vector3 gforce, string movementType) {
        this.timestamp = timestamp;
        this.deltaTime = deltaTime;
        this.values = values;
        this.pos1 = pos1;
        this.pos2 = pos2;
        this.gforce = gforce;
        this.movementType = movementType;
    }
}

[System.Serializable]
public struct DeltaPositionsStruct {
    public float timestamp;
    public float deltaTime;
    public Vector3 values;
    public Vector3 gForce;
    public string movementType;

    public DeltaPositionsStruct(float timestamp, float deltaTime, Vector3 values, Vector3 gForce, string movementType) {
        this.timestamp = timestamp;
        this.deltaTime = deltaTime;
        this.values = values;
        this.gForce = gForce;
        this.movementType = movementType;
    }
}

public static class ConstantsMovements {
    public static string idle = "Idle";
    public static string fall = "Fall";
    public static string walking = "Walking";
    public static string after_fall = "After_fall";
    public static string transition = "Transition";
    public static string animTransitionWalk = "TransitionWalk";
    public static string notFall = "NotFall";
}