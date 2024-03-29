﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public AudioClip fxCoin;

    private Player player;
    private Rigidbody2D rigidbody2Player;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player").GetComponent<Player>();
        this.rigidbody2Player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            player.DamagePlayer();
        }

        if (collision.CompareTag("BoundaryDeath"))
        {
            player.KillPlayer();
        }

        if (collision.CompareTag("Coin"))
        {
            Debug.Log("Coletou");
            SoundManager.instance.PlaySound(fxCoin);
            Destroy(collision.gameObject);
        }
    }

    
}
