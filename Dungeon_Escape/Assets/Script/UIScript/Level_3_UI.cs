using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_3_UI : MonoBehaviour
{
    private float x; 
    void Update()
    {
        x += Time.deltaTime;

        if(x > 7f)
        {
        SceneManager.LoadScene("Level_4");
        }   
    }
}
