using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChooseEnemy : MonoBehaviour
{
    public string play1;
    public string play1b;
    public string play1c;
    public string play2;
    // Start is called before the first frame update
    public void ChooseEnemy1()
    {
        AI.whichEnemy = 1;
        SceneManager.LoadScene(play1);
    }
    
    public void ChooseEnemy1b()
    {
        AI.whichEnemy = 1;
        SceneManager.LoadScene(play1b);
    }
    
    public void ChooseEnemy1c()
    {
        AI.whichEnemy = 1;
        SceneManager.LoadScene(play1c);
    }

    public void ChooseEnemy2()
    {
        SceneManager.LoadScene(play2);
    }
}
