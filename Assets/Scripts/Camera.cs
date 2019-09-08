using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float smoothTimeX;
    public float smoothTimeY;

    private Vector2 velocity;
    private Transform player;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
