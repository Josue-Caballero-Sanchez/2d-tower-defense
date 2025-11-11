using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int playerLives = 0;
    private int startingLives = 10;
    [SerializeField] private TextMeshProUGUI livesText;

    public void Start()
    {
        UpdateLives(startingLives);
    }

    private void Awake()
    {
        // Ensure a single instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLives(int amount)
    {
        playerLives += amount;
        livesText.text = $"Lives: {playerLives}";
        if (playerLives <= 0)
        {
            Debug.Log("Game Over!");
        }
    }
}
