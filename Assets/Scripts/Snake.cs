using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Transform wallCheck;
    public float speed;
    public int health;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2;
    private bool isFacingRight = true;
    private bool touchedWall = false;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody2 = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.touchedWall = Physics2D.Linecast(base.transform.position, this.wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (touchedWall)
        {
            this.Flip();
        }
    }

    private void FixedUpdate()
    {
        this.rigidbody2.velocity = new Vector2(speed, this.rigidbody2.velocity.y);
    }

    private void Flip()
    {
        this.isFacingRight = !isFacingRight;
        base.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        this.speed *= -1;
    }
}
