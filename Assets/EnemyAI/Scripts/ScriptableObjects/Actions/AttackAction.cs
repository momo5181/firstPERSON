using System.Collections;
using UnityEngine;
using EnemyAI;

// NPC攻击行动。
[CreateAssetMenu(menuName = "Enemy AI/Actions/Attack")]
public class AttackAction : Action
{
	private readonly float startShootDelay = 0.2f; // 开始射击前的延迟。
	private readonly float aimAngleGap = 30f;      // 当前和期望瞄准方向之间的最小角度差。

	// Act函数，在Update()中调用（State controller - current state - action）。
	public override void Act(StateController controller)
	{
		// 始终关注视线位置。
		controller.focusSight = true;

		if (CanShoot(controller))
		{
			Shoot(controller);
		}
		// 累积盲目参与计时器。
		controller.variables.blindEngageTimer += Time.deltaTime;
	}
	// NPC是否能射击？
	private bool CanShoot(StateController controller)
	{
		// NPC正在瞄准并且与期望位置几乎对齐？
		if (controller.Aiming && 
			(controller.enemyAnimation.currentAimAngleGap < aimAngleGap ||
			// 或者如果目标太近，无论如何都射击
			(controller.personalTarget - controller.enemyAnimation.gunMuzzle.position).sqrMagnitude <= 0.25f))
		{
			// 所有条件都匹配，检查开始延迟。
			if (controller.variables.startShootTimer >= startShootDelay)
			{
				return true;
			}
			else
			{
				controller.variables.startShootTimer += Time.deltaTime;
			}
		}
		return false;
	}
	// 启用行动时的功能，在FSM状态转换后触发一次。
	public override void OnEnableAction(StateController controller)
	{
		// 为行动设置初始值。
		controller.variables.shotsInRound = Random.Range(controller.maximumBurst / 2, controller.maximumBurst);
		controller.variables.currentShots = 0;
		controller.variables.startShootTimer = 0f;
		controller.enemyAnimation.anim.ResetTrigger("Shooting");
		controller.enemyAnimation.anim.SetBool("Crouch", false);
		controller.variables.waitInCoverTimer = 0;
		controller.enemyAnimation.ActivatePendingAim();
	}
	// 执行射击动作。
	private void Shoot(StateController controller)
	{
		// 检查射击间隔。
		if (Time.timeScale > 0 && controller.variables.shotTimer == 0f)
		{
			controller.enemyAnimation.anim.SetTrigger("Shooting");
			CastShot(controller);
		}
		// 更新与射击相关的变量并启用下一发射击。
		else if(controller.variables.shotTimer >= (0.1f + 2 * Time.deltaTime))
		{
			controller.bullets = Mathf.Max(--controller.bullets, 0);
			controller.variables.currentShots++;
			controller.variables.shotTimer = 0f;
			return;
		}
		controller.variables.shotTimer += controller.classStats.shotRateFactor * Time.deltaTime;
	}
	// 发射子弹。
	private void CastShot(StateController controller)
	{
		// 获取射击不准确向量。
		Vector3 imprecision = Random.Range(-controller.classStats.shotErrorRate, controller.classStats.shotErrorRate)
			* controller.transform.right;

		imprecision += Random.Range(-controller.classStats.shotErrorRate, controller.classStats.shotErrorRate)
			* controller.transform.up;
		// 获取射击期望方向。
		Vector3 shotDirection = controller.personalTarget - controller.enemyAnimation.gunMuzzle.position;
		// 发射子弹。
		Ray ray = new Ray(controller.enemyAnimation.gunMuzzle.position, shotDirection.normalized + imprecision);
		if (Physics.Raycast(ray, out RaycastHit hit, controller.viewRadius, controller.generalStats.shotMask.value))
		{
			// 击中某物体？将目标掩码中的所有图层都视为有机物。
			bool isOrganic = ((1 << hit.transform.root.gameObject.layer) & controller.generalStats.targetMask) != 0;
			DoShot(controller, ray.direction, hit.point, hit.normal, isOrganic, hit.transform);
		}
		else
		{
			// 未命中任何目标（射击未命中），在带有不准确性的期望方向进行射击。
			DoShot(controller, ray.direction, ray.origin + (ray.direction * 500f));
		}
	}
	// 绘制射击和额外资源。
	private void DoShot(StateController controller, Vector3 direction, Vector3 hitPoint,
		Vector3 hitNormal = default, bool organic = false, Transform target = null)
	{
		// 绘制枪口火焰。
		GameObject muzzleFlash = Object.Instantiate<GameObject>(controller.classStats.muzzleFlash, controller.enemyAnimation.gunMuzzle);
		muzzleFlash.transform.localPosition = Vector3.zero;
		muzzleFlash.transform.localEulerAngles = Vector3.back * 90f;
		controller.StartCoroutine(this.DestroyFlash(muzzleFlash));

		// 绘制射击示踪器和烟雾。
		GameObject shotTracer = Object.Instantiate<GameObject>(controller.classStats.shot, controller.enemyAnimation.gunMuzzle);
		// 起始射击示踪器填充
		Vector3 origin = controller.enemyAnimation.gunMuzzle.position - controller.enemyAnimation.gunMuzzle.right * 0.5f;
		shotTracer.transform.position = origin;
		shotTracer.transform.rotation = Quaternion.LookRotation(direction);

		// 绘制子弹孔和火花，如果目标不是有机物。
		if(target && !organic)
		{
			GameObject bulletHole = Instantiate(controller.classStats.bulletHole);
			bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
			bulletHole.transform.position = hitPoint + 0.01f * hitNormal;
			GameObject instantSparks = Object.Instantiate<GameObject>(controller.classStats.sparks);
			instantSparks.transform.position = hitPoint;
		}
		// 被击中的物体是有机物，调用受伤函数。
		else if(target && organic)
		{
			HealthManager targetHealth = target.GetComponent<HealthManager>();
			if(targetHealth)
			{
				targetHealth.TakeDamage(hitPoint, direction, controller.classStats.bulletDamage, target.GetComponent<Collider>(), controller.gameObject);
			}
		}
		// 在射击位置播放射击音频剪辑。
		AudioSource.PlayClipAtPoint(controller.classStats.shotSound, controller.enemyAnimation.gunMuzzle.position, 2f);
	}
	// 销毁枪口火焰的函数。
	public IEnumerator DestroyFlash(GameObject flash)
	{
		yield return new WaitForSeconds(0.1f);
		Destroy(flash);
	}
}