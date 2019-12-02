using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    [SerializeField] GameObject gameOverScreen;

    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.OnDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        StartCoroutine(FadeScreen(Color.clear, Color.black, 1));
        gameOverScreen.SetActive(true);
    }

    IEnumerator FadeScreen(Color original, Color target, float duration)
    {
        float rate = 1 / duration;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * rate;
            fadeScreen.color = Color.Lerp(original, target, percent);

            yield return null;
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
}
