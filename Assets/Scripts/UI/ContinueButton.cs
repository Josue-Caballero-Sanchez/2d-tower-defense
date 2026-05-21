using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ContinueButton : MonoBehaviour
{
    private TextMeshProUGUI buttonText;
    private Image image;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private GameObject fastForwardIcon;
    [SerializeField] private Sprite graySprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite fastForwardSprite;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { OnButtonClick(); });
    }

    private void Update()
    {
        ChangeButtonAppearance();
    }

    private void OnButtonClick()
    {
        if (WaveManager.Instance.GetIsSpawningWave())
        {
            GameManager.Instance.UpdateIsFastForwarding(!GameManager.Instance.GetIsFastForwarding());
        }
        else
        {
            if (WaveManager.Instance.GetCurrentWave() != 1)
            {
                WaveManager.Instance.StartNewWave();
            }
            else
            {
                WaveManager.Instance.StartFirstWave();
            }
        }
    }

    private void ChangeButtonAppearance()
    {
        if (WaveManager.Instance.GetIsSpawningWave())
        {
            if (GameManager.Instance.GetIsFastForwarding())
            {
                buttonText.text = "1.5x";
                image.sprite = fastForwardSprite;
            }
            else
            {
                buttonText.text = "1x";
                image.sprite = graySprite;
            }
            continueIcon.SetActive(false);
            fastForwardIcon.SetActive(true);
        }
        else
        {
            buttonText.text = "Start Wave";
            continueIcon.SetActive(true);
            fastForwardIcon.SetActive(false);
            image.sprite = normalSprite;
        }
    }
}
