using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;//設定血量
    public void TakeDamage (float amount)
    {
        health -=amount;
        if(health <= 0f) 
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);//銷毀物體
    }
}
