using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private float firstenemyPositionX;
    [SerializeField] private float spaceBetweenEnemiesX;
    [SerializeField] private float firstenemyPositionY;
    [SerializeField] private float spaceBetweenEnemiesY;
    [SerializeField] private int enemiesPerRow;
    [SerializeField] private int rowAmmount;

    public bool coroutineActive = false;

    private GameObject[,] enemies;

    private bool gameEnded = true;
    private int totalEnemies;
    private int enemiesKilled = 0;
    
    [SerializeField] private Sounds[] sounds;

    private Transform _transform;
    private EnemyMovement enemyMovement;


    void Awake()
    {
        _transform = transform;
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        totalEnemies = rowAmmount * enemiesPerRow;
    }

    void Update() {
        if (!gameEnded && !coroutineActive)
        {
            StartCoroutine(enemyMovement.Movement());
        }
    }



//Teniendo como referencia la posición incial del primer enemigo, crea toda la matriz de enemigos.
//Tamaño de la matriz: rows: rowAmmount - columns: enemiesPerRow
    private void CreateEnemyMatrix()
    {
        float posX = firstenemyPositionX;
        float posY = firstenemyPositionY;
    
        for (int i = 0; i < rowAmmount; i++)
        {
            for (int j = 0; j < enemiesPerRow; j++)
            {
                Vector3 position = new Vector3(posX, posY);
                CreateEnemy(position, i, j);
                posX += spaceBetweenEnemiesX;
            }
            posX = firstenemyPositionX;
            posY -= spaceBetweenEnemiesY;
        }
    }
//--------------------------------------------------------------------

//Crea un enemigo, en la posicion pasada por argumento. 
//Un switch con un numero aleatorio se encarga de escoger un color al azar para el nuevo enemigo instanciado. 
//Le asigna al script del enemigo creado los valores de su posicion en la matriz: i(row) - j(column).
//Guarda la instancia de ese enemigo en un array, y le asigna de parent el GameObject que tiene este componente.
    private void CreateEnemy (Vector3 position, int i, int j)
    {
        GameObject newEnemy = Instantiate(enemySO.enemy);
        EnemyCombatController newEnemyCombatController = newEnemy.GetComponent<EnemyCombatController>();
        Transform newEnemyTransform = newEnemy.transform;

        switch (UnityEngine.Random.Range(1, 5))
        {
            case 1:
                SetEnemyColorAndLives(newEnemy, Color.blue, 1, newEnemyCombatController);
                break;
            case 2:
                SetEnemyColorAndLives(newEnemy, Color.green, 1, newEnemyCombatController);
                break;
            case 3:
                SetEnemyColorAndLives(newEnemy, Color.yellow, 2, newEnemyCombatController);
                break;
            case 4:
                SetEnemyColorAndLives(newEnemy, Color.red, 2, newEnemyCombatController);
                break;
            default:
                break;
            
        }
        
        newEnemyTransform.position = position;

        newEnemyCombatController.i = i;
        newEnemyCombatController.j = j;
        newEnemyCombatController.gameEnded = true;
        enemies[i,j] = newEnemy;

        newEnemyTransform.SetParent(_transform);
    }

    private void SetEnemyColorAndLives(GameObject newEnemy, Color color, int lives, EnemyCombatController newEnemyCombatController)
    {
        newEnemy.GetComponentInChildren<SpriteRenderer>().color = color;
        newEnemyCombatController.lives = lives;
    }
//---------------------------------------------------------------------------------------------------------


// Compara el color del argumento con el de la nave en la posicion "i"-"j".
// Devuelve Verdadero si son iguales, Falso si son diferentes.
    public bool CheckColor(Color color, int i, int j)
    {
        return enemies[i,j].GetComponentInChildren<SpriteRenderer>().color == color;
    }
