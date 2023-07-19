using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChooseEnemy : MonoBehaviour
{
    public string play1;
    public string play2;
    // Start is called before the first frame update
    public void ChooseEnemy1()
    {
        AI.whichEnemy = 1;
        SceneManager.LoadScene(play1);
    }

    public void ChooseEnemy2()
    {
        SceneManager.LoadScene(play2);
    }
}