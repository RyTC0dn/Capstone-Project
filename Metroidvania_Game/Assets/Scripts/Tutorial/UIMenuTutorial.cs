using NUnit.Framework;
using TMPro;
using UnityEngine;

public class UIMenuTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public GameObject textBox;
    public GameObject[] arrows;

    public SceneInfo info;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBox.SetActive(false);
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method will be triggered at the start of the game in town scene, 
    // and will display the tutorial for navigating the UI, such as opening the inventory, equipping items, etc.
    public void Tutorial()
    {
        Time.timeScale = 0f; // Pause the game

        
    }
}
