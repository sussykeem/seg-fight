using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class PlayerController : MonoBehaviour
{
    public GameObject Character;
    private string charName;

    private GameObject [] players;
    public GameObject otherPlayer;

    private GameObject gameTimer;
    private Timer timerSc;
    
    private Rigidbody2D rb;
    public Animator anim;

    //Horizontal Moving Variables
    private float moveTime = 1f;
    private float canMove = 0.0f;
    private float moveDist = 3f;
    private float dirFace = 0.0f;
    private bool atPos = true;
    private Vector2 MoveDir = Vector2.zero;

    //Jumping Variables
    private float jumpForce = 28.0f;
    public bool onGround = false;
    private float horizontalJump = 5f;
    private float vertDamper = 16f;
    private float jumpTime = 0.5f;
    private float canJump = 0.0f;
    private Vector2 jumpMoveDir = Vector2.zero;

    public GameObject groundCheckObj;
    groundCheck gcs;

    //Blocking Variables
    public bool isBlock = false;
    private float blockThreshold = -0.5f;
    private float blockStaggerTimer = 2.0f;
    private float blockStagger = 0.0f;

    //Relative Pos, for flipping sprites, to face each other
    private bool flipped = false;
    private float flipTime = 1.0f;
    private float canFlip = 0.0f;
    private float flipSwap = 1;
    public Quaternion ogRot;

    //For collision detection and stuff
    private Coroutine moveCo;
    private float collisionTime = 0.25f;
    private float canStopCollide = 0.0f;
    private float offWall = 0.1f;

    //Getting Character Information
    public float health = 100.0f;
    public Dictionary<int, int>[] moveContainer;
    //array index is for moveType IE. moveContainer[0] is light attack info
    //the key int array contains the damage of the move and if the move spawns a projectile or not
    //the value int array contains the damaging frames of the move

    //Attacking Variables
    public GameObject [] projectiles;
    public bool attacked, isProj, isBuffed = false;
    public bool[] attackType = new bool[4] { false, false, false, false };
    private float projTime = 1.5f;
    private float canProj = 0.0f;
    private float attackTime = 1.0f;
    private float canAttack = 0.0f;
    public int attackPower, attInd, projType = 0;
    private string[] attackNames = { "light", "heavy", "special", "gb" };
    public string attackName = "";

    //Getting Hit Variables
    public bool isHit = false;
    private float blockPadding = 0.5f;
    private float hitStagger = 0.5f;
    private float hitTimer = 0.0f;
    public float deathTime = 0.0f;

    //Round Variables
    public bool playerWon = false;
    public int numWins = 0;

    //round time controls for player input
    public float startTimer = 1.5f;
    public float canStart = 0.0f;

    private void Awake()
    {
        rb = Character.GetComponent<Rigidbody2D>();
        anim = Character.GetComponent<Animator>();
        gcs = groundCheckObj.GetComponent<groundCheck>();

        gameTimer = GameObject.FindGameObjectWithTag("Timer");
        timerSc = gameTimer.GetComponent<Timer>();

        canStopCollide = collisionTime;

        charName = Character.name;
        charName = charName.Substring(0, charName.Length - 7);
        moveContainer = new[]
        {
            new Dictionary<int, int>(),
            new Dictionary<int, int>(),
            new Dictionary<int, int>(),
            new Dictionary<int, int>()
        };

        ReadCharInfo();
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].name == gameObject.name)
        { //Player[0] is this player, so this player is player1
            otherPlayer = players[1];
            dirFace = 1;
        }
        else
        { //Player[0] is not this player, so this player is player2
            otherPlayer = players[0];
            dirFace = -1;
        }
        GetDeathClipTime();
        canStart = startTimer;
        ogRot = transform.rotation;
    }

    public void OnMove(InputAction.CallbackContext context)
    { // Get Move values
        MoveDir = context.ReadValue<Vector2>();
    }

    public void OnLAtt(InputAction.CallbackContext context)
    { // Get light attack value
        if (context.started && attackType[1] == false && attackType[2] == false && attackType[3] == false)
        {
            attInd = 0;
            attackType[attInd] = true;
        }
    }

    public void OnHAtt(InputAction.CallbackContext context)
    { // Get heavy attack value
        if (context.started && attackType[0] == false && attackType[2] == false && attackType[3] == false)
        {
            attInd = 1;
            attackType[attInd] = true;
        }
    }

    public void OnSpAtt(InputAction.CallbackContext context)
    { // Get special attack value
        if (context.started && attackType[0] == false && attackType[1] == false && attackType[3] == false)
        {
            attInd = 2;
            attackType[attInd] = true;
        }
    }

    public void OnShAtt(InputAction.CallbackContext context)
    { // Get sheild break value
        if (context.started && attackType[0] == false && attackType[1] == false && attackType[2] == false)
        {
            attInd = 3;
            attackType[attInd] = true;
        }
    }

    private void FixedUpdate()
    {
        if (canStart <= 0)
        {
            PlayerWins();

            onGround = gcs.onGround;

            anim.SetBool("onGround", onGround);

            if (onGround)
            { //if on ground count down to when they can jump and move on the ground
                canMove -= Time.deltaTime;
                canJump -= Time.deltaTime;
                canFlip -= Time.deltaTime;
                canAttack -= Time.deltaTime;
                blockStagger -= Time.deltaTime;
            }

            canProj -= Time.deltaTime;
            hitTimer -= Time.deltaTime;

            if (isHit && hitTimer <= 0)
            {
                isHit = false;
                anim.SetBool("hit", false);
                anim.SetBool("walkF", false);
                anim.SetBool("walkB", false);
                if (attackName != "")
                {
                    anim.SetBool(attackName, false);
                }
                anim.SetBool("block", false);
                attacked = false;
                attackType[attInd] = false;
            }

            if (attacked)
            { //if the player did an attack
                if (canAttack <= 0)
                { //do the attack until the animation time has ran
                    anim.SetBool(attackName, false);
                    attackType[attInd] = false;
                    attacked = false;
                    if (attackPower == -1)
                    { //Move is a buff
                        isBuffed = true;
                    }
                    if (isProj)
                    {
                        SpawnProj();
                    }
                }
            }

            if (MoveDir.y <= blockThreshold && blockStagger <= 0 && !isHit)
            { //Character is trying to block if they are holding down and they are not staggered from a shield break
                isBlock = true;
            }
            else
            {
                isBlock = false;
            }


            if (!attacked && isBlock) //Character is blocking if they are holding down and are not staggered and not attacking
            {
                anim.SetBool("block", true);
            }
            else
            {
                anim.SetBool("block", false);
            }

            if (canMove <= 0.0f && !isBlock && onGround == true && (MoveDir.x >= 0.5 || MoveDir.x <= -0.5) && MoveDir.y < 0.5 && !attacked && blockStagger <= 0 && !isHit && canProj <= 0) //Character can move if they are not blocking and its been time since the last move
            {
                atPos = false;
                if (MoveDir.x * dirFace > 0)
                {
                    anim.SetBool("walkF", true);
                }
                else
                {
                    anim.SetBool("walkB", true);
                }
                anim.SetBool("hit", false);
                HorizMove();
                canMove = moveTime;
            }
            if (onGround && MoveDir.y >= 0.4 && atPos == true && canJump <= 0.0f && !attacked && blockStagger <= 0 && !isHit && canProj <= 0) //Character can jump if they are on the ground and pressing up
            {
                var curForce = jumpForce - vertDamper * MoveDir.y;
                jumpMoveDir = MoveDir.normalized;
                jumpMoveDir = new Vector2(jumpMoveDir.x * horizontalJump, jumpMoveDir.y * curForce);
                rb.AddForce(jumpMoveDir, ForceMode2D.Impulse);
                canJump = jumpTime;
            }

            if (onGround && otherPlayer.GetComponent<PlayerController>().onGround && canFlip <= 0 && blockStagger <= 0 && !isHit && canProj <= 0)
            {
                RotChar();
            }

            if (onGround && !attacked && atPos && blockStagger <= 0 && canProj <= 0 && !isHit && canProj <= 0)
            { //Can attack if you are on the ground and so much time has passed since the last attack
                Attack();
            }

            NormalizeTimers();
        } else
        {
            canStart -= Time.deltaTime;
            anim.SetBool("onGround", true);
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
        anim.SetBool("walkF", false);
        anim.SetBool("walkB", false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == otherPlayer.name && canStopCollide > 0 && !atPos)
        { //so players don't jankily pass through each other
            StopCoroutine(moveCo);
            atPos = true;
            anim.SetBool("walkF", false);
            anim.SetBool("walkB", false);
            canMove = -1;
            canStopCollide -= Time.deltaTime;
        } else if (collision.gameObject.layer == 9 && !atPos)
        { //so players can't move through a wall
            StopCoroutine(moveCo);
            atPos = true;
            anim.SetBool("walkF", false);
            anim.SetBool("walkB", false);
            var relativePos = (collision.gameObject.transform.position - gameObject.transform.position).normalized.x;
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + relativePos * offWall, gameObject.transform.position.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == otherPlayer.name)
        { //so players can move after not being able to pass through each other
            canStopCollide = collisionTime;
        }
    }

    private void ReadCharInfo() //Gets each character's info from a text file
    {
        int counter = 1;
        int moveNum = 0;
        int thisDam = 0;
        int thisProj = 0;
        string first, second;
        //string path = "Assets/Characters/CharInfo/" + charName + ".txt";
        string path = "SegFight_Data/characters/" + charName + ".txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            if (counter % 2 != 0)
            { //Getting Damage of move, key for Dictionaries
                first = reader.ReadLine();
                thisDam = int.Parse(first);
            } 
            else
            { //Getting if the move is a projectile, val for Dictionaries
                second = reader.ReadLine();
                thisProj = int.Parse(second);
                moveContainer[moveNum].Add(thisDam, thisProj);
                moveNum++;
            }
            counter++;
        }
        reader.Close();
    }
    public void RotChar()
    { // To rotate the characters to make sure they are always facing each other
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
            dirFace = directionScale/flipSwap;
            canFlip = flipTime;
        } else if (players[1].name == gameObject.name && directionScale > 0)
        {
            flipped = !flipped;
            transform.rotation = fixedQuat;
            dirFace = directionScale/flipSwap;
            canFlip = flipTime;
        }
    }

    private void Attack()
    { // Handles figuring out what attack the player wants to do
        for (int i = 0; i < attackType.Length; i++)
        {
            if (attackType[i] == true)
            {
                foreach (var testValue in moveContainer[i])
                {
                    attackPower = testValue.Key;
                    if (testValue.Value <= 0)
                    { //The move is not a projectile
                        isProj = false;
                    }
                    else
                    { //The move is a projectile
                        isProj = true;
                        projType = testValue.Value;
                        canProj = projTime;
                    }
                }
                if (isBuffed)
                {
                    isBuffed = false;
                    attackPower *= 2;
                }
                attacked = true;
                attackName = attackNames[i];
                anim.SetBool(attackName, true);
                UpdateAttackClipTime();
                canAttack = attackTime;
            }
        }
    }

    private void SpawnProj()
    {
        var projDir = 1;
        var spawnOffset = 1.5f;
        if (gameObject.transform.eulerAngles.y == 180)
        {
            projDir = -1;
            spawnOffset = 2;
        }
        GameObject.Instantiate(projectiles[projType - 1], new Vector2(gameObject.transform.position.x + (spawnOffset * projDir), gameObject.transform.position.y + 0.5f), gameObject.transform.rotation);
    }

    public void OnHit(float attackDamage, int moveInd)
    { // what happens when hit
        bool playHit = true;
        if (isBlock && moveInd != 3) // block and no gb
        {
            playHit = false;
            attackDamage -= attackDamage*blockPadding;
        } else if (isBlock && moveInd == 3) // block and gb
        {
            attackDamage -= attackDamage * blockPadding;
            blockStagger = blockStaggerTimer;
            isBlock = false;
        }
        health -= attackDamage;
        isHit = true;
        anim.SetBool("hit", playHit);
        hitTimer = hitStagger;
    }

    private void PlayerWins()
    { //checking who won the round
        if (otherPlayer.GetComponent<PlayerController>().health <= 0)
        { // if the other player has been killed
            numWins++;
            playerWon = true;
        }
    }

    public void UpdateAttackClipTime()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        attackTime = Array.Find(clips, element => element.name[2..] == attackName).length - .1f;
    }

    public void GetDeathClipTime()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        deathTime = Array.Find(clips, element => element.name[2..] == "dead").length * 1.5f;
    }

        private void NormalizeTimers()
    { //Making sure these values don't grow up to an absurd size
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
        if (blockStagger < -1)
        {
            blockStagger = -1;
        }
        if (canProj < -1)
        {
            canProj = -1;
        }
    }

}
