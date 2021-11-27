using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRoom : MonoBehaviour
{
    [SerializeField] private GameObject wallNorth;
    [SerializeField] private GameObject wallSouth;
    [SerializeField] private GameObject wallWest;
    [SerializeField] private GameObject wallEast;
    private IDungeon dungeon = null;
    private GameObject cellPrefab = null;
    private CellCoordsCalculator cellCalculator;
    private int row;
    private int col;
    private const int centerCol = 5;
    private const int centerRow = 5;
    private const float roomSizeX = 10.0f;
    private const float roomSizeZ = 13.0f;
    private bool isFreeNorth = true;
    private bool isFreeSouth = true;
    private bool isFreeWest = true;
    private bool isFreeEast = true;

    public bool IsFreeNorth() { return isFreeNorth; }
    public bool IsFreeSouth() { return isFreeSouth; }
    public bool IsFreeWest() { return isFreeWest; }
    public bool IsFreeEast() { return isFreeEast; }

    private void Start()
    {
        if (dungeon == null) Debug.Log("Not init CRoom before start!");
        //cellCalculator.Build(row, col, basePosition);
    }

    private void OnCell(Cell _cell)
    {
        _cell.AddRoom(this);
        _cell.SetObject(Instantiate(cellPrefab, transform));
    }

    public CRoom Init(IDungeon _dungeon, GameObject _cellPrefab, CellCoordsCalculator _cellCalculator)
    {
        dungeon = _dungeon;
        cellPrefab = _cellPrefab;
        cellCalculator = _cellCalculator;
        cellCalculator.SetOnCellAction(OnCell);

        return this;
    }

    public static Vector3 CalcPosition(int _col, int _row) => new Vector3((float)(_col - centerCol) * roomSizeX, 0, (float)(_row - centerRow) * roomSizeZ);

    public CRoom SetBasePosition(int _col, int _row)
    {
        row = _row; col = _col;

        return this;
    }

    public int GetRow() => row;
    public int GetCol() => col;

    public CRoom SetWalls(bool _north, bool _south, bool _west, bool _east)
    {
        wallNorth.SetActive(_north);
        wallSouth.SetActive(_south);
        wallWest.SetActive(_west);
        wallEast.SetActive(_east);

        isFreeNorth = !_north;
        isFreeSouth = !_south;
        isFreeWest = !_west;
        isFreeEast = !_east;

        return this;
    }

    public Vector3 GetPosition() => transform.position;
}
