using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceenHandler : MonoBehaviour {

    public GameObject[] slids;

    private float timePrSlide = 4f;
    private int currentSlideIndex = -1;
    private float lastSlideTime;

	// Use this for initialization
	void Start () {
        if (SoundHandler.Instance != null)
            SoundHandler.Instance.PlayIntroSceneMusic();

        lastSlideTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextLevelIndex);
            return;
        }

        if(Time.time - lastSlideTime >= timePrSlide)
        {
            var areMoreSlidsToShow = currentSlideIndex + 1 < slids.Length;
            if (areMoreSlidsToShow)
            {
                ShowNextSlide();
            }
            else
            {
                var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextLevelIndex);
            }
        }

        
	}
    void ShowNextSlide()
    {
        //Hide current slide
        if(currentSlideIndex > -1)
        {
            var currentSlide = slids[currentSlideIndex];
            currentSlide.gameObject.SetActive(false);
        }

        if(currentSlideIndex + 1 < slids.Length)
        {
            currentSlideIndex++;
            var slide = slids[currentSlideIndex];
            slide.gameObject.SetActive(true);

            lastSlideTime = Time.time;
        }
    }
}
