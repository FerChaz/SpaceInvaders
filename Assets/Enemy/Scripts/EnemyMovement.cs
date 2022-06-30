using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] private float rightLimit;
    [SerializeField] private float leftLimit;
    [SerializeField] private float spaceBetweenEnemiesY;
    [SerializeField] private float ticksToMove;


    private bool movingRight = true;
    private bool movingLeft = false;

    private bool movingDown = false;
    private bool movingDownCO = false;
    

    private Transform _transform;
    private EnemyController enemyController;


    void Awake()
    {
        _transform = transform;
        enemyController = gameObject.GetComponent<EnemyController>();
    }


// Script de movimiento. 
// Cada 2 segundos el bloque entero de enemigos se va a mover.
// Sobre la distancia total que deben recorrer, calcula la distancia entre cada "tick".
// Analiza la direccion hacia la que debe moverse el bloque y tambien si ya ha llegado al limite en esa direccion. Pueden darse dos situaciones para cada direccion.
// Caso 1: El bloque no llego, por lo que se mueve un tick mas hacia la direccion correcpondiente.
// Caso 2: El bloque llego al limite. En este caso el bloque baja una distancia indicada por "spaceBetweenEnemiesY", y comienza a moverse hacia la otra direccion.
    public IEnumerator Movement()
    {
        enemyController.coroutineActive = true;
        float movementInX = ticksToMove;
        float currentPositionX = _transform.position.x;
        float currentPositionY = _transform.position.y;

        yield return new WaitForSeconds(2);

        if (!movingDown && movingRight)
        {
            MovingLeftRight(currentPositionX, movementInX, currentPositionY);
        }

        if (!movingDown && movingLeft)
        {
            MovingLeftRight(currentPositionX, -movementInX, currentPositionY);
        }

        if (movingDown)
        {
            _transform.position = new Vector3(_transform.position.x, currentPositionY - spaceBetweenEnemiesY);
            movingDown = false;
        }

        enemyController.coroutineActive = false;
    }

    private void MovingLeftRight(float currentPositionX, float movementInX, float currentPositionY)
    {
        _transform.position = new Vector3(currentPositionX + movementInX, _transform.position.y);
    }
//-----------------------------------------------------------------------------------------------------------------------------------------

public void ChangeDirection()
{
    if (movingRight)
    {
        movingLeft = true;
        movingRight = false;
    } else {
        movingLeft = false;
        movingRight = true;
    }

    StartCoroutine(WaitToMoveDownCooldown());
}

public void MoveDown()
{
    if (!movingDownCO) {
        movingDownCO = true;
        movingDown = true;
        ChangeDirection();
    }
}

public IEnumerator WaitToMoveDownCooldown()
{
    yield return new WaitForSeconds(5);
    movingDownCO = false;
}
    
}
