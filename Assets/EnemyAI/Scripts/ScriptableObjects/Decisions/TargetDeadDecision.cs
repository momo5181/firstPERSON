using UnityEngine;
using EnemyAI;
//用於敵人AI的決策腳本。這個腳本用於檢查目標是否已經死亡
// 檢查目標死亡
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Target Dead")]
public class TargetDeadDecision : Decision
{  
    // 這是決策函數，會在Update()中被調用（狀態控制器 - 當前狀態 - 轉換 - 決策）。
    public override bool Decide(StateController controller)
    {
        try
        {      
            // 檢查目標的健康管理器是否已死亡。
            return controller.aimTarget.root.GetComponent<HealthManager>().dead;
        }
        catch (UnassignedReferenceException)
        {
            // Ensure the target has a health manager set.
            // 確保目標已經設置了一個健康管理器。
            Debug.LogError("Assign a health manager to" + controller.name);
        }
        return false;
    }
}