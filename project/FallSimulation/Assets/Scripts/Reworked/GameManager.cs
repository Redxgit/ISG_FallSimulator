using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	private void Awake() {
		// if the singleton hasn't been initialized yet
		if (Instance != null && Instance != this) {
			Destroy(this.gameObject);
			return; //Avoid doing anything else
		}

		Instance = this;
	}

	public void InvokeResetThings() {
		OnResetThings.Invoke();
	}

	public void InvokeStartMoving() {
		OnStartMoving.Invoke();
	}

	public void InvokeFallEnded() {
		OnFallEnded.Invoke();
	}

	public void InvokeActivateRagdoll(bool applyForces) {
		OnActivateRagdoll.Invoke(applyForces);
	}

	public Action OnStartMoving = delegate { };
	public Action OnResetThings = delegate { };
	public Action OnFallEnded = delegate { };
	public Action<bool> OnActivateRagdoll = delegate { };
	
	public Action OnResetRecording = delegate {  };
	public Action OnStartRecording = delegate {  };
	public Action OnEndRecordingAndWriteToFile = delegate {  };
	
	public Action OnAdvanceController = delegate {  };
	public Action OnPlayRandomNotFallAnim = delegate {  };
	
	public Action OnReturnNewState = delegate {  };

	[SerializeField] private string status = ConstantsMovements.idle;

	public string Status {
		get => status;
		set {
			prevStatus = status;
			status = value;
			Debug.Log("Updated to "  + value + " from " + prevStatus);
			UpdateTimes();
		} 
	}

	private void UpdateTimes() {
		if (status.Equals(ConstantsMovements.after_fall) && prevStatus.Equals(ConstantsMovements.fall)) {
			timeStartAfterFall = Time.time;
			checkAfterFall = true;
		}
	}


	public float distanceTh;
	public int countThFall;
	public float delayAfterFall;
	public float waitTimeAtIdle;
	public float maxTimeBetweenResets;
	public float timeResetRecording;
	

	private string prevStatus;
	private float timeStartAfterFall;
	private float timeDelayStartRecording;
	private float timeStartWalking;
	private float timeStartIdle;
	private float lastTimeReset;
	private bool checkAfterFall;
	private bool delayResetRecording;
	private bool delayStartWalking;

	public bool onSyncope;

	public Text fname;
	
	public string FileName => fname.text;


	public void Update() {
		ManageInput();
		ManageTime();
	}

	private void Start() {
		OnStartRecording.Invoke();
		lastTimeReset = Time.time;
		
		OnResetThings.Invoke();
		lastTimeReset = Time.time;
		delayResetRecording = true;
		checkAfterFall = false;
		timeDelayStartRecording = Time.time;
	}

	public void AdvanceController_() {
		OnResetThings.Invoke();
		lastTimeReset = Time.time;
		delayResetRecording = true;
		checkAfterFall = false;
		timeDelayStartRecording = Time.time;
		onSyncope = !onSyncope;
	}
	
	

	private void ManageTime() {
		if (checkAfterFall) {
			if (Time.time > timeStartAfterFall + delayAfterFall) {
				if (status.Equals(ConstantsMovements.after_fall) && prevStatus.Equals(ConstantsMovements.fall))
					OnEndRecordingAndWriteToFile.Invoke();
				
				OnResetThings.Invoke();
				lastTimeReset = Time.time;
				delayResetRecording = true;
				checkAfterFall = false;
				timeDelayStartRecording = Time.time;
			}
		}

		if (delayResetRecording) {
			if (Time.time > timeDelayStartRecording + timeResetRecording) {
				delayResetRecording = false;
				OnStartRecording.Invoke();
				delayStartWalking = true;
				timeStartIdle = Time.time;
			}
		}

		if (delayStartWalking) {
			if (Time.time > timeStartIdle + waitTimeAtIdle) {
				if (!onSyncope) {
					InvokeStartMoving();
				}
				else {
					OnActivateRagdoll.Invoke(false);
					status = ConstantsMovements.fall;
				}
				delayStartWalking = false;

			}
		}

		if (Time.time > lastTimeReset + maxTimeBetweenResets) {
			lastTimeReset = Time.time;
			OnResetThings.Invoke();
			delayResetRecording = true;
			checkAfterFall = false;
			timeDelayStartRecording = Time.time;
		}
	}

	private void ManageInput() {
		if (Input.GetKeyDown(KeyCode.X)) {
			OnEndRecordingAndWriteToFile.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.Z)) {
			OnResetRecording.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.V)) {
			OnAdvanceController.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.B)) {
			OnPlayRandomNotFallAnim.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.N)) {
			OnReturnNewState.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.M) && !Input.GetKey(KeyCode.LeftShift)) {
			OnActivateRagdoll.Invoke(true);
		}

		if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift)) {
			OnActivateRagdoll.Invoke(false);
		}

		if (Input.GetKeyDown(KeyCode.C)) {
			OnResetThings.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			OnStartRecording.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			//StartWalking();
			InvokeStartMoving();
		}

		if (Input.GetKey(KeyCode.K)) {
			ScreenCapture.CaptureScreenshot(sshotName);
		}
	}

	public string sshotName = "s1.png";
	[ContextMenu("SShot")]
	public void TakeSShot() {
		
	}
}