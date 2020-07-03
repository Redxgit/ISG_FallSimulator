using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedxTweenSpriteColor : RedxTweener {

	public SpriteRenderer rend;
	public Color startColor;
	public Color targetColor;

	private void Awake() {
		rend = GetComponent<SpriteRenderer>();
	}

	protected override void Update() {
		switch (timeType) {
			case TimeType.Normal:
				base.Update();
				rend.color = (targetColor - startColor) * currentValue + startColor;
				break;
			case TimeType.Real:
				base.Update();
				rend.color = (targetColor - startColor) * currentValue + startColor;
				break;
			default:
				break;
		}
	}

	protected override void FixedUpdate() {
		switch (timeType) {
			case TimeType.Fixed:
				base.FixedUpdate();
				rend.color = (targetColor - startColor) * currentValue + startColor;
				break;
			default:
				break;
		}
	}
}