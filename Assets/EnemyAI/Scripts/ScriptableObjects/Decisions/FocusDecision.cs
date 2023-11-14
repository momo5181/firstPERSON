using UnityEngine;
using EnemyAI;

// The decision to focus on the target.
// 用於專注於目標的決策。
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Focus")]
public class FocusDecision : Decision
{
    [Tooltip("Which sense radius will be used?")]
    // 哪個感知半徑將被使用？
    public Sense sense;
    [Tooltip("Invalidate current cover when target is spotted?")]
    // 當目標被發現時，無效化當前掩護點？
    public bool invalidateCoverSpot;

    private float radius;          // The sense radius that will be used.
    // NPC Sense types.
    // NPC感知類型。
    public enum Sense
    {
        NEAR,
        PERCEPTION,
        VIEW
    }

    // The decision on enable function, triggered once after a FSM state transition.
    // 啟用決策函數，在FSM狀態轉換後觸發一次。
    public override void OnEnableDecision(StateController controller)
    {
        // Define sense radius.
        // 定義感知半徑。
        switch (sense)
        {
            case Sense.NEAR:
                radius = controller.nearRadius;
                break;
            case Sense.PERCEPTION:
                radius = controller.perceptionRadius;
                break;
            case Sense.VIEW:
                radius = controller.viewRadius;
                break;
            default:
                break;
        }
    }
    // The decide function, called on Update() (State controller - current state - transition - decision).
    // 決策函數，在Update()中被調用（狀態控制器 - 當前狀態 - 轉換 - 決策）。
    public override bool Decide(StateController controller) //override延伸定義
    {
        // If target is not near: felt a shot and sight to target is clear, can focus.
        // 如果目標不在附近：感覺到了一槍並且視線清晰，可以專注。
        // If target is near, always check sense for target.
        // 如果目標靠近，始終檢查目標感知。
        return (sense != Sense.NEAR && controller.variables.feelAlert && !controller.BlockedSight()) ||
            Decision.CheckTargetsInRadius(controller, radius, MyHandleTargets);
    }
    // The delegate for results of overlapping targets in focus decision.
    // 專注決策中重疊目標的結果委派。
    private bool MyHandleTargets(StateController controller, bool hasTargets, Collider[] targetsInHearRadius)
    {
        // Is there any target, with a clear sight to it?
        // 是否有任何目標，並且對其有清晰的視線？
        if (hasTargets && !controller.BlockedSight())
        {
            // Invalidade current cover spot (ex.: used to move from position when spotted).
            // 無效化當前掩護點（例如：在發現時用於移動位置）。
            if (invalidateCoverSpot)
                controller.CoverSpot = Vector3.positiveInfinity;
            // Set current target parameters.
            // 設置當前目標參數。
            controller.targetInSight = true;
            controller.personalTarget = controller.aimTarget.position;
            return true;
        }
        // No target on sight.
        // 沒有目標在視線上。
        return false;
    }
}