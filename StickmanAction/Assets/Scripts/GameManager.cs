using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI killCountText;
    public GameObject clearPanel;

    private int killCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateKillCountUI();
    }

    public void AddKill()
    {
        killCount++;
        UpdateKillCountUI();
    }

    void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = "Kills: " + killCount;
        }
    }

    public void StageClear()
    {
        Debug.Log("스테이지 클리어!");
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}