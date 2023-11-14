using UnityEngine;
using UnityEngine.UI;
//這是一個在Unity中的腳本，屬於EnemyAI命名空間，用於管理生命值顯示的控制。該腳本確保生命值顯示始終會面向主攝像機，以便隨時更新玩家的生命狀態。
namespace EnemyAI
{
	// HealthBillboardManager將生命值HUD定位在物體的頂部，以始終面對主攝像機。
	public class HealthBillboardManager : MonoBehaviour
	{
		[Tooltip("物體被擊中後，生命值HUD顯示的持續時間（秒）。")]
		public float decayDuration = 2f;

		private Camera m_Camera;                    // 主攝像機的引用。
		private Image hud, bar;                     // 生命值HUD和血條的引用。
		private float decayTimer;                   // 當前衰變計時器。
		private Color originalColor, noAlphaColor;  // 生命值HUD的顏色引用。

		private void Start()
		{
			// 設置引用。
			hud = transform.Find("HUD").GetComponent<Image>();
			bar = transform.Find("Bar").GetComponent<Image>();
			m_Camera = Camera.main;
			originalColor = noAlphaColor = hud.color;
			noAlphaColor.a = 0f;

			// 開始時隱藏生命值HUD。
			gameObject.SetActive(false);
		}

		// 在這個幀中完成所有攝像機運動後，朝向生命值HUD，以避免抖動。
		void LateUpdate()
		{
			if (gameObject.activeSelf)
			{
				// 定向HUD。
				transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
					m_Camera.transform.rotation * Vector3.up);
				// 在HUD可見時更新衰變計時器。
				decayTimer += Time.deltaTime;
				if (decayTimer >= 0.5f * decayDuration)
				{
					float from = decayTimer - (0.5f * decayDuration);
					float to = 0.5f * decayDuration;
					// 將HUD顏色漸變為透明。
					hud.color = Color.Lerp(originalColor, noAlphaColor, from / to);
					bar.color = Color.Lerp(originalColor, noAlphaColor, from / to);
				}
				// 禁用HUD可見性。
				if (decayTimer >= decayDuration)
				{
					gameObject.SetActive(false);
				}
			}
		}

		// 設置生命值HUD為可見（由外部調用）。
		public void SetVisible()
		{
			gameObject.SetActive(true);
			decayTimer = 0;
			hud.color = bar.color = originalColor;
		}
	}
}