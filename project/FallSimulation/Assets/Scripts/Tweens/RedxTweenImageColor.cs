using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedxTweenImageColor : RedxTweener {

	public Image img;
	public Color startColor;
	public Color targetColor;

	private void Awake() {
		img = GetComponent<Image>();
	}

	protected override void Update() {
		switch (timeType) {
			case TimeType.Normal:
				base.Update();
				img.color = (targetColor - startColor) * currentValue + startColor;
				break;
			case TimeType.Real:
				base.Update();
				img.color = (targetColor - startColor) * currentValue + startColor;
				break;
			default:
				break;
		}
	}

	protected override void FixedUpdate() {
		switch (timeType) {
			case TimeType.Fixed:
				base.FixedUpdate();
				img.color = (targetColor - startColor) * currentValue + startColor;
				break;
			default:
				break;
		}
	}

}