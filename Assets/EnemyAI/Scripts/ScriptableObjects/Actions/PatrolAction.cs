using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

// NPC巡逻动作。
[CreateAssetMenu(menuName = "Enemy AI/Actions/Patrol")]
public class PatrolAction : Action
{
	// Act函数，在Update()中调用（State controller - current state - action）。
	public override void Act(StateController controller)
	{
		Patrol(controller);
	}
	// 启用行动时的功能，在FSM状态转换后触发一次。
	public override void OnEnableAction(StateController controller)
	{
		// 为动作设置初始值。
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool("Crouch", false);
		controller.personalTarget = Vector3.positiveInfinity;
		controller.CoverSpot = Vector3.positiveInfinity;
		controller.focusSight = false;
	}
	// NPC巡逻功能。
	private void Patrol(StateController controller)
	{
		// 没有巡逻路径点，保持闲置状态。
		if (controller.patrolWayPoints.Count == 0)
			return;
		// 设置导航参数。
		controller.focusSight = false;
		controller.nav.speed = controller.generalStats.patrolSpeed;
		// 到达路径点，等待一段时间后继续巡逻。
		if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending)
		{
			controller.variables.patrolTimer += Time.deltaTime;

			if (controller.variables.patrolTimer >= controller.generalStats.patrolWaitTime)
			{
				controller.waypointIndex = (controller.waypointIndex + 1) % controller.patrolWayPoints.Count;
				controller.variables.patrolTimer = 0f;
			}
		}
		// 设置下一个巡逻路径点。
		try
		{
			controller.nav.destination = controller.patrolWayPoints[controller.waypointIndex].position;
		}
		catch (UnassignedReferenceException)
		{
			// 为NPC建议巡逻路径点（如果没有）。
			Debug.LogWarning("No waypoints assigned for " + controller.transform.name + ", enemy will remain idle");
			// 没有路径点，创建单一位置以保持静止。
			controller.patrolWayPoints = new List<Transform>
			{
				controller.transform
			};
			controller.nav.destination = controller.transform.position;
		}
	}
}