using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timer = 10f;
    public int toScreenInt = 0;
    public TextMeshProUGUI timerBox;

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
