using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{

    public static GameOverUIController Instance { get; private set; }

    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField]
    private Button mainMenuButton;

    public const string TargetScene = "MainMenu";
    

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.enabled = false;
        GameController.Instance.GameOverAction += OnGameOver;
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoToMainMenu() {
        SceneManager.LoadScene(TargetScene);
    }

    private void OnGameOver() {
        gameOverCanvas.enabled = true;
    }
}
