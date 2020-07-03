using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_player : MonoBehaviour {
	[SerializeField] private Animator anim;
	[SerializeField] private RuntimeAnimatorController[] controllers;
	[SerializeField] private int maxAnims;
	[SerializeField] private int fallController;

	private int ind;

	public Action<string> AnimatorChanged;
	public Action startedPlayingAnim;

	private void Start() {
		AnimatorChanged.Invoke(anim.runtimeAnimatorController.name);
		GameManager.Instance.OnAdvanceController += AdvanceController;
		GameManager.Instance.OnPlayRandomNotFallAnim += PlayRandomAnim;
		GameManager.Instance.OnReturnNewState += ReturnNewStateTrigger;
	}

	[ContextMenu("Advance Controller")]
	public void AdvanceController() {
		ind = ++ind % controllers.Length;
		ChangeController(ind);
	}

	public void ChangeController(int index) {
		anim.runtimeAnimatorController = controllers[index];
		AnimatorChanged.Invoke(anim.runtimeAnimatorController.name);
		GameManager.Instance.Status = ConstantsMovements.idle;
		GameManager.Instance.AdvanceController_();
	}

	private void ReturnNewStateTrigger() {
		anim.SetTrigger("ReturnNewState");
	}

	
	[ContextMenu("Play Random Anim")]
	private void PlayRandomAnim() {
		if (ind == fallController)
			return;
		anim.SetInteger("TransitionID", UnityEngine.Random.Range(0, maxAnims));
		anim.SetTrigger("DoAnim");
		anim.enabled = true;
		startedPlayingAnim.Invoke();
	}
}