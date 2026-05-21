using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int playerLives = 0;
    private int startingLives = 10;
    private bool isFastForwarding = false;
    [SerializeField] private TextMeshProUGUI livesText;

    public void Start()
    {
        UpdateLives(startingLives);
    }

    private void Awake()
    {
        // Ensure a single instance
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

    private void Update()
    {
        ManageSpeed();
    }

    public void UpdateLives(int amount)
    {
        playerLives += amount;
        livesText.text = playerLives.ToString();
        if (playerLives <= 0)
        {
            Debug.Log("Game Over!");
        }
    }

    public void UpdateIsFastForwarding(bool value)
    {
        isFastForwarding = value;
    }

    public bool GetIsFastForwarding()
    {
        return isFastForwarding;
    }


    private void ManageSpeed()
    {
        if (WaveManager.Instance.GetIsSpawningWave())
        {
            if (isFastForwarding)
            {
                Time.timeScale = 1.5f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else if (!WaveManager.Instance.GetIsSpawningWave())
        {
            Time.timeScale = 1f;
        }
    }
}