//-------------------------
// Esta funcion es la encargada de analizar las naves vecinas a la que esta "muriendo".
// Recibe como argumentos: Color (color de la nave moribunda) - i-j (enteros que representan la posicion de la nave en la matriz).
// Primero chequea si es que no han muerto todas las naves ya. En caso afirmativo reinicia la matriz. 
// Si no han muerto todas las naves, analiza los lugares vecinos (norte, sur, este y oeste) de la nave que esta muriendo en busca de mas naves.
// Si encuentra una nave viva, compara el color. Si es el mismo, mata a la nave encontrada. (Se vuelve a llamar a esta funcion recursivamente por cada 
// nave que se va sumando a la cadena) 

    public bool CheckIFLastEnemyAliveDied()
    {
        enemiesKilled++;

        if(enemiesKilled == totalEnemies)
        {
            StopAllCoroutines();
            GameStatusUpdate();
            Restart();
            GameStatusUpdate();
            coroutineActive = false;
            return true;
        }

        return false;
    }

    public void CheckSides(Color color, int i, int j)
    {   
            int iUp = i - 1;
            int iDown = i + 1;
            int jRight = j + 1;
            int jLeft = j - 1;

            if (iUp >= 0) {
                if (enemies[iUp,j] != null && !enemies[iUp,j].GetComponent<EnemyCombatController>().isDeath && CheckColor(color, iUp, j)) {
                    enemies[iUp,j].GetComponent<EnemyCombatController>().Death();
                    enemies[iUp,j] = null;
                }
            }
            if (iDown <= rowAmmount - 1) {
                if (enemies[iDown,j] != null && !enemies[iDown,j].GetComponent<EnemyCombatController>().isDeath && CheckColor(color, iDown, j)) {
                    enemies[iDown,j].GetComponent<EnemyCombatController>().Death();
                    enemies[iDown,j] = null;
                }
            }
            if (jRight <= enemiesPerRow - 1) {
                if (enemies[i,jRight] != null && !enemies[i,jRight].GetComponent<EnemyCombatController>().isDeath && CheckColor(color, i, jRight)) {
                    enemies[i,jRight].GetComponent<EnemyCombatController>().Death();
                    enemies[i,jRight] = null;
                }
            }
            if (jLeft >= 0) {
                if (enemies[i,jLeft] != null && !enemies[i,jLeft].GetComponent<EnemyCombatController>().isDeath && CheckColor(color, i, jLeft)) {
                    enemies[i,jLeft].GetComponent<EnemyCombatController>().Death();
                    enemies[i,jLeft] = null;
                }
            }
        
    }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------

// Analiza la posicion en la matriz indicada con los argumentos i-j para saber si corresponde a la ultima fila "viva".
    public bool CheckIfLast(int i, int j)
    {
        bool isLast = true;
        int iDown = i + 1;
        while(iDown <= rowAmmount - 1){
            if (enemies[iDown,j] != null && !enemies[iDown,j].GetComponent<EnemyCombatController>().isDeath) {
                isLast = false;
                break;
            }
            iDown++;
        }
        
        return isLast;
    }
//-----------------------------------------------------------------------------------------------------------------------

// Actualiza el estado del juego. "GameEnded"= true, pone al juego en un estado de pausa. 
// Aplica el cambio de estado tanto al bloque, como a cada nave en particular.
    public void GameStatusUpdate()
    {
        if (!gameEnded){
            gameEnded = true;

            for (int i = 0; i < rowAmmount; i++)
            {
                for (int j = 0; j < enemiesPerRow; j++)
                {
                    if(enemies[i,j] != null)
                    {
                        enemies[i,j].GetComponent<EnemyCombatController>().gameEnded = true;
                    }
                }
            }
        } else {
            gameEnded = false;

            for (int i = 0; i < rowAmmount; i++)
            {
                for (int j = 0; j < enemiesPerRow; j++)
                {
                    if(enemies[i,j] != null)
                    {
                        enemies[i,j].GetComponent<EnemyCombatController>().gameEnded = false;
                    }
                }
            }
        }
    }
//-----------------------------------------------------------------------------------------------------


// Reinicia totalmente el bloque de naves.
// Destruye las naves que queden, reinicia la posicion del bloque, vacia el array de naves y, finalmente, crea las naves otra vez.
    public void Restart()
    {
        enemiesKilled = 0;
        for (int i = 0; i < rowAmmount; i++)
        {
            for (int j = 0; j < enemiesPerRow; j++)
            {
                if(enemies != null && enemies[i,j] != null)
                {
                    Destroy(enemies[i,j]);
                }
            }
        }

        _transform.position = new Vector3(5f, 5f);
        enemies = new GameObject[rowAmmount, enemiesPerRow];
        CreateEnemyMatrix();
    }
//-----------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CombatController>().LoseLife();
            transform.position = new Vector3(5f, 5f);
            return;
        }   
        
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    
}
