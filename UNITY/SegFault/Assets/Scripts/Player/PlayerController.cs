using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerController : MonoBehaviour
{
    public GameObject Character;
    public string charName;
    
    private Rigidbody2D rb;

    public float canMove = 0.0f;
    public float moveSpeed = 35f;
    public float moveTime = 1.0f;
    public Vector2 MoveDir = Vector2.zero;

    public float jumpHeight = 3.0f;
    public float jumpForce = 0.0f; 
    public bool onGround = false;

    public GameObject groundCheckObj;
    groundCheck gcs;

    public bool isBlock = false;
    public float blockThreshold = -0.5f;

    public bool flipped = false;

    public float health = 0.0f;
    private Dictionary<int, float>[] moveContainer;
    private void Awake()
    {
        rb = Character.GetComponent<Rigidbody2D>();
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale - 10));
        gcs = groundCheckObj.GetComponent<groundCheck>();
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

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        canMove = canMove - Time.deltaTime;
        onGround = gcs.onGround;

        if(MoveDir.y <= blockThreshold) //Character is blocking if they are holding down
        {
            isBlock = true;
        } else
        {
            isBlock = false;
        }
        if (canMove <= 0.0f && !isBlock) //Character can move if they are not blocking and its been time since the last move
        {   
            rb.AddForce(new Vector2(MoveDir.x * moveSpeed, 0), ForceMode2D.Impulse);
            canMove = moveTime;
        }
        if (onGround && MoveDir.y > 0) //Character can jump if they are not on the ground and pressing up
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (transform.position.x == GameObject.FindGameObjectsWithTag("Player")[1].transform.position.x)
        {
            rotChar();
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
    private void rotChar()
    {
        if(!flipped)
        {
            Vector3 eulerRot;
            Quaternion flippedQuat;
            eulerRot = transform.rotation.eulerAngles;
            flippedQuat = Quaternion.Euler(eulerRot.x, eulerRot.y+180, eulerRot.z);
            transform.rotation = flippedQuat;
            flipped = true;
        }
    }
}
