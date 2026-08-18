using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    [Header("Core Stats")]
    public int level = 1;
    public int maxHP = 100;
    public int currentHP = 100;
    public int maxMP = 50;
    public int currentMP = 50;
    public int currentExp = 0;
    public int requiredExp = 20;

    public int minDamage = 5;
    public int maxDamage = 10;

    public int attack = 5;
    public int defense = 5;

    [Header("RPG Attributes")]
    public int str = 10; 
    public int agi = 10; 
    public int wis = 10; 
    public int luk = 10; 

    [Header("Advanced Stats")]
    public float critChance = 5.0f; // 5% crit
    public float critDamage = 1.5f; // x1.5 dmg
    public int accuracy = 20; 
    public float movementSpeed = 100f; 

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(0, damage - defense);

        currentHP -= actualDamage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }
        
        Debug.Log($"Took {actualDamage} damage. Current HP: {currentHP}");

        StartCoroutine(FlashRed());

        if (currentHP <= 0)
        {
            Debug.Log("Player has died.");

            Destroy(gameObject);
        }
    }

    public void GainExp(int amount)
    {
        currentExp += amount;

        Debug.Log($"Gained {amount} EXP");

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;

            level++;

            attack += 2;
            minDamage += 2;
            maxDamage += 2;

            maxHP += 5;

            currentHP = maxHP;

            requiredExp += 10;

            Debug.Log($"Level up! Current level: {level}");
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        spriteRenderer.color = Color.white;
    }
}
