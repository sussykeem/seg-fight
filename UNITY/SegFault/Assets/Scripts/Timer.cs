using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float roundTime = 90.0f;
    public float timer = 0.0f;
    public int toScreenInt = 0;
    public TextMeshProUGUI timerBox;

    public void Start()
    {
        timer = roundTime;
    }

    private void FixedUpdate()
    {
        if(timer <= 0)
        {
            timer = 0;
        }
        timer -= Time.deltaTime;
        toScreenInt = Mathf.CeilToInt(timer);
        timerBox.text = toScreenInt.ToString();
    }

    public float getTime()
    {
        return timer;
    }
}
