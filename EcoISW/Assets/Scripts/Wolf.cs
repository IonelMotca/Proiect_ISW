using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public int lifespanDaysMin = 2920;
    public int lifespanDaysMax = 4380;

    public float chanceOfDyingOldAge = 0.0007f;
    public float chanceOfDyingStarvation = 0.1f;

    public float weightGainPerDay = 0.017f;

    public float dailyFoodReq = 5f;

    public int minDaysToStarve = 10;
    public int maxDaysToStarve = 20;

    public int femaleSexualMaturityDays = 669;
    public int maleSexualMaturityDays = 669;
    public int gestationDays = 109;
    public int gestationCooldownDays = 365;

    public float resetTime = 60f;

    private int ageDays = 47;

    private float weight = 5f;

    private int daysWithoutFood = 0;
    private bool isSatiated = false;

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
        InvokeRepeating("Move", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void Reproduce() {
        if (IsFemaleGestationReady()) {
            foreach (Wolf w in cell.GetAllWolvesOnCell()) {
                if (w.IsMaleGestationReady()) {
                    gestating = true;
                    break;
                }
            }
        }

        if (gestating) {
            gestatingDays++;

            if (gestatingDays == gestationDays) {
                GiveBirth();
                gestating = false;
                gestatingDays = 0;
                gestatingCooldown = true;
            }
        }

        if (gestatingCooldown) {
            gestatingCooldownDays++;

            if (gestatingCooldownDays == gestationCooldownDays) {
                gestatingCooldown = false;
                gestatingCooldownDays = 0;
            }
        }
    }

    void GiveBirth() {
        var gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        gm.CreateWolf(6, gameObject.transform.position);
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

        int numberOfSheep;
        float yield = 0;

        numberOfSheep = cell.GetNumberOfSheepOnCell();

        if (numberOfSheep > 0 && isSatiated == false) {
            var sheep = cell.GetAllSheepOnCell()[Random.Range(0, cell.GetAllSheepOnCell().Count)];
            yield = sheep.GetWeight() * sheep.yield;
            sheep.Die();
        }

        if (yield > 0) {
            daysWithoutFood -= (int)(yield / dailyFoodReq);
        }
        else {
            daysWithoutFood++;
        }

        if (daysWithoutFood <= 0) {
            isSatiated = true;
            weight += weightGainPerDay;
        }
        else {
            isSatiated = false;
            weight -= weightGainPerDay;
        }
    }

    void Move() {
        if (!isSatiated) {
            var neighbouringCells = map.GetNeighbouringCells(cell);
            var mostSheepCell = map.GetCellWithMostSheep(neighbouringCells);
            var position = map.GetRandomPositionInCell(mostSheepCell);

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
        weight = 5f + (days - 47) * weightGainPerDay;
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

    void Die() {
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
