using TMPro;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MerchantHeroTower : Tower
{
    private float generateIncomeDelay = 5f;
    private float generateIncomeTimer = 0;
    private int generatedIncome = 10;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private MMFeedbacks generateFeedback;
    protected override void Update()
    {
        if (isLevelTwoAnimationActive)
        {
            LevelTwoAnimation();
        }

        if (WaveManager.Instance.GetIsSpawningWave() && !isSold)
        {
            if (generateIncomeTimer == 0)
            {
                GenerateIncome();
            }

            generateIncomeTimer += Time.deltaTime;

            if (generateIncomeTimer >= generateIncomeDelay)
            {
                generateIncomeTimer = 0;
            }
        }

        if (!WaveManager.Instance.GetIsSpawningWave())
        {
            generateIncomeTimer = 0;
        }
    }

    private void GenerateIncome()
    {
        animator.SetBool("isShooting", true);
        generateFeedback.PlayFeedbacks();

        Vector3 textSpawnPosition = transform.position;
        textSpawnPosition.x += 0.5f;
        textSpawnPosition.y += 0.5f;
        GameObject textPrefab = Instantiate(floatingTextPrefab, textSpawnPosition, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMeshPro>().text = "+$" + generatedIncome;
        textPrefab.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
        ScoreManager.Instance.UpdateScore(10);
    }

    protected override void Upgrade1()
    {

    }

    protected override void Upgrade2()
    {

    }
    protected override void Upgrade3()
    {

    }

    protected override void Upgrade4()
    {

    }
}