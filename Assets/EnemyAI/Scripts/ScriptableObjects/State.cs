using UnityEngine;

namespace EnemyAI
{
	// 有限狀態機狀態可腳本化對象定義，包含動作和轉換。
	[CreateAssetMenu(menuName = "Enemy AI/State")]
	public class State : ScriptableObject
	{
		[Tooltip("處於該狀態時要執行的動作。")]
		public Action[] actions;
		[Tooltip("處於該狀態時要檢查的轉換。")]
		public Transition[] transitions;
		[Tooltip("狀態類別的顏色（藍色：清晰，黃色：警告，紅色：接觸）。")]
		public Color sceneGizmoColor = Color.grey;

		// 執行對應的狀態動作。
		public void DoActions(StateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].Act(controller);
			}
		}

		// 當狀態成為當前狀態時觸發狀態動作一次。
		public void OnEnableActions(StateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				// 在狀態被啟用時觸發所有動作一次。
				actions[i].OnEnableAction(controller);
			}
			for (int i = transitions.Length - 1; i >= 0; i--)
			{
				// 在狀態被啟用時觸發所有決策一次。
				transitions[i].decision.OnEnableDecision(controller);
			}
		}

		// 檢查對應的狀態轉換到其他有限狀態機狀態。
		public void CheckTransitions(StateController controller)
		{
			// 第一個決策優先於最後一個決策。
			for (int i = 0; i < transitions.Length; i++)
			{
				bool decision = transitions[i].decision.Decide(controller);
				if (decision)
				{
					// 轉到 true 狀態。
					controller.TransitionToState(transitions[i].trueState, transitions[i].decision);
				}
				else
				{
					// 轉到 false 狀態。
					controller.TransitionToState(transitions[i].falseState, transitions[i].decision);
				}
				// 如果轉換到另一個狀態，觸發新狀態的所有動作。
				if (controller.currentState != this)
				{
					controller.currentState.OnEnableActions(controller);
					// 無需檢查剩餘的轉換。
					break;
				}
			}
		}
	}
}