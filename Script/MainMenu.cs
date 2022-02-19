using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //������
    public GameObject loadScreen;
    public Text loadPercentage;
    public Slider loadSlider;

    public AsyncOperation aOper;

    //UI����
    public Animator uiAnim;

    //��ͣ�˵�
    public GameObject pauseMenu;
    

    void Start()
    {
        loadScreen.SetActive(false);
    }

    
    public void StartGame()
    {
        StartCoroutine(LoadLevel());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UIEnable()
    {
        uiAnim.SetBool("Start", true);
    }

    //�첽����
    IEnumerator LoadLevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        operation.allowSceneActivation = false;
        loadScreen.SetActive(true);

        while (!operation.isDone)
        {
            loadSlider.value = operation.progress;

            loadPercentage.text = (loadSlider.value * 100) + "%";

            

            if (operation.progress >= 0.9f)
            {
                loadSlider.value = 1;
                loadPercentage.text = "competed";
                aOper = operation;
                Invoke("LoadCompeted", 1f);
                
            }
            yield return null;
        }
            
    }


    public void LoadCompeted()
    {
        aOper.allowSceneActivation = true;
    }

    //��ͣ
    public void PauseMenu()
    {

    }

}
