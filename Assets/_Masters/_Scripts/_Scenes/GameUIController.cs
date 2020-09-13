using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance { get; private set; }

    [SerializeField] private GameObject m_LayerPause;
    [SerializeField] private Image m_ImageProgress;
    [SerializeField] private Text m_TextProgress;

    private void Awake()
    {
        Instance = this;
        m_ImageProgress.fillAmount = 0f;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_LayerPause.SetActive(false);
    }

    public void updateProgress(float ratio)
    {
        m_ImageProgress.fillAmount = ratio;
        m_TextProgress.text = (ratio * 100f).ToString("F0") + "%";
    }

    public void onPauseGame()
    {
        Time.timeScale = 0f;
        m_LayerPause.SetActive(true);
    }

    public void onBackGame()
    {
        Time.timeScale = 1f;
        m_LayerPause.SetActive(false);
    }
}
