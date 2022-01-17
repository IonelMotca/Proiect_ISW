using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int startingNumberOfSheep = 100;
    public int startingNumberOfWolves = 10;

    public int sheepStartingAgeDays = 2000;
    public int wolfStartingAgeDays = 2000;

    public int wolvesAddingDelayDays = 15;

    public GameObject sheep;
    public GameObject wolf;

    private GameObject map;

    // Start is called before the first frame update
    void Start() {
        map = GameObject.FindWithTag("Map");

        Invoke("InstantiateSheep", 1f);
        Invoke("InstantiateWolves", wolvesAddingDelayDays);
    }

    // Update is called once per frame
    void Update() {

    }

    void InstantiateSheep() {
        for (int i = 0; i < startingNumberOfSheep; i++) {
            GameObject go = Instantiate(sheep, new Vector2(0, 0), Quaternion.identity);
            go.transform.parent = map.transform;

            Cell midCell = map.GetComponent<Map>().GetCellInMidOfMap();
            var position = map.GetComponent<Map>().GetRandomPositionInCell(midCell);

            go.transform.position = new Vector3(position.Item1, position.Item2);

            go.GetComponent<Sheep>().SetAge(sheepStartingAgeDays);
        }
    }

    void InstantiateWolves() {
        for (int i = 0; i < startingNumberOfWolves; i++) {
            GameObject go = Instantiate(wolf, new Vector2(0, 0), Quaternion.identity);
            go.transform.parent = map.transform;

            Cell randomCell = map.GetComponent<Map>().GetRandomCell();
            var position = map.GetComponent<Map>().GetRandomPositionInCell(randomCell);

            go.transform.position = new Vector3(position.Item1, position.Item2);

            go.GetComponent<Wolf>().SetAge(wolfStartingAgeDays);
        }
    }

    public  void CreateSheep(int nr, Vector3 pos) {
        for (int i = 0; i < nr; i++) {
            GameObject go = Instantiate(sheep, new Vector2(0, 0), Quaternion.identity);
            go.transform.parent = map.transform;

            go.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }
    }

    public void CreateWolf(int nr, Vector3 pos) {
        for (int i = 0; i < nr; i++) {
            GameObject go = Instantiate(wolf, new Vector2(0, 0), Quaternion.identity);
            go.transform.parent = map.transform;

            go.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }
    }
}
