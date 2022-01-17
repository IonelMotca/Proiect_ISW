using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    float grass;
    int sheep;
    int wolves;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Stat", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Stat() {

        var gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        var map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

        // Grass

        var cells = map.GetMap();

        grass = 0;

        foreach (Cell c in cells) {
            grass += c.GetGrass();
        }

        grass /= 1000;

        // Sheep

        sheep = 0;

        foreach (Cell c in cells) {
            sheep += c.GetNumberOfSheepOnCell();
        }

        // Wolves

        wolves = 0;

        foreach (Cell c in cells) {
            wolves += c.GetNumberOfWolvesOnCell();
        }

        var output = "Grass: " + grass + " ton | Sheep: " + sheep + " | Wolves: " + wolves;

        GetComponent<TextMeshProUGUI>().text = output;
    }
}
