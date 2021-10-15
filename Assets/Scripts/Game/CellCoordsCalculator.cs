using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECellType
{
    none=0, ground=1, stone=2, water=3, ice=4, wood=5
}

public class Cell
{
    private readonly Vector3 position;
    private readonly int number;
    private ECellType baseType;

    public Cell(Vector3 _position, int _number)
    {
        position = _position;
        number = _number;
        baseType = ECellType.none;
    }

    public void SetBaseType(ECellType _type) => baseType = _type;
    public ECellType GetBaseType() => baseType;
    public Vector3 GetPosition() => position;
}
public abstract class CellCoordsCalculator
{
    private Action<Cell> onCell;
    private int startCellInRoom = 0;
    private Cell[] map;

    protected int width = 10;
    protected int height = 10;
    protected int mapWidth = 10;
    protected int mapHeight = 10;
    protected const float x0 = -4.5f;
    protected const float y0 = 0.001f;
    protected const float z0 = 0;

    public int GetWidth() => width;
    public int GetHeight() => height;

    public void SetOnCellAction(Action<Cell> _onCell)
    {
        onCell = _onCell;
    }

    public void CreateMap(int _mapWidth, int _mapHeight)
    {
        mapHeight = _mapHeight;
        mapWidth = _mapWidth;
        map = new Cell[mapHeight * mapWidth * width * height];
    }

    protected void CreateCell(int _x, int _y, Vector3 _position)
    {
        int cellNumber = startCellInRoom + _y * mapWidth * width + _x;
        Cell cell = new Cell(_position, cellNumber);
        map[cellNumber] = cell;
        onCell(cell);
    }

    protected void SetStartCell(int _roomRow, int _roomCol)
    {
        if (_roomRow > mapHeight) Debug.LogError("room row above height!");
        if (_roomCol > mapWidth) Debug.LogError("room col above width!");
        startCellInRoom = _roomRow * mapWidth * width + _roomCol*width;
    }
    public abstract void Build(int _roomRow, int _roomCol);
}

public class CellSquareCalculator : CellCoordsCalculator
{
    public CellSquareCalculator()
    {
        width = 10;
        height = 13;
    }

    public override void Build(int _roomRow, int _roomCol)
    {
        int i, j;
        int cellX, cellY;
        float x, y, z;
        Vector3 cellPosition = new Vector3(0, 0, 0);
        SetStartCell(_roomRow, _roomCol);

        y = y0;
        cellPosition.y = y;
        cellY = height / 2;
        for (i = 0; i < width; i++)
        {
            cellX = i;
            x = (float)i + x0;
            z = z0;
            cellPosition.x = x;
            cellPosition.z = z;
            CreateCell(cellX, cellY, cellPosition);
            for (j = 1; j <= cellY; j++)
            {
                z = z0 + ((float)j);
                cellPosition.z = z;
                CreateCell(cellX, cellY+j, cellPosition);
                z = z0 - ((float)j);
                cellPosition.z = z;
                CreateCell(cellX, cellY-j, cellPosition);
            }
        }
    }
}

public class CellHexCalculator : CellCoordsCalculator
{
    public CellHexCalculator()
    {
        width = 10;
        height = 15;
    }
    public override void Build(int _roomRow, int _roomCol)
    {
        int i, j;
        int cellX, cellY;
        float x, y, z;
        Vector3 cellPosition = new Vector3(0, 0, 0);
        SetStartCell(_roomRow, _roomCol);

        y = y0;
        cellPosition.y = y;
        cellY = height / 2;
        for (i = 0; i < width; i++)
        {
            cellX = i;
            j = 0;
            x = (float)i + x0;
            z = z0;
            cellPosition.x = x + ((j % 2) != (_roomRow % 2) ? 0.5f : 0.0f);
            cellPosition.z = z;
            CreateCell(cellX, cellY, cellPosition);

            for (j = 1; j <= cellY; j++)
            {
                cellPosition.x = x + ((j % 2) != (_roomRow % 2) ? 0.5f : 0.0f);

                z = z0 + ((float)j) * 0.866f;
                cellPosition.z = z;
                CreateCell(cellX, cellY + j, cellPosition);

                z = z0 - ((float)j) * 0.866f;
                cellPosition.z = z;
                CreateCell(cellX, cellY - j, cellPosition);
            }
        }
    }
}