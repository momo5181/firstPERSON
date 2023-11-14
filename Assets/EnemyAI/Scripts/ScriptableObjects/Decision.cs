using UnityEngine;

namespace EnemyAI
{
	// 用於FSM決策的模板腳本對象。
	// 任何自定義的FSM決策都必須繼承自這個類。
	public abstract class Decision : ScriptableObject
	{
		// 決策功能，於Update()時調用（狀態控制器-當前狀態-過渡-決策）。
		public abstract bool Decide(StateController controller);
		// 啟用決策時的功能，在FSM狀態過渡後觸發一次。
		public virtual void OnEnableDecision(StateController controller) { }
		// 用於感知決策的常見重疊功能（看、聽、接近等）不會改變的宣告。
		public static bool CheckTargetsInRadius(StateController controller, float radius, HandeTargets handleTargets)
		{
			// 目標死亡，忽略感知觸發。
			if (controller.aimTarget.root.GetComponent<HealthManager>().dead)
				return false;
			// 目標存活。
			else
			{
				Collider[] targetsInRadius =
					Physics.OverlapSphere(controller.transform.position, radius, controller.generalStats.targetMask);

				return handleTargets(controller, targetsInRadius.Length > 0, targetsInRadius);
			}
		}
		// 用於感知決策中重疊目標的結果的委派。
		public delegate bool HandeTargets(StateController controller, bool hasTargets, Collider[] targetsInRadius);
	}
}