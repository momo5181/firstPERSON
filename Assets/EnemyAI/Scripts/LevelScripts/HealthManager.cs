using UnityEngine;

// 這是一個遊戲中物件健康管理器的範本腳本。
// 任何對射擊作出反應的遊戲實體都必須具有此腳本並包含公開函式 TakeDamage()。
public class HealthManager : MonoBehaviour
{
	// 用於封裝回調函式的損傷參數的類別。
	public class DamageInfo
	{
		public Vector3 location, direction;      // 擊中位置和方向。
		public float damage;                     // 傷害量。
		public Collider bodyPart;                // 被擊中的身體部位（可選）。
		public GameObject origin;                // 生成擊中的遊戲物件（可選）。

		public DamageInfo(Vector3 location, Vector3 direction, float damage, Collider bodyPart=null, GameObject origin=null)
		{
			this.location = location;
			this.direction = direction;
			this.damage = damage;
			this.bodyPart = bodyPart;
			this.origin = origin;
		}
	}

	[HideInInspector] public bool dead;          // 這個實體是否死亡？

	// 這是從射擊接收傷害的必要函式。
	// 在編寫內容之前，可以刪除 'virtual' 關鍵字。
	public virtual void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart=null, GameObject origin=null)
	{
	}

	// 這是接收由子遊戲物件剛體（例如布娃娃）承受的傷害的消息接收器。
	public void HitCallback(DamageInfo damageInfo)
	{
		this.TakeDamage(damageInfo.location, damageInfo.direction, damageInfo.damage, damageInfo.bodyPart, damageInfo.origin);
	}
}