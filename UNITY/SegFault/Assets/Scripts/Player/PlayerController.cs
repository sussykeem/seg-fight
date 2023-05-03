using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerController : MonoBehaviour
{
    public GameObject Character;
    public string charName;

    private PlayerControls playerControls;

    private GameObject [] players;
    public GameObject otherPlayer;
    
    private Rigidbody2D rb;

    //Horizontal Moving Variables
    public float canMove = 0.0f;
    public float gMoveSpeed = 25.0f;
    public float aMoveSpeed = 35.0f;
    public float curMoveSpeed = 0;
    public float moveTime = 1.0f;
    public float moveDist = 3f;
    public bool atPos = true;
    public Vector2 MoveDir = Vector2.zero;

    //Jumping Variables
    public float jumpForce = 28.0f;
    public bool onGround = false;
    public float horizontalJump = 4.5f;
    public float vertDamper = 14f;
    public float jumpTime = 0.5f;
    public float canJump = 0.0f;
    private Vector2 jumpMoveDir = Vector2.zero;

    public GameObject groundCheckObj;
    groundCheck gcs;

    //Blocking Variables
    public bool isBlock = false;
    public float blockThreshold = -0.5f;

    //Relative Pos, for flipping sprites, to face each other
    public bool flipped = false;
    public float flipTime = 1.0f;
    public float canFlip = 0.0f;
    public float flipSwap = 1;

    //For collision detection and stuff
    private Coroutine moveCo;

    //Getting Character Information
    public float health = 0.0f;
    private Dictionary<int, float>[] moveContainer;

    //Attacking Variables
    public BoxCollider2D damageCollider;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = Character.GetComponent<Rigidbody2D>();
        gcs = groundCheckObj.GetComponent<groundCheck>();
        damageCollider = gameObject.GetComponent<BoxCollider2D>();
        damageCollider.enabled = false;
        charName = Character.name;
        charName = charName.Substring(0, charName.Length - 7);
        moveContainer = new []
        {   
            new Dictionary<int, float>(),
            new Dictionary<int, float>(),
            new Dictionary<int, float>(),
            new Dictionary<int, float>()
        };

        ReadCharInfo();
        Debug.Log(health);
        Debug.Log(moveContainer[0].Keys + " " + moveContainer[0].Values);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].name == gameObject.name)
        { //Player[0] is this player, so this player is player1
            otherPlayer = players[1];
        }
        else
        { //Player[0] is not this player, so this player is player2
            otherPlayer = players[0];
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        onGround = gcs.onGround;

        if (onGround){ //if on ground count down to when they can jump and move on the ground
            canMove -= Time.deltaTime;
            canJump -= Time.deltaTime;
            canFlip -= Time.deltaTime;
        }

        if(MoveDir.y <= blockThreshold) //Character is blocking if they are holding down
        {
            isBlock = true;
        } else
        {
            isBlock = false;
        }
        if (canMove <= 0.0f && !isBlock && onGround == true && (MoveDir.x >= 0.5 || MoveDir.x <= -0.5) && MoveDir.y < 0.5) //Character can move if they are not blocking and its been time since the last move
        {
            atPos = false;
            HorizMove();
            canMove = moveTime;
        }
        if (onGround && MoveDir.y >= 0.5 && atPos == true && canJump <= 0.0f) //Character can jump if they are on the ground and pressing up
        {
            var curForce = jumpForce - vertDamper * MoveDir.y;
            jumpMoveDir = MoveDir.normalized;
            jumpMoveDir = new Vector2(jumpMoveDir.x * horizontalJump, jumpMoveDir.y * curForce);
            rb.AddForce(jumpMoveDir, ForceMode2D.Impulse);
            canJump = jumpTime;
        }

        if (onGround && otherPlayer.GetComponent<PlayerController>().onGround && canFlip <= 0)
        {
            rotChar();
        }

        if (playerControls.Gameplay.LightAttack.triggered)
        {
            damageCollider.enabled = true;
            Debug.Log("Light Attack");
        }

            //Making sure these values don't grow up to an absurd size
        if (canJump < -1)
        {
            canJump = -1;
        }
        if(canMove < -1)
        {
            canMove = -1;
        }
        if(canFlip < -1)
        {
            canFlip = -1;
        }
    }

    private void HorizMove() //this moves the player horizontally using the MovePlayer coroutine, from their current position to set endPos in a setTime
    {
        StopAllCoroutines();
        if (atPos == false)
        {
            Vector2 movePos = new Vector2(transform.position.x + MoveDir.normalized.x * moveDist, transform.position.y);
            moveCo = StartCoroutine(MovePlayer(movePos));
        }
    }

    IEnumerator MovePlayer(Vector2 targetPos)
    {
        float timeElap = 0;
        Vector2 startPos = transform.position;
        while (timeElap < moveTime)
        {
            var t = Mathf.SmoothStep(0, 1, timeElap / moveTime);
            transform.position = Vector2.Lerp(startPos, targetPos, t);
            timeElap += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        atPos = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == otherPlayer)
        {
            StopCoroutine(moveCo);
            atPos = true;
            canMove = -1;
        }
    }

    private void ReadCharInfo() //Gets each character's info from a text file
    {
        int counter = 0;
        string first, second;
        string path = "Assets/Characters/CharInfo/" + charName + ".txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            if(counter == 0) //get character's health
            {
                health = float.Parse(reader.ReadLine());
            }
            first = reader.ReadLine();
            second = reader.ReadLine();
            moveContainer[counter].Add(int.Parse(first), float.Parse(second));
            counter++;
        }
        reader.Close();
    }
    public void rotChar()
    {
        if (flipped)
        {
            flipSwap = -1;
        } else
        {
            flipSwap = 1;
        }

        float directionScale = Mathf.Sign(otherPlayer.transform.position.x - transform.position.x) * flipSwap;
        var eulerAngle = transform.rotation.eulerAngles;
        var fixedQuat = Quaternion.Euler(eulerAngle.x, eulerAngle.y + 180, eulerAngle.z);

        if (players[0].name == gameObject.name && directionScale < 0)
        {
            flipped = !flipped;
            transform.rotation = fixedQuat;
            canFlip = flipTime;
        } else if (players[1].name == gameObject.name && directionScale > 0)
        {
            flipped = !flipped;
            transform.rotation = fixedQuat;
            canFlip = flipTime;
        }
    }
}
