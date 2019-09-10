﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region [ Variáveis públicas ]

    public float speed;
    public float attackRate;
    public int jumpForce;
    public int health;
    public Transform groundCheck;
    public Transform attackSpawn;
    public GameObject slashPrefab;
    public GameObject coroa;
    public AudioClip fxHurt;
    public AudioClip fxJump;
    public AudioClip fxAttack;

    #endregion  

    #region [ Variáveis privadas ] 

    private bool isInvulnerable = false;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFacingRight = true;

    private float nextAttack = 0f;

    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private Camera camera;

    #endregion 

    // Start is called before the first frame update
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update() {
        isGrounded = Physics2D.Linecast(base.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && isGrounded){
            isJumping = true;
            SoundManager.instance.PlaySound(fxJump);
        }

        this.Animations();

        if (Input.GetButtonDown("Fire1") && isGrounded && Time.time > nextAttack)
        {
            SoundManager.instance.PlaySound(fxAttack);
            this.Attack();
        }
    }

    private void FixedUpdate() {

        float move = Input.GetAxis("Horizontal");
        rigidBody2D.velocity = new Vector2(move * speed, rigidBody2D.velocity.y);

        if ( (move < 0f && isFacingRight) || (move > 0f && !isFacingRight) )
        {
            this.Flip();
        }

        if (isJumping){
            rigidBody2D.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }
    }

    private void Flip(){
        isFacingRight = !isFacingRight;
        base.transform.localScale = new Vector3(-base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.x);
    }

    private void Animations(){
        animator.SetFloat("VelY", rigidBody2D.velocity.y);
        animator.SetBool("isMidAir", !isGrounded);
        animator.SetBool("isWalking", isGrounded && rigidBody2D.velocity.x != 0f);
    }

    private void Attack(){
        animator.SetTrigger("Attack");
        nextAttack = Time.time + attackRate;

        GameObject attackClone = Instantiate(slashPrefab, attackSpawn.position, attackSpawn.rotation);

        if (!isFacingRight)
        {
            attackClone.transform.eulerAngles = new Vector3(180, 0, 180);
        }
    }

    private IEnumerator DamageEffect()
    {
        camera.ShakeCamera(0.5f, 0.25f);

        for (float i = 0f; i < 1; i+= 0.1f)
        {
            this.sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        this.isInvulnerable = false;
    }

    public void DamagePlayer()
    {
        if (!isInvulnerable)
        {
            this.isInvulnerable = true;
            this.health--;
            StartCoroutine(this.DamageEffect());
            SoundManager.instance.PlaySound(fxHurt);
            Hud.instance.RefreshLife(this.health);

            if (this.health <= 0)
            {
                Debug.Log("Murieu");
                this.KillPlayer();
                
            }
        }
    }

    public void KillPlayer()
    {
        GameObject cloneCoroa = Instantiate(coroa, transform.position, Quaternion.identity);
        Rigidbody2D rigidbody2DCoroa = cloneCoroa.GetComponent<Rigidbody2D>();

        rigidbody2DCoroa.AddForce(Vector3.up * 500);

        Invoke("ReloadLevel", 2f);
        gameObject.SetActive(false);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}