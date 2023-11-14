using UnityEngine;
using EnemyAI;

// 搜索兴趣点动作。
[CreateAssetMenu(menuName = "Enemy AI/Actions/Search")]
public class SearchAction : Action
{
	// Act函数，在Update()中调用（State controller - current state - action）。
	public override void Act(StateController controller)
	{
		if (Equals(controller.personalTarget, Vector3.positiveInfinity))
			controller.nav.destination = controller.transform.position;
		else
		{
			// 设置导航参数。
			controller.nav.speed = controller.generalStats.chaseSpeed;
			controller.nav.destination = controller.personalTarget;
		}
	}
	// 启用行动时的功能，在FSM状态转换后触发一次。
	public override void OnEnableAction(StateController controller)
	{
		// 为动作设置初始值。
		controller.focusSight = false;
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool("Crouch", false);
		controller.CoverSpot = Vector3.positiveInfinity;
	}
}