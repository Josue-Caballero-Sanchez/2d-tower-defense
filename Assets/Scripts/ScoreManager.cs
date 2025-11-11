using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;
    private int startingScore = 100;

    private void Awake()
    {
        // Ensure a single instance of the ScoreManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        scoreText.text = $"Points: {score}";
    }

    public int GetScore()
    {
        return score;
    }
}
