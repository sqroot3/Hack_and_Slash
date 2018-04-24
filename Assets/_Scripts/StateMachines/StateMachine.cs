using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : StateMachineBehaviour {
	public float flDamping = 0.15f;

	private readonly int hashHorizontalParameter = Animator.StringToHash("Horizontal");
	private readonly int hashVerticalParameter = Animator.StringToHash("Vertical");

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		float flHorizontal = Input.GetAxis("Sides");
		float flVertical = Input.GetAxis("ForwardBack");

		Vector2 vecInput = new Vector2(flHorizontal, flVertical).normalized;

		animator.SetFloat(hashHorizontalParameter, vecInput.x, flDamping, Time.deltaTime);
		animator.SetFloat(hashVerticalParameter, flVertical, flDamping, Time.deltaTime);
	}
}
