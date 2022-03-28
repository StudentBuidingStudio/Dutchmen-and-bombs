using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer mainMixer;
    public float audioSourse;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        mainMixer.SetFloat("MainVolume", 0);
    }

    // Update is called once per frame
    
    //°´¼ü¼ì²â
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Turn();
        }
    }

    //ÔÝÍ£
    public void  Turn()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            mainMixer.SetFloat("MainVolume", audioSourse);
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            mainMixer.SetFloat("MainVolume", -80);
        }

    }

    public void SetVolume(float value)
    {
        audioSourse = value;
    }
}
