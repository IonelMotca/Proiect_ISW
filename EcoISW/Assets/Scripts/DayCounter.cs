using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    private int day, year;
    private string output;

    // Start is called before the first frame update
    void Start()
    {
        day = 0;
        year = 1;

        InvokeRepeating("AddDay", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddDay() {
        day++;

        if(day == 366) {
            day = 1;
            year++;
        }

        output = "d " + day + "  " + "y " + year;

        GetComponent<TextMeshProUGUI>().text = output;
    }
}
