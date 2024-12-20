using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour // Major Modification implemented
{
    public float timeStart;  
    private float timeLeft;   
    public TextMeshProUGUI timerText;  
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    private float m_Timer;  
    public float fadeDuration = 1f;  
    public float displayImageDuration = 1f;
    bool m_HasAudioPlayed;

    void Start()
    {
        timeLeft = timeStart;
    }

    void Update()
    {
        
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;  
        }
        else
        {
            timeLeft = 0;  
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);  
        }

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;  
        imageCanvasGroup.alpha = m_Timer / fadeDuration;  

      
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);  
            }
            else
            {
                Application.Quit();  
            }
        }
    }
}
