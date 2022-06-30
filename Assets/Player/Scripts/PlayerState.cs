using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool _isAlive;
    
    [SerializeField] private int _lives;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject[] livesIMG;
    
    public bool isAlive
    {
        get
        {
            return _isAlive;
        }
    }
    public int lives
    {
        get
        {
            return _lives;
        }
    }

    private PlayerController playerController;

    private void Awake() 
    {
        _isAlive = false;
        playerController = gameObject.GetComponent<PlayerController>();
    }

    public void PlayerStatusUpdate()
    {
        if (_isAlive){
            _isAlive = false;
        } else {
            _isAlive = true;
        }
    }

    public void MinusOneLife ()
    {
        _lives --;
        livesIMG[_lives].SetActive(false);
    }

    public void GameOver ()
    {
        gameOver.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerStatusUpdate();
    }

    public void Restart()
    {
        _lives = 2;
        gameOver.SetActive(false);
        playerController.Respawn();
        livesIMG[0].SetActive(true);
        livesIMG[1].SetActive(true);
    }
}
