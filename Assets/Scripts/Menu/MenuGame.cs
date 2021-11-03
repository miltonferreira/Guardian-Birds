using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour
{

    public GameObject credits;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NextScene(string nextScene){
        SceneManager.LoadScene(nextScene);
    }

    public void CloseGame(){
        Application.Quit();
    }

    public void showCredit(){
        credits.SetActive(true);
    }

    public void hideCredit(){
        credits.SetActive(false);
    }
}
