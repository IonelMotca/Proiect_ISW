using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private int i,j;
    
    public Material material0;
    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;

    public float grassGrowth = 8f;

    private float grass;

    private List<Sheep> sheep = new List<Sheep>();
    private List<Wolf> wolves = new List<Wolf>();

    public void Initialize(int i, int j) {
        this.i = i;
        this.j = j;
    }

    public int GetI() {
        return i;
    }

    public int GetJ() {
        return j;
    }

    // Start is called before the first frame update
    void Start()
    {
        grass = 0;

        InvokeRepeating("UpdateGrassColor", 0.0f, 1.0f);
        InvokeRepeating("GrowGrass", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Sheep") {
            var go = collider.gameObject;
            sheep.Add(go.GetComponent<Sheep>());
        }

        if (collider.gameObject.tag == "Wolf") {
            var go = collider.gameObject;
            wolves.Add(go.GetComponent<Wolf>());
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Sheep") {
            var go = collider.gameObject;
            sheep.Remove(go.GetComponent<Sheep>());
        }

        if (collider.gameObject.tag == "Wolf") {
            var go = collider.gameObject;
            wolves.Remove(go.GetComponent<Wolf>());
        }
    }

    public List<Sheep> GetAllSheepOnCell() {
        return sheep;
    }

    public List<Wolf> GetAllWolvesOnCell() {
        return wolves;
    }

    private void UpdateGrassColor() {
        if (grass < grassGrowth * 10)
            GetComponent<SpriteRenderer>().material = material0;
        else if (grass < grassGrowth * 30)
            GetComponent<SpriteRenderer>().material = material1;
        else if (grass < grassGrowth * 90)
            GetComponent<SpriteRenderer>().material = material2;
        else if (grass < grassGrowth * 180)
            GetComponent<SpriteRenderer>().material = material3;
        else if (grass < grassGrowth * 300)
            GetComponent<SpriteRenderer>().material = material4;
        else if (grass <= grassGrowth * 365)
            GetComponent<SpriteRenderer>().material = material5;
    }

    void GrowGrass() {
        if (grass < grassGrowth * 365)
            grass += grassGrowth;
    }

    public float GetGrass() {
        return grass;
    }

    public void EatGrass(float amount) {
        grass -= amount;
    }

    public int GetNumberOfSheepOnCell() {
        return sheep.Count;
    }

    public int GetNumberOfWolvesOnCell() {
        return wolves.Count;
    }
}
