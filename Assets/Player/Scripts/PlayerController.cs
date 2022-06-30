using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float startingPosition;


    [SerializeField] private Sounds[] sounds;
    private GameState gameState;
    private PlayerState playerState;

    void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        playerState = gameObject.GetComponent<PlayerState>();

        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

    }

    void Update()
    {
        if (playerState.isAlive)
        {
            Move();
            gameState.Pause();
        }
    }

    private void Move()
    {
        Vector2 playerPos = transform.position;
            
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0) * speed * Time.deltaTime;

        if(transform.position.x < leftBorder) transform.position = new Vector3(leftBorder, transform.position.y, transform.position.z);
        if(transform.position.x > rightBorder) transform.position = new Vector3(rightBorder, transform.position.y, transform.position.z);
    }

    public void Respawn()
    {
        transform.position = new Vector3(startingPosition, 0);
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

}
