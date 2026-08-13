using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    public void TriggerAttackDamage()
    {
        if (playerAttack != null)
        {
            playerAttack.DealDamageHit();
        }
    }
}
