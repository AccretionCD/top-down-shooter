using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Image waveBanner;
    [SerializeField] Text waveText;

    PlayerController player;
    SpawnHandler spawnHandler;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        spawnHandler = FindObjectOfType<SpawnHandler>();
    }

    void Start()
    {
        player.OnDeath += OnPlayerDeath;
        spawnHandler.OnWaveStart += OnWaveStart;
    }

    void OnWaveStart(int waveNumber)
    {
        waveText.text = "Wave " + waveNumber;

        StartCoroutine(FadeWaveBanner());
    }

    void OnPlayerDeath()
    {
        StartCoroutine(FadeScreen());
        gameOverScreen.SetActive(true);
        Cursor.visible = true;
    }

    IEnumerator FadeWaveBanner()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            waveBanner.color = Color.Lerp(Color.clear, Color.black, percent);
            waveText.color = Color.Lerp(Color.clear, Color.white, percent);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            waveBanner.color = Color.Lerp(Color.black, Color.clear, percent);
            waveText.color = Color.Lerp(Color.white, Color.clear, percent);

            yield return null;
        }
    }

    IEnumerator FadeScreen()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;
            fadeScreen.color = Color.Lerp(Color.clear, Color.black, percent);

            yield return null;
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
        Cursor.visible = false;
    }
}
