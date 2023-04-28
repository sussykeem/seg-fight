using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class button_change : MonoBehaviour
{
    public Button button_blake, button_randy, button_karim, button_turner;

    public void button_press() {
        // button_blake.GetComponent<Image>().color = Color.red;
        GetComponent<Image>().color = Color.green;
    }
    
}