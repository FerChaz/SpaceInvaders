using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemies;
    [SerializeField] private GameObject Shields;
    [SerializeField] private GameObject PauseMenu;
    private bool gamePaused = false;
    private bool started = false;

    private PlayerState playerState;
    private EnemyController enemyController;

    private void Awake() {
        playerState = Player.GetComponent<PlayerState>();
        enemyController = Enemies.GetComponent<EnemyController>();
    }

    private void Start() {
        if(Player != null)
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerState.Restart();
        enemyController.Restart();
        ShieldController[] shieldcontrollers = Shields.GetComponentsInChildren<ShieldController>(true);
        for (int i = 0; i < shieldcontrollers.Length; i++)
        {
            shieldcontrollers[i].ActivateShield();
        }
        StartCoroutine(CooldownToStart());
        
    }

    IEnumerator CooldownToStart()
    {
        yield return new WaitForSeconds(3);
        Player.GetComponent<CombatController>().isAlive = true;
        playerState.PlayerStatusUpdate();
        enemyController.GameStatusUpdate();
        started = true;
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {
        if (Input.GetButtonDown("Pause") && started)
        {
            playerState.PlayerStatusUpdate();
            enemyController.GameStatusUpdate();

            if (gamePaused) 
            {
                gamePaused = false;
                PauseMenu.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            } else {
                gamePaused = true;
                PauseMenu.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
        }

    }

    public void PauseCanvas()
    {
            playerState.PlayerStatusUpdate();
            enemyController.GameStatusUpdate();

            if (gamePaused) 
            {
                gamePaused = false;
                PauseMenu.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            } else {
                gamePaused = true;
                PauseMenu.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
        

    }

}
