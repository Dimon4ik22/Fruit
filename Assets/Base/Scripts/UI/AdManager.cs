using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAd();

    public GameObject adPanel; // Панель рекламы
    public TMP_Text timerText; // Текст таймера

    private void Start()
    {
        StartCoroutine(ShowAdRoutine());
    }

    private IEnumerator ShowAdRoutine()
    {
        while (true)
        {
            // Ожидание 2 минут - 3 секунды (120 секунд - 3 секунды = 117 секунд)
            yield return new WaitForSeconds(177);

            // Активация таймера и отсчет 3 секунд
            adPanel.SetActive(true);
            timerText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                timerText.text = $"Реклама начнется через {i}...";
                yield return new WaitForSeconds(1);
            }
            // Показ рекламы
            timerText.gameObject.SetActive(false);
            adPanel.SetActive(true);

            ShowAd();

            // Демонстрация рекламы в течение некоторого времени, здесь 5 секунд
            yield return new WaitForSeconds(2);

            // Скрытие рекламы и переход к следующему циклу
            adPanel.SetActive(false);
        }
    }
}
