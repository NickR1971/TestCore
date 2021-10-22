using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMapDirection
{
    center=0, north=1, northeast=2, east=3, southeast=4, south=5, southwest=6, west=7, northwest=8
}

public class Cell
{
    private readonly Vector3 position;
    private readonly int number;
    private GameObject obj;
    private ECellType baseType;
    private int[] nearList;
    private CRoom room;

    public Cell(Vector3 _position, int _number, int[] _nearList)
    {
        position = _position;
        number = _number;
        baseType = ECellType.none;
        nearList = _nearList;
    }

    public void SetBaseType(ECellType _type) => baseType = _type;
    public ECellType GetBaseType() => baseType;
    public Vector3 GetPosition() => position + room.GetPosition();
    public int GetNearNumber(EMapDirection _direction) => nearList[(int)_direction];
    public void AddRoom(CRoom _room) => room = _room;
    public CRoom GetRoom() => room;

    public void SetObject(GameObject _obj)
    {
        obj = _obj;
        obj.transform.position = position;
    }

    public void SetColor(Color _color)
    {
        obj.GetComponent<Renderer>().material.color = _color;
    }
}

public interface IGameMap
{
    int GetRoomWidth();
    int GetRoomHeight();
    int GetWidth();
    int GetHeight();
    Cell GetCell(int _number);
}
public abstract class CellCoordsCalculator : IGameMap
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
    protected int[] xList = new int[9];
    protected int[] yList = new int[9];

    public CellCoordsCalculator()
    {
        int n = (int)EMapDirection.center;
        xList[n] = 0; yList[n] = 0;
        n = (int)EMapDirection.north;
        xList[n] = 0; yList[n] = 1;
        n = (int)EMapDirection.northeast;
        xList[n] = 1; yList[n] = 1;
        n = (int)EMapDirection.east;
        xList[n] = 1; yList[n] = 0;
        n = (int)EMapDirection.southeast;
        xList[n] = 1; yList[n] = -1;
        n = (int)EMapDirection.south;
        xList[n] = 0; yList[n] = -1;
        n = (int)EMapDirection.southwest;
        xList[n] = -1; yList[n] = -1;
        n = (int)EMapDirection.west;
        xList[n] = -1; yList[n] = 0;
        n = (int)EMapDirection.northwest;
        xList[n] = -1; yList[n] = 1;
    }

    public int GetRoomWidth() => width;
    public int GetRoomHeight() => height;
    public int GetWidth() => width * mapWidth;
    public int GetHeight() => height * mapHeight;
    public Cell GetCell(int _number)
    {
        if (_number >= map.Length) return null;

        return map[_number];
    }

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

    private int CalcNum(int _x, int _y)
    {
        return startCellInRoom + _y * mapWidth * width + _x;
    }

    private Color GetSurfaceColor(ECellType _surfaceType)
    {
        switch(_surfaceType)
        {
            case ECellType.ground:
                return Color.green;
            case ECellType.stone:
                return Color.gray;
            case ECellType.water:
                return Color.blue;
            default:
                return Color.white;
        }
    }
    private bool CheckSurface(Vector3 _position, out Vector3 _result, out ECellType _surfaceType)
    {
        Vector3 start;
        Vector3 dir = new Vector3(0, -1, 0);
        RaycastHit hit;

        _result = _position;
        _surfaceType = ECellType.none;

        start = _position;
        start.y = 50.0f;
        if (Physics.Raycast(start, dir, out hit, 100.0f))
        {
            _result = hit.point;
            _result.y += y0; // -------------------------- ??
            GameObject obj = hit.collider.gameObject;
            ISurface surface = obj.GetComponent<ISurface>();
            if (surface != null) _surfaceType = surface.GetCellType();
        }
        else return false;

        return true;
    }
    protected void CreateCell(int _x, int _y, Vector3 _position)
    {
        int cellNumber = CalcNum(_x, _y);
        int[] nearList = new int[9];
        for (int i = 0; i < 9; i++) nearList[i] = CalcNum(_x + xList[i], _y + yList[i]);

        Vector3 position;
        ECellType surfaceType;
        if (CheckSurface(_position, out position, out surfaceType))
        {
            Cell cell = new Cell(_position, cellNumber, nearList);
            map[cellNumber] = cell;
            cell.SetBaseType(surfaceType);
            onCell(cell);
            cell.SetColor(GetSurfaceColor(surfaceType));
        }
    }

    protected void SetStartCell(int _roomRow, int _roomCol)
    {
        if (_roomRow > mapHeight) Debug.LogError("room row above height!");
        if (_roomCol > mapWidth) Debug.LogError("room col above width!");
        startCellInRoom = _roomRow * mapWidth * width * height + _roomCol * width;
        Debug.Log($"Row={_roomRow} Col={_roomCol} : Start number is {startCellInRoom}");
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

    private void CorrectNearList(float _offset)
    {
        int n;
        if (_offset < 0.1f)
        {
            n = (int)EMapDirection.northeast;
            xList[n] = 0;
            n = (int)EMapDirection.southeast;
            xList[n] = 0;
             n = (int)EMapDirection.northwest;
            xList[n] = -1;
            n = (int)EMapDirection.southwest;
            xList[n] = -1;
       }
        else
        {
            n = (int)EMapDirection.northeast;
            xList[n] = 1;
            n = (int)EMapDirection.southeast;
            xList[n] = 1;
             n = (int)EMapDirection.northwest;
            xList[n] = 0;
            n = (int)EMapDirection.southwest;
            xList[n] = 0;
        }
    }
    public override void Build(int _roomRow, int _roomCol)
    {
        int i, j;
        int cellX, cellY;
        float x, y, z;
        float offset;
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
            offset = ((j % 2) != (_roomRow % 2) ? 0.5f : 0.0f);
            cellPosition.x = x + offset;
            cellPosition.z = z;
            CorrectNearList(offset);
            CreateCell(cellX, cellY, cellPosition);

            for (j = 1; j <= cellY; j++)
            {
                offset = ((j % 2) != (_roomRow % 2) ? 0.5f : 0.0f);
                CorrectNearList(offset);
                cellPosition.x = x + offset;

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