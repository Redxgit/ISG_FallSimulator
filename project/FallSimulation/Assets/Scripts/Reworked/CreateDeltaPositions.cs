using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDeltaPositions : MonoBehaviour {
	[SerializeField] private List<DeltaPositionsStruct> posList;
	[SerializeField] private bool useFixedUpdate = true;
	[SerializeField] private Transform transformToTrack;
	[SerializeField] private AccelerometerFromDeltaPosition accelerometer;
	[SerializeField] private AnimController anim;


	[SerializeField] private animation_player animPlayer;

	private int countNotFall;
	private Vector3 lastPos;
	private bool fallEnded;
	private int count;

	private int countThFall;
	private float distanceTh;

	private void Start() {
		posList = new List<DeltaPositionsStruct>();
		countThFall = GameManager.Instance.countThFall;
		distanceTh = GameManager.Instance.distanceTh;

		if (anim != null) {
			//anim.ActionReturnToAnimation += ResetThings;
			//anim.ActionStartWalking += StartRecording;
			GameManager.Instance.OnResetThings += ResetThings;
			GameManager.Instance.OnStartMoving += StartRecording;
			GameManager.Instance.OnStartRecording += StartRecording;
			GameManager.Instance.OnResetRecording += ResetThings;
			GameManager.Instance.OnEndRecordingAndWriteToFile += EndRecordingAndWrite;
		}

		enabled = false;
		animPlayer.startedPlayingAnim += StartPlayingNotFallAnim;
	}

	private void StartPlayingNotFallAnim() {
		GameManager.Instance.Status = ConstantsMovements.notFall;
		StartRecording();
	}

	private void StartRecording() {
		posList = new List<DeltaPositionsStruct>();
		accelerometer.ResetThings();
		fallEnded = false;
		count = 0;
		enabled = true;
	}

	private void ResetThings() {
		posList = new List<DeltaPositionsStruct>();
		accelerometer.ResetThings();
		fallEnded = false;
		count = 0;
	}

	private void EndRecordingAndWrite() {
		enabled = false;
		accelerometer.WriteInfoToFile();
	}

	private void Update() {
		/*
		if (Input.GetKeyDown(KeyCode.X))
		{
		    EndRecording();
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
		    ResetThings();
		}
*/
		if (useFixedUpdate)
			return;
		DoTick(Time.deltaTime);
	}

	private void DoTick(float dtime) {
		if (!fallEnded)
			posList.Add(new DeltaPositionsStruct(
				Time.time, dtime, transformToTrack.position,
				transformToTrack.InverseTransformDirection(Vector3.down),
				anim.Status));
		else
			posList.Add(new DeltaPositionsStruct(
				Time.time, dtime, transformToTrack.position,
				transformToTrack.InverseTransformDirection(Vector3.down),
				GameManager.Instance.Status));

		++count;

		if (count > 1) {
			accelerometer.CreateAccelerometerListFromDeltaPos(posList, count - 2);
		}

		float currDist = Vector3.Distance(transformToTrack.position, lastPos);
		if (anim.Status.Equals(ConstantsMovements.fall)) {
			if (currDist < distanceTh) {
				++countNotFall;
				if (countNotFall > countThFall) {
					fallEnded = true;
					GameManager.Instance.Status = ConstantsMovements.after_fall;
				}
			}
			else {
				countNotFall = countThFall;
			}
		}

		lastPos = transformToTrack.position;
	}

	private void FixedUpdate() {
		if (!useFixedUpdate)
			return;
		DoTick(Time.fixedDeltaTime);
	}
}