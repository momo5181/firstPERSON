using System.Collections;
using UnityEngine;
using EnemyAI;

// 寻找掩体位置的动作。
[CreateAssetMenu(menuName = "Enemy AI/Actions/Find Cover")]
public class FindCoverAction : Action
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
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool("Crouch", false);
		// 获取最佳掩体位置，考虑当前NPC和目标位置。
		ArrayList nextCoverData = controller.coverLookup.GetBestCoverSpot(controller);
		Vector3 potentialCover = (Vector3)nextCoverData[1];
		// 没有掩体位置。
		if (Vector3.Equals(potentialCover, Vector3.positiveInfinity))
		{
			controller.nav.destination = controller.transform.position;
			return;
		}
		// 更近的掩体位置，更新位置。
		else if ((controller.personalTarget - potentialCover).sqrMagnitude < (controller.personalTarget - controller.CoverSpot).sqrMagnitude
			&& !controller.IsNearOtherSpot(potentialCover, controller.nearRadius))
		{
			controller.coverHash = (int)nextCoverData[0];
			controller.CoverSpot = potentialCover;
		}
		// 设置导航参数。
		controller.nav.destination = controller.CoverSpot;
		controller.nav.speed = controller.generalStats.evadeSpeed;
		// 完成当前回合射击。
		controller.variables.currentShots = controller.variables.shotsInRound;
	}
}