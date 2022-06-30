using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private Sprite[] states; 
    [SerializeField] private int hitAmountToDestroy;
    private int state = 0;
    private int hits = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("EnemyBullet") || other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            ChangeShieldState();
            return;
        }  
    }

    private void ChangeShieldState()
    {
        hits++;

        if (state == 0 &&   hits == hitAmountToDestroy) 
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = states[1];
            hits = 0;
            state = 1;
            return;
        }

        if (state == 1 && hits == hitAmountToDestroy)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = states[2];
            hits = 0;
            state = 2;
            return;
        }

        if (hits == hitAmountToDestroy)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void ActivateShield()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = states[0];
        hits = 0;
        state = 0;
        gameObject.SetActive(true);
    }
}
