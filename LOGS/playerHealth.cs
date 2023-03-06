/*John Turner Heath
 * Simple Player object for the initialization and update of player health
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public int MaximumHealth = 100;
    public int CurrentHealth;

    public healthbar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaximumHealth;
        healthbar.SetMaxHealth(MaximumHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            Damage(10);
        }
    }

    void Damage(int damage) {
        CurrentHealth -= damage;
        healthbar.SetHealth(CurrentHealth);

        if (CurrentHealth == 0)
        {
            Destroy(gameObject);
        }
    }
}