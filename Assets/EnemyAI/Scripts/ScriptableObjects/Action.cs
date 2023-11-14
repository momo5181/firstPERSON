using UnityEngine;

namespace EnemyAI
{
	// 用于FSM动作的模板脚本对象。
	// 任何自定义FSM动作都必须继承自此类。
	public abstract class Action : ScriptableObject
	{
		// Act函数，在Update()中调用（State controller - current state - action）。
		public abstract void Act(StateController controller);
		// 启用行动时的功能，在FSM状态转换后触发一次。
		public virtual void OnEnableAction(StateController controller) { }
	}
}