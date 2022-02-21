using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //加载条
    public GameObject loadScreen;
    public Text loadPercentage;
    public Slider loadSlider;

    public AsyncOperation aOper;

    //UI动画
    public Animator uiAnim;

    

    //开始动画
    void Start()
    {
        loadScreen.SetActive(false);
    }

    //开始游戏
    public void StartGame()
    {
        StartCoroutine(LoadLevel());
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }

    //按键动画
    public void UIEnable()
    {
        uiAnim.SetBool("Start", true);
    }

    

    //异步加载
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

    

}
