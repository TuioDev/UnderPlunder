using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehaviour : MonoBehaviour
{
    [Header("All main objetcs of the menu")]
    [SerializeField] private List<GameObject> AllObjects;
    [Header("Every main object of the menu")]
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Play;
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject Credits;

    private void Start()
    {
        GoToMenu();
    }
    public void GoToMenu()
    {
        SetVisual(Menu);
    }
    public void GoToPlay()
    {
        SetVisual(Play);
    }
    public void GoToOptions()
    {
        SetVisual(Options);
    }
    public void GoToCredits()
    {
        SetVisual(Credits);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    private void SetVisual(GameObject gameObject)
    {
        foreach (GameObject go in AllObjects)
        {
            if(go.Equals(gameObject))
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }
}
