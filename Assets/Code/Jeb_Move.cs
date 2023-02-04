using UnityEngine;

public class Jeb_Move : MonoBehaviour
{ 
    //sets the player
    private Rigidbody2D rigibody;
    
    //sets camera
    private Camera p_Camera;
    
    //speed for player
    private Vector2 velocity;
    private float inputAxis;
    
    //movement speed
    public float moveSpeed = 8f; 
    
    //jump control
    public float maxJHeight = 5f;
    public float maxJTime = 1f;
    public float JForce => (2f * maxJHeight) / (maxJTime / 2f);

    //gravity for jump
    public float gravity => (-2f * maxJHeight) / Mathf.Pow((maxJTime / 2f), 2);
    
    //private to be able to call from other scripts
    public bool grounded{ get; private set;}
    public bool jumping{get; private set;}

    private void Awake()
    {
        //makes it so camera follows player
        rigibody = GetComponent<Rigidbody2D>();
        p_Camera = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();

        grounded = rigibody.Raycast(Vector2.down);
        if(grounded)
        {
            GroundedMovement();
        }

        Gravity();
    }

    void GroundedMovement()
    {
        //controls player while they are on the gorund & checks if they are jumping
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        if(Input.GetButtonDown("Jump"))
        {
            velocity.y = JForce;
            jumping = true;
        }
    }

    void Gravity()
    {
        //controls player falling down after reaching apex
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f: 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity /2f);
    }

    private void HorizontalMovement()
    {
        //controls movement of player going left & right
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if(rigibody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        if( velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }
    //happens after update
    private void FixedUpdate() 
    {
        //camera follows player and keeps it centered on player
        Vector2 position = rigibody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftedge = p_Camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightedge = p_Camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftedge.x, rightedge.x);
        
        rigibody.MovePosition(position);
    }

    //stops the jump when hitting block
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if(transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
                Debug.Log("jump speed 0");
            }
        }
    }


}
