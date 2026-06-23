using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    [Header("Level")]
    public int level = 1;

    [Header("Experience")]
    public int currentExp = 0;
    public int requiredExp = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
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

            requiredExp += 10;

            Debug.Log($"Level up! Current level: {level}");
        }
    }
}
