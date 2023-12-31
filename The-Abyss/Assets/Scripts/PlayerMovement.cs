using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private GrappleHook gh;

    [Header("Movement")]
    [SerializeField] private float movementVelocity;
    [Range(0, 0.3f)] [SerializeField] private float movementSoft;
    private float actualVelocity;
    private float movementHor = 0;
    private Vector3 velocity = Vector3.zero;
    private bool lokingRight = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundController;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool grounded;
    private float actualJumpForce;
    private bool jump = false; 

    [Header("Run")]
    private bool releasedShiftInAir = false;
    private float runVelocity;
    
    [Header("Respawn")]
    Vector3 startPoint;
    [SerializeField] private Transform[] respawns;
    public static int count;


    [Header("Animation")]
    private Animator animator;

    [Header("Sound")]
 
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip key2Sound;
    [SerializeField] private AudioSource Music1;
    [SerializeField] private AudioSource Music2;

    [Header("EasterEggs")]
    public static bool canUnlock;
    private Rubys rubyCounter;
    [SerializeField] public GameObject image;

    [Header("Coyote Time")]
    [SerializeField] private float timecoyoteTime = 0.1f;
    [SerializeField] private bool coyoteTime = false;
    [SerializeField] private float tiempocoyoteTime;

    [Header("Pause Controls")]
    public bool pause;

    private void Start()
    {
        
        count = 0;
        canUnlock = false;

        startPoint = respawns[0].position;
        rb2D = GetComponent<Rigidbody2D>();
        gh = GetComponent<GrappleHook>();
        runVelocity = movementVelocity * 2;
        actualVelocity = movementVelocity;
        actualJumpForce = jumpForce;

        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && grounded == true)
        {
            actualVelocity = runVelocity;
            if (jump == true)
            {
                RunJump();   
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (!grounded)
            {
                releasedShiftInAir = true;
            }
            else
            {
                StopRun();
            }
        }

        movementHor = Input.GetAxisRaw("Horizontal") * actualVelocity;

        animator.SetFloat("Horizontal", Mathf.Abs(movementHor));

        if ((Input.GetButtonDown("Jump") && coyoteTime && !pause) || (Input.GetKeyDown(KeyCode.W) && coyoteTime && !pause))
        {
            jump = true;

        }
            else
            {
                if(pause)
                {
                   jump = false;
                }
            }   

        if (grounded && releasedShiftInAir)
        {            
            StopRun();
            releasedShiftInAir = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerSound1.Instance.ExecuteSound(deathSound);
            this.gameObject.SetActive(false);
            StopRun();
            transform.position = startPoint;
            gh.ResetGrapple();
            Invoke("Delay", 1f);
        }

        if(!grounded && coyoteTime){
            tiempocoyoteTime += Time.deltaTime; 
            if(tiempocoyoteTime > timecoyoteTime) coyoteTime = false;
        }


    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapBox(groundController.position, boxDimensions, 0f, whatIsGround);

        if (!gh.retracting)
        {
            Move(movementHor * Time.fixedDeltaTime, jump);
        }
        else
        {
            rb2D.velocity = Vector2.zero;
        }

        jump = false;

          if (grounded == true){
            coyoteTime = true;
            tiempocoyoteTime = 0;
        }
    }

    private void Move(float move, bool jump)
    {
        Vector3 objectiveVel = new Vector2(move, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, objectiveVel, ref velocity, movementSoft);
        

        if (move > 0 && !lokingRight)
        {
            Flip();
        }
        else if (move < 0 && lokingRight)
        {
            Flip();
        }

        if (grounded && jump)
        {
            PlayerSound1.Instance.ExecuteSound(jumpSound);
            grounded = false;
            rb2D.AddForce(new Vector2(0f, actualJumpForce));    
                  
        }
    }

    private void RunJump()
    {
        actualVelocity = runVelocity;
        actualJumpForce = jumpForce * 1.5f;
        rb2D.gravityScale = 4.0f;                
    }

    private void StopRun()
    {
        actualVelocity = movementVelocity;
        actualJumpForce = jumpForce;
        rb2D.gravityScale = 2.35f;
    }

    private void Flip()
    {
        lokingRight = !lokingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundController.position, boxDimensions);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Finish"))
        {
            PlayerSound1.Instance.ExecuteSound(deathSound);
            this.gameObject.SetActive(false);
            transform.position = startPoint;
            StopRun();
            gh.ResetGrapple();
            Invoke("Delay", 1f);
        }

        if (collider.gameObject.CompareTag("Finish1"))
        {
            startPoint = respawns[0].position;
            
        }

        if (collider.gameObject.CompareTag("Finish2"))
        {
            startPoint = respawns[1].position;
    
        }

        if (collider.gameObject.CompareTag("Finish3"))
        {           
            startPoint = respawns[2].position;

        }

        if (collider.gameObject.CompareTag("Finish4"))
        {            
            startPoint = respawns[3].position;
          
        }

        if (collider.gameObject.CompareTag("Finish5"))
        {
            startPoint = respawns[6].position;
            
        }

        if (collider.gameObject.CompareTag("Finish6"))
        {
            startPoint = respawns[7].position;
            
        }

        if (collider.gameObject.CompareTag("Finish7"))
        {
            startPoint = respawns[8].position;
            
        }

        if (collider.gameObject.CompareTag("Finish8"))
        {
            startPoint = respawns[9].position;
           
        }

        if (collider.gameObject.CompareTag("Finish9"))
        {
            startPoint = respawns[10].position;

        }

        if (collider.gameObject.CompareTag("Finish10"))
        {
            startPoint = respawns[11].position;

        }

        if (collider.gameObject.CompareTag("Music1"))
        {
            Music1.Stop();
            Music2.Play();

        }

        if (collider.gameObject.CompareTag("Music2"))
        {
            Debug.Log(PlayerMovement.count);
            Music2.Stop();
            Music1.Play();
        }

        if (collider.gameObject.CompareTag("Unlock"))
        {            
            startPoint = respawns[4].position;
            transform.position = startPoint;
            
        }

        if (collider.gameObject.CompareTag("Unlock2"))
        {
            startPoint = respawns[5].position;
            transform.position = startPoint;
        }

        if (collider.gameObject.CompareTag("Level1"))
        {
            SceneManager.LoadScene(3);
        }

        if (collider.gameObject.CompareTag("End"))
        {
           
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (collider.gameObject.CompareTag("End2"))
        {

            SceneManager.LoadScene(5);
        }

        if (collider.gameObject.CompareTag("Key"))
        {
            PlayerSound1.Instance.ExecuteSound(keySound);
            canUnlock = true;
            Destroy(collider.gameObject);
        }

         if (collider.gameObject.CompareTag("Coin"))
        {
            image.SetActive(true);
            Invoke("Delay2", 2f);
        }

       
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            PlayerSound1.Instance.ExecuteSound(deathSound);
            this.gameObject.SetActive(false);
            transform.position = startPoint;
            StopRun();
            gh.ResetGrapple();
            Invoke("Delay", 1f);
        }

        if (collider.gameObject.CompareTag("SecretWall") && canUnlock)
        {
            PlayerSound1.Instance.ExecuteSound(key2Sound);
            collider.gameObject.SetActive(false);
        }    
    }

    private void Delay()
    {
        this.gameObject.SetActive(true);
    }

     private void Delay2()
    {
        image.SetActive(false);
        
    }
}