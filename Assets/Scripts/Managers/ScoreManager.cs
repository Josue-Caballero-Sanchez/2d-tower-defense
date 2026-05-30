using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;
    private int startingScore = 10000;

    private void Awake()
    {
        // Ensure a single instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScore(startingScore);
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "$" + score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
}
