using UnityEngine;

namespace EnemyAI
{
	// NPC常规统计信息。
	[CreateAssetMenu(menuName = "Enemy AI/General Stats")]
	public class GeneralStats : ScriptableObject
	{
		[Header("General")]
		[Tooltip("NPC巡逻速度（清晰状态）。")]
		public float patrolSpeed = 2f;
		[Tooltip("NPC搜索速度（警告状态）。")]
		public float chaseSpeed = 5f;
		[Tooltip("NPC躲避速度（交战状态）。")]
		public float evadeSpeed = 15f;
		[Tooltip("在路径点上等待的时间。")]
		public float patrolWaitTime = 2f;
		[Tooltip("障碍层掩码。")]
		public LayerMask obstacleMask;
		[Header("Animation")]
		[Tooltip("避免瞄准抖动的清除角度（死区）。")]
		public float angleDeadzone = 5f;
		[Tooltip("速度参数的阻尼时间。")]
		public float speedDampTime = 0.4f;
		[Tooltip("angularSpeed参数的阻尼时间。")]
		public float angularSpeedDampTime = 0.2f;
		[Tooltip("将角度转换为angularSpeed的响应时间。")]
		public float angleResponseTime = 0.2f;
		[Header("Cover")]
		[Tooltip("考虑蹲伏的低掩体高度。")]
		public float aboveCoverHeight = 1.5f;
		[Tooltip("掩体层掩码。")]
		public LayerMask coverMask;
		[Header("Shoot")]
		[Tooltip("用于发射射击的层掩码。")]
		public LayerMask shotMask;
		[Tooltip("目标的层掩码。")]
		public LayerMask targetMask;
	}
}