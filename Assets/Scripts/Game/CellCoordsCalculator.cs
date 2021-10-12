using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CellCoordsCalculator
{
    protected Action<int, int, Vector3> onCell;
    protected int width = 10;
    protected int height = 10;
    protected const float x0 = -4.5f;
    protected const float y0 = 0.001f;
    protected const float z0 = 0;

    public int GetWidth() => width;
    public int GetHeight() => height;

    public void SetOnCellAction(Action<int, int, Vector3> _onCell)
    {
        onCell = _onCell;
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

        y = y0;
        cellPosition.y = y;
        cellY = height / 2;
        for (i = 0; i < width; i++)
        {
            //cell = Instantiate(cellPrefab, transform);
            cellX = i;
            x = (float)i + x0;
            z = z0;
            cellPosition.x = x;
            cellPosition.z = z;
            //cell.transform.position = cellPosition;
            onCell(cellX, cellY, cellPosition);
            for (j = 1; j <= cellY; j++)
            {
                //cell = Instantiate(cellPrefab, transform);
                z = z0 + ((float)j);
                cellPosition.z = z;
                //cell.transform.position = cellPosition;
                onCell(cellX, cellY+j, cellPosition);
                //cell = Instantiate(cellPrefab, transform);
                z = z0 - ((float)j);
                cellPosition.z = z;
                //cell.transform.position = cellPosition;
                onCell(cellX, cellY-j, cellPosition);
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

        y = y0;
        cellPosition.y = y;
        cellY = height / 2;
        for (i = 0; i < width; i++)
        {
            cellX = i;
            j = 0;
            //cell = Instantiate(cellPrefab, transform);
            x = (float)i + x0;
            z = z0;
            cellPosition.x = x + ((j % 2) != (_roomRow % 2) ? 0.5f : 0.0f);
            cellPosition.z = z;
            //cell.transform.position = cellPosition;
            onCell(cellX, cellY, cellPosition);

            for (j = 1; j <= cellY; j++)
            {
                cellPosition.x = x + ((j % 2) != (_roomRow % 2) ? 0.5f : 0.0f);

                //cell = Instantiate(cellPrefab, transform);
                z = z0 + ((float)j) * 0.866f;
                cellPosition.z = z;
                //cell.transform.position = cellPosition;
                onCell(cellX, cellY + j, cellPosition);

                //cell = Instantiate(cellPrefab, transform);
                z = z0 - ((float)j) * 0.866f;
                cellPosition.z = z;
                //cell.transform.position = cellPosition;
                onCell(cellX, cellY - j, cellPosition);
            }
        }
    }
}