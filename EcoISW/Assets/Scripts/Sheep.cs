using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float yield = 0.5f;

    public int lifespanDaysMin = 3650;
    public int lifespanDaysMax = 4380;

    public float chanceOfDyingOldAge = 0.0007f;
    public float chanceOfDyingStarvation = 0.1f;

    public float weightGainPerDay = 0.026f;

    public float dailyFoodReq = 1f;

    public int minDaysToStarve = 10;
    public int maxDaysToStarve = 20;

    public int femaleSexualMaturityDays = 549;
    public int maleSexualMaturityDays = 914;
    public int gestationDays = 146;
    public int gestationCooldownDays = 365;

    public float resetTime = 60f;

    private int ageDays = 0;

    private float weight = 5f;

    private int daysWithoutFood = 0;
    private bool isSatiated;

    private bool predatorInCell = false;

    private bool sex;
    private bool gestating = false;
    private bool gestatingCooldown = false;
    private int gestatingDays = 0;
    private int gestatingCooldownDays = 0;

    private static Map map;
    private Cell cell;

    // Start is called before the first frame update
    void Start() {
        map = gameObject.transform.parent.gameObject.GetComponent<Map>();

        sex = GetRandomSex();

        InvokeRepeating("Age", 1.0f, 1.0f);
        InvokeRepeating("OldAgeDeath", 1.0f, 1.0f);
        InvokeRepeating("Eat", 1.0f, 1.0f);
        InvokeRepeating("HungerDeath", 1.0f, 1.0f);
        InvokeRepeating("Reproduce", 1.0f, 1.0f);
        InvokeRepeating("CheckPredator", 1.0f, 1.0f);
        InvokeRepeating("Move", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void CheckPredator() {
        if (cell.GetAllWolvesOnCell().Count > 0 && (Random.value < 0.8f))
            predatorInCell = true;
        else
            predatorInCell = false;
    }

    void Reproduce() {
        if (IsFemaleGestationReady()) {
            foreach(Sheep s in cell.GetAllSheepOnCell()) {
                if(s.IsMaleGestationReady()) {
                    gestating = true;
                    break;
                }
            }
        }

        if (gestating) {
            gestatingDays++;

            if(gestatingDays == gestationDays) {
                GiveBirth();
                gestating = false;
                gestatingDays = 0;
                gestatingCooldown = true;
            }
        }

        if (gestatingCooldown) {
            gestatingCooldownDays++;

            if(gestatingCooldownDays == gestationCooldownDays) {
                gestatingCooldown = false;
                gestatingCooldownDays = 0;
            }
        }
    }

    void GiveBirth() {
        var gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        gm.CreateSheep(1, gameObject.transform.position);
    }

    bool IsFemaleGestationReady() {
        if (sex == false && ageDays >= femaleSexualMaturityDays && gestating == false && gestatingCooldown == false)
            return true;
        return false;
    }

    bool IsMaleGestationReady() {
        if (sex == true && ageDays >= maleSexualMaturityDays)
            return true;
        return false;
    }

    void HungerDeath() {
        if (daysWithoutFood >= maxDaysToStarve)
            Die();
        else if (daysWithoutFood >= minDaysToStarve)
            if (Random.value <= chanceOfDyingStarvation)
                Die();
    }

    void OldAgeDeath() {
        if (ageDays >= lifespanDaysMax)
            Die();
        else if (ageDays >= lifespanDaysMin)
            if (Random.value <= chanceOfDyingOldAge)
                Die();
    }

    void Age() {
        ageDays++;
    }

    void Eat() {

        float grass, foodAmount;

        grass = cell.GetGrass();

        isSatiated = false;

        if (grass >= dailyFoodReq)
            foodAmount = dailyFoodReq;
        else
            foodAmount = grass;

        cell.EatGrass(foodAmount);

        if (foodAmount >= dailyFoodReq) {
            weight += weightGainPerDay;
            isSatiated = true;

            if (daysWithoutFood > 0)
                daysWithoutFood -= 1;
        }
        else {
            daysWithoutFood++;
            weight -= weightGainPerDay;
        }
    }

    void Move() {
        if (!isSatiated || predatorInCell) {
            var neighbouringCells = map.GetNeighbouringCells(cell);
            var mostFoodCell = map.GetCellWithMostFood(neighbouringCells);
            var position = map.GetRandomPositionInCell(mostFoodCell);

            gameObject.transform.position = new Vector3(position.Item1, position.Item2);
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag == "Cell") {
            var go = collider.gameObject;
            cell = go.GetComponent<Cell>();
        }
    }

    public void SetAge(int days) {
        ageDays = days;
        weight = 5f + days * weightGainPerDay;
        daysWithoutFood = 0;
    }

    bool GetRandomSex() {
        if (Random.value <= 0.5f)
            return false;
        return true;
    }

    bool GetSex() {
        return sex;
    }

    public float GetWeight() {
        return weight;
    }

    public void Die() {
        Destroy(gameObject);
    }

    void ResetScale() {
        gameObject.transform.localScale = new Vector3(1, 1, 0);
    }

    void OnMouseDown() {
        gameObject.transform.localScale = new Vector3(2, 2, 0);
        Invoke("ResetScale", resetTime);
    }
}
