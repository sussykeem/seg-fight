using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerController : MonoBehaviour
{
    public GameObject Character;
    public string charName;

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
    public float vertDamper = 14.5f;
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
    public int health = 100;
    public Dictionary<int, int[]>[] moveContainer;

    //Attacking Variables
    public BoxCollider2D[] damageColliders;
    public attackScri attackScript;
    public bool lAtt, hAtt, spAtt, shAtt = false;
    public bool[] attackType = new bool[4] { false, false, false, false };
    public float lAttTime, hAttTime, spAttTime, shAttTime;
    public float canAttack = 0.0f;

    private void Awake()
    {
        rb = Character.GetComponent<Rigidbody2D>();
        gcs = groundCheckObj.GetComponent<groundCheck>();
        attackScript = gameObject.GetComponentInChildren<attackScri>();

        damageColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();

        charName = Character.name;
        charName = charName.Substring(0, charName.Length - 7);
        moveContainer = new[]
        {
            new Dictionary<int, int[]>(),
            new Dictionary<int, int[]>(),
            new Dictionary<int, int[]>(),
            new Dictionary<int, int[]>()
        };

        ReadCharInfo();

        /* Loop through moveContainer Dictionary 
        for(int i = 0; i < moveContainer.Length; i++)
        {
            foreach( var testValue in moveContainer[i])
            {
                var damg = testValue.Key;
                var result = testValue.Value;
                Debug.Log(damg);
                for(int j = 0; j < result.Length; j++)
                {
                    Debug.Log(", " + result[j]);
                }
            }
        }
        */
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
    { // Get Move values
        MoveDir = context.ReadValue<Vector2>();
    }

    public void OnLAtt(InputAction.CallbackContext context)
    { // Get light attack value
        if (context.performed && hAtt == false && spAtt == false && shAtt == false)
        {
            lAtt = true;
            attackType[0] = lAtt;
        }
        if (context.canceled)
        {
            lAtt = false;
            attackType[0] = lAtt;
        }
    }

    public void OnHAtt(InputAction.CallbackContext context)
    { // Get heavy attack value
        if (context.performed && lAtt == false && spAtt == false && shAtt == false)
        {
            hAtt = true;
            attackType[1] = hAtt;
        }
        if (context.canceled)
        {
            hAtt = false;
            attackType[1] = hAtt;
        }
    }

    public void OnSpAtt(InputAction.CallbackContext context)
    { // Get special attack value
        if (context.performed && hAtt == false && lAtt == false && shAtt == false)
        {
            spAtt = true;
            attackType[2] = spAtt;
        }
        if (context.canceled)
        {
            spAtt = false;
            attackType[2] = spAtt;
        }
    }

    public void OnShAtt(InputAction.CallbackContext context)
    { // Get sheild break value
        if (context.performed && hAtt == false && spAtt == false && lAtt == false)
        {
            shAtt = true;
            attackType[3] = shAtt;
        }
        if (context.canceled)
        {
            shAtt = false;
            attackType[3] = shAtt;
        }
    }

    private void FixedUpdate()
    {
        onGround = gcs.onGround;

        if (onGround){ //if on ground count down to when they can jump and move on the ground
            canMove -= Time.deltaTime;
            canJump -= Time.deltaTime;
            canFlip -= Time.deltaTime;
            canAttack -= Time.deltaTime;
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

        if(onGround && canAttack <= 0)
        {
            attack();
        }

        normalizeTimers();
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
        int counter = 1;
        int moveNum = 0;
        int dmgKey = 0;
        string first, second;
        string[] secondWords;
        string path = "Assets/Characters/CharInfo/" + charName + ".txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            if (counter % 2 != 0)
            { //Getting Damage of move, key for Dictionaries
                first = reader.ReadLine();
                dmgKey = int.Parse(first);
            }
            else
            { //Getting the Damage frames of the move, val for Dictionaries
                second = reader.ReadLine();
                secondWords = second.Split(' ');
                int[] tempFramVal = new int[secondWords.Length];
                for (int i = 0; i < secondWords.Length; i++)
                {
                    tempFramVal[i] = int.Parse(secondWords[i]);
                }
                moveContainer[moveNum].Add(dmgKey, tempFramVal);
                moveNum++;
            }
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

    private void attack()
    {
        attackScri.attack();
        /*
        if (lAtt)
        {
            lCol.enabled = true;
            canAttack = lAttTime;
            return;
        }
        else
        {
            lCol.enabled = false;
        }

        if (hAtt)
        {
            hCol.enabled = true;
            canAttack = hAttTime;
            return;
        }
        else
        {
            hCol.enabled = false;
        }

        if (spAtt)
        {
            spCol.enabled = true;
            canAttack = spAttTime;
            return;
        }
        else
        {
            spCol.enabled = false;
        }

        if (shAtt)
        {
            shCol.enabled = true;
            canAttack = shAttTime;
            return;
        }
        else
        {
            shCol.enabled = false;
        }
        */
    }

    private void normalizeTimers()
    {
        //Making sure these values don't grow up to an absurd size
        if (canJump < -1)
        {
            canJump = -1;
        }
        if (canMove < -1)
        {
            canMove = -1;
        }
        if (canFlip < -1)
        {
            canFlip = -1;
        }
        if (canAttack < -1)
        {
            canAttack = -1;
        }
    }
}
