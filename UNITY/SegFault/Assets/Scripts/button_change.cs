using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class button_change : MonoBehaviour
{
    int counter = 0;
    public Button button_blake, button_randy, button_karim, button_turner;
    EventSystem EventSystem = EventSystem.current;

    public void button_press(Button button) {
        if (counter == 0) { 
        button.GetComponent<Image>().color = Color.red;
            ColorBlock colorVar = button.colors;
            //colorVar.highlightedColor = new Color(0, 0, 255);
            colorVar.selectedColor = new Color(0, 0, 255);
            button_blake.colors = colorVar;
            button_randy.colors = colorVar;
            button_karim.colors = colorVar;
            button_turner.colors = colorVar;
            PlayerPrefs.SetString("Player 1", button.name);
            counter++;

        }
        else if (counter == 1 && button.GetComponent<Image>().color != Color.red) {
            button.GetComponent<Image>().color = Color.blue;
            ColorBlock colorVar = button.colors;
            colorVar.highlightedColor = new Color(255, 255, 255);
            colorVar.selectedColor = new Color(255, 255, 255);
            button_blake.colors = colorVar;
            button_randy.colors = colorVar;
            button_karim.colors = colorVar;
            button_turner.colors = colorVar;
            counter++;
            PlayerPrefs.SetString("Player 2", button.name);
            SceneManager.LoadScene("mapSelect");

        }
        else
        { }
    }
    
}