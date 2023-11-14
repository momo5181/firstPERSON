using UnityEngine;

// 這個類處理事件傳播（例如：射擊噪音），通知附近的物件。
public class AlertManagement : MonoBehaviour
{
	[Tooltip("警報傳播的半徑。")]
	[Range(0, 50)] public float alertRadius;
	[Tooltip("存活時間。警報傳播的額外層級數。")]
	public int extraWaves;
	[Tooltip("要警報的物件的圖層遮罩。")]
	public LayerMask alertMask = 1 << 12;

	private Vector3 current;       // 目前的警報位置。
	private bool alert;            // 是否有新的警報要傳播？

	void Start()
	{
		// 周期性地 ping 當前警報（如果有的話）。預設週期：1 秒。
		InvokeRepeating("PingAlert", 1, 1);
	}

	// 警報附近的物件發生事件。
	private void AlertNearby(Vector3 origin, Vector3 target, int wave = 0)
	{
		// 這個物件會保留或轉發警報嗎？
		// 如果 TTL（存活時間）大於定義值，則不轉發警報。
		if (wave > this.extraWaves)
			return;

		// 獲取附近的物件以觸發警報。
		Collider[] targetsInViewRadius = Physics.OverlapSphere(origin, alertRadius, alertMask);

		foreach (Collider obj in targetsInViewRadius)
		{
			// 調用物件的回調函式以接收警報。
			obj.SendMessageUpwards("AlertCallback", target, SendMessageOptions.DontRequireReceiver);

			// 將警報轉發到附近的物件
			AlertNearby(obj.transform.position, target, wave + 1);
		}
	}

	// 根警報回調，設置當前警報的來源（從外部調用）。
	public void RootAlertNearby(Vector3 origin)
	{
		current = origin;
		alert = true;
	}

	// 周期性地 ping 警報（如果有的話）。
	void PingAlert()
	{
		if (alert)
		{
			alert = false;
			AlertNearby(current, current);
		}
	}
}