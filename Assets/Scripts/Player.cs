using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region [ Variáveis públicas ]

    public float speed;
    public int jumpForce;
    public int health;
    public Transform groundCheck;

    #endregion  

    #region [ Variáveis privadas ] 
    private bool isInvulnerable = false;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFacingRight = true;

    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private Transform transform;

    #endregion 

    // Start is called before the first frame update
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    private void Update() {
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && isGrounded){
            isJumping = true;
        }

        this.Animations();
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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);
    }

    private void Animations(){
        animator.SetFloat("VelY", rigidBody2D.velocity.y);
        animator.SetBool("isMidAir", !isGrounded);
        animator.SetBool("isWalking", isGrounded && rigidBody2D.velocity.x != 0f);
    }
}