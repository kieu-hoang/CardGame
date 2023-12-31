using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string play;

    public string deck;

    public string collection;

    public string tutorial;

    public string menu;

    public string shop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlay()
    {
        SceneManager.LoadScene(play);
    }
    public void LoadDeck()
    {
        SceneManager.LoadScene(deck);
    }
    public void LoadCollection()
    {
        SceneManager.LoadScene(collection);
    }

    public void LoadShop()
    {
        SceneManager.LoadScene(shop);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(tutorial);
    }    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menu);
    }
}
