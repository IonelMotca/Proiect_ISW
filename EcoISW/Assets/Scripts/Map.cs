using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;

    public float cellSize;

    public GameObject cell;

    private Cell[,] map;

    // Start is called before the first frame update
    void Start() {

        map = new Cell[mapHeight, mapWidth];

        InstantiateCells();
    }

    // Update is called once per frame
    void Update() {

    }

    void InstantiateCells() {

        float xPos, yPos, startXPos, startYPos;

        xPos = (0 - mapWidth / 2) * cellSize;
        yPos = (0 - mapHeight / 2) * cellSize;

        startXPos = xPos;
        startYPos = yPos;

        for (int i = 0; i < mapHeight; i++) {
            for (int j = 0; j < mapWidth; j++) {
                GameObject go = Instantiate(cell, new Vector2(xPos, yPos), Quaternion.identity);
                go.transform.parent = transform;
                go.transform.localScale = new Vector3(cellSize, cellSize, 1);

                Cell comp = go.GetComponent<Cell>();
                map[i, j] = comp;
                comp.Initialize(i, j);

                xPos += cellSize;
            }
            yPos += cellSize;
            xPos = startXPos;
        }
    }

    public List<Cell> GetNeighbouringCells(Cell cell) {
        List<Cell> neighbouringCells = new List<Cell>();
        Cell westCell, northCell, eastCell, southCell;
        int i, j;

        i = cell.GetI();
        j = cell.GetJ();

        if (j > 0) {
            westCell = map[i, j - 1];
            neighbouringCells.Add(westCell);
        }
        if (i > 0) {
            northCell = map[i - 1, j];
            neighbouringCells.Add(northCell);
        }
        if (j < mapWidth - 1) {
            eastCell = map[i, j + 1];
            neighbouringCells.Add(eastCell);
        }
        if (i < mapHeight - 1) {
            southCell = map[i + 1, j];
            neighbouringCells.Add(southCell);
        }

        return neighbouringCells;
    }

    public Cell GetCellWithMostFood(List<Cell> cells) {
        Cell randomCell = cells[Random.Range(0, cells.Count)];

        foreach (var cell in cells) {
            if (cell.GetGrass() > randomCell.GetGrass())
                randomCell = cell;
        }
        return randomCell;
    }

    public Cell GetCellWithMostSheep(List<Cell> cells) {
        Cell randomCell = cells[Random.Range(0, cells.Count)];

        foreach (var cell in cells) {
            if (cell.GetNumberOfSheepOnCell() > randomCell.GetNumberOfSheepOnCell())
                randomCell = cell;
        }
        return randomCell;
    }

    public (float, float) GetRandomPositionInCell(Cell cell) {
        float x, y, randomX, randomY;

        randomX = Random.Range(0 - cellSize / 2 + 0.6f, cellSize / 2 - 0.6f);
        randomY = Random.Range(0 - cellSize / 2 + 0.6f, cellSize / 2 - 0.6f);

        x = cell.transform.position.x + randomX;
        y = cell.transform.position.y + randomY;

        return (x, y);
    }

    public Cell GetCellInMidOfMap() {
        return map[mapHeight / 2, mapWidth / 2];
    }

    public Cell GetRandomCell() {
        var x = Random.Range(0, mapHeight);
        var y = Random.Range(0, mapWidth);

        return map[x, y];
    }

    public Cell[,] GetMap() {
        return map;
    }
}
