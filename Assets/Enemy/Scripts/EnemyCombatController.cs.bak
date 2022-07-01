using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyCombatController : MonoBehaviour
{
    public int i;
    public int j;
    [SerializeField] private int shootProb;
    [SerializeField] private int shootingCooldown;
    public int lives;

    public bool isDeath = false;
    [SerializeField] private GameObject bullet;
    private bool shootingInCooldown = false;
    
    public bool gameEnded = false;

    private Color thisColor;
    private EnemyController parentEnemyController;
    private Transform _transform;

    

    void Start()
    {
        thisColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        parentEnemyController = gameObject.GetComponentInParent<EnemyController>();
        _transform = gameObject.transform;
    }

    void Update()
    {
        if (!gameEnded)
        {
            ShootPlayer();
        }
    }

// Tiene ciertas chances de disparar al jugador indicadas por "shootProb".
// Si cimple con esas probabilidades, no tiene enfriamiento y es la ultima nave de la columna, instancia una bala que es disparada hacia abajo.
    private void ShootPlayer()
    {
        int prob = UnityEngine.Random.Range(1, shootProb);
        bool isLast = parentEnemyController.CheckIfLast(i, j);

        if(prob == 1 && !shootingInCooldown && isLast){
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = new Vector3(_transform.position.x, _transform.position.y);
            newBullet.SetActive(true);
            shootingInCooldown = true;
            StartCoroutine(Cooldown());
        }
    }
//---------------------------------------------------------------------------------------------------------------------------

// Actualiza el estado de la nave a "muerta". Envia el color de la nave y su posicion a la funcion "CheckSides" para buscar naves vecinas con el mismo color.
// Finalmente destruye el objeto de la nave.
    public void Death()
    {
        isDeath = true;
        gameObject.GetComponentInParent<EnemyController>().CheckSides(thisColor, i, j);
        Destroy(gameObject);
    }
//-----------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            lives--;
            if (lives <= 0)
            {
                gameObject.GetComponentInParent<EnemyController>().Play("enemyExplosion");
                Death();
            }
            return;
        }

        if (other.CompareTag("SideLimit"))
        {
            gameObject.GetComponentInParent<EnemyMovement>().MoveDown();
        }   
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);
        shootingInCooldown = false;
    }
}
