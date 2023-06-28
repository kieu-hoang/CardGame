using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChooseEnemy : MonoBehaviour
{
    public string play;
    // Start is called before the first frame update
    public void ChooseEnemy1()
    {
        AI.whichEnemy = 1;
        SceneManager.LoadScene(play);
    }

    public void ChooseEnemy2()
    {
        AI.whichEnemy = 2;
        SceneManager.LoadScene(play);
    }
}
