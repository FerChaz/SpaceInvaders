using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConfig : MonoBehaviour
{
    public Vector2 velocity;
    public string owner;
    public float speed;
    public float lifeTime;
    float timer;

    private const string _PLAYER = "player";
    private const string _ENEMY = "enemy";

    void Start()
    {
        timer = lifeTime;
    }

    void Update()
    {
        BulletTravel();
    }

    public void ResetTimer()
    {
        timer = lifeTime;
    }

    public void BulletTravel()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }


    /*private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Triggered");
        if (owner == _PLAYER && other.CompareTag("Enemy"))
        {
            Debug.Log("Triggered enemy");
            other.GetComponent<EnemyCombatController>().Death();
            return;
        }

        if (owner == _ENEMY && other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CombatController>().LoseLife();
            return;
        }    
    }*/
}
