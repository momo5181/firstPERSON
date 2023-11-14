using UnityEngine;
using EnemyAI;

// 导航到可以清晰看到目标的位置动作。
[CreateAssetMenu(menuName = "Enemy AI/Actions/Go To Shot Spot")]
public class GoToShotSpotAction : Action
{
	// Act函数，在Update()中调用（State controller - current state - action）。
	public override void Act(StateController controller)
	{
	}
	// 启用行动时的功能，在FSM状态转换后触发一次。
	public override void OnEnableAction(StateController controller)
	{
		// 为行动设置初始值。
		controller.focusSight = false;
		controller.nav.destination = controller.personalTarget;
		controller.nav.speed = controller.generalStats.chaseSpeed;
		controller.enemyAnimation.AbortPendingAim();
	}
}