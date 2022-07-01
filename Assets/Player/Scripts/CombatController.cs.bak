using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootingCooldown;

    private PlayerState playerState;
    private PlayerController playerController;
    private EnemyController enemyBlock;
    private bool shootingInCooldown = false;
    public bool isAlive = false;

    private void Awake()
    {
        playerState = gameObject.GetComponent<PlayerState>();
        playerController = gameObject.GetComponent<PlayerController>();
        enemyBlock = GameObject.FindGameObjectWithTag("EnemyCollide").GetComponent<EnemyController>();
    }
    
    private void Update()
    {
        Shoot();
    }

    public void LoseLife()
    {
        if (playerState.lives > 0)
        {
            playerController.Respawn();
            playerState.MinusOneLife();
        } else {
            //Gameover
            playerState.GameOver();
            enemyBlock.GameStatusUpdate();
        }
    }

    public void Shoot()
    {
        if (playerState.isAlive && Input.GetButtonDown("Fire1"))
        {
            if(!shootingInCooldown){
                GameObject newBullet = Instantiate(bullet);
                newBullet.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y);
                newBullet.SetActive(true);
                playerController.Play("shoot");
                shootingInCooldown = true;
                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);
        shootingInCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
            playerController.Play("playerExplosion");
            LoseLife();
            return;
        }

        if (other.CompareTag("EnemyCollide"))
        {
            LoseLife();
            other.gameObject.transform.position = new Vector3(5f, 5f);
            return;
        }   
    }

}
