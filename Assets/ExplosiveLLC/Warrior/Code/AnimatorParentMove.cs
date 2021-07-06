/// <summary>
/// Enables Root Motion in the animation to drive the Warrior movement.
/// </summary>

using UnityEngine;

namespace WarriorAnims
{
	public class AnimatorParentMove:MonoBehaviour
	{
		public Animator animator;
		public HeroMovement heroMovement;

		/// <summary>
		/// If there is translation in the Root Motion Node, as defined in each animation file under 
		/// Motion > Root Motion Node, then use that motion to move the Warrior if the controller is
		/// set to useRootMotion.
		/// </summary>
		void OnAnimatorMove()
		{
			if(heroMovement.useRootMotion) {
				transform.parent.rotation = animator.rootRotation;
				transform.parent.position += animator.deltaPosition;
			}
		}
	}
}
