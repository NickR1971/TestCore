using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERoomSide { free, wall, door, passage };

public class CRoom : MonoBehaviour
{
    private IDungeon dungeon = null;
    private GameObject cellPrefab = null;
    private CellCoordsCalculator cellCalculator;
    private Vector3 basePosition;
    private int row = 5;
    private int col = 5;
    private const float roomSizeX = 10.0f;
    private const float roomSizeZ = 13.0f;
    private ERoomSide sideNorth;
    private ERoomSide sideSouth;
    private ERoomSide sideEast;
    private ERoomSide sideWest;

    private void Start()
    {
        if (dungeon == null) Debug.Log("Not init CRoom before start!");
        
        transform.position = basePosition;
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
        basePosition = Vector3.zero;
        cellCalculator = _cellCalculator;
        cellCalculator.SetOnCellAction(OnCell);
        
        return this;
    }

    public CRoom SetBasePosition(int _col, int _row)
    {
        row = _row; col = _col;
        basePosition = new Vector3((float)(_col - 5) * roomSizeX, 0, (float)(_row - 5) * roomSizeZ);

        return this;
    }

    public void Build()
    {
        cellCalculator.Build(row, col);
    }

    public Vector3 GetPosition() => transform.position;
}
