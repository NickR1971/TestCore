using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMap
{
    int GetRoomWidth();
    int GetRoomHeight();
    int GetWidth();
    int GetHeight();
    Cell GetCell(int _number);
    Cell GetCell(int _x, int _y);
    Vector3 GetDirectionVector(EMapDirection _direction);
}
public abstract class CellCoordsCalculator : IGameMap
{
    private Action<Cell> onCreateCell;
    private int startCellInRoom = 0;
    private Cell[] map;

    protected readonly int width;
    protected readonly int height;
    protected int mapWidth;
    protected int mapHeight;
    protected const float x0 = 0.45f;
    protected const float y0 = 0.0001f;
    protected const float z0 = 0.5f;
    protected int[] xNearList = new int[9];
    protected int[] yNearList = new int[9];
    protected Vector3[] direction = new Vector3[9];

    public CellCoordsCalculator(int _width, int _height)
    {
        width = _width; height = _height;

        xNearList[(int)EMapDirection.center] = 0;
        yNearList[(int)EMapDirection.center] = 0;
        xNearList[(int)EMapDirection.north] = 0;
        yNearList[(int)EMapDirection.north] = 1;
        xNearList[(int)EMapDirection.northeast] = 1;
        yNearList[(int)EMapDirection.northeast] = 1;
        xNearList[(int)EMapDirection.east] = 1;
        yNearList[(int)EMapDirection.east] = 0;
        xNearList[(int)EMapDirection.southeast] = 1;
        yNearList[(int)EMapDirection.southeast] = -1;
        xNearList[(int)EMapDirection.south] = 0;
        yNearList[(int)EMapDirection.south] = -1;
        xNearList[(int)EMapDirection.southwest] = -1;
        yNearList[(int)EMapDirection.southwest] = -1;
        xNearList[(int)EMapDirection.west] = -1;
        yNearList[(int)EMapDirection.west] = 0;
        xNearList[(int)EMapDirection.northwest] = -1;
        yNearList[(int)EMapDirection.northwest] = 1;
        direction[(int)EMapDirection.center] = new Vector3(0, -1, 0);
        direction[(int)EMapDirection.north] = new Vector3(0, -1, 1);
        direction[(int)EMapDirection.northeast] = new Vector3(1, -1, 1);
        direction[(int)EMapDirection.east] = new Vector3(1, -1, 0);
        direction[(int)EMapDirection.southeast] = new Vector3(1, -1, -1);
        direction[(int)EMapDirection.south] = new Vector3(0, -1, -1);
        direction[(int)EMapDirection.southwest] = new Vector3(-1, -1, -1);
        direction[(int)EMapDirection.west] = new Vector3(-1, -1, 0);
        direction[(int)EMapDirection.northwest] = new Vector3(-1, -1, 1);
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
    public Cell GetCell(int _x, int _y)
    {
        if (_x < 0)
        {
            Debug.Log($"Error cell cordinates: x={_x}? : y={_y}");
            return null;
        }
        if (_x >= GetWidth())
        {
            Debug.Log($"Error cell cordinates: x={_x}? : y={_y}");
            return null;
        }

        if (_y < 0)
        {
            Debug.Log($"Error cell cordinates: x={_x} : y={_y}?");
            return null;
        }

        if (_y >= GetHeight())
        {
            Debug.Log($"Error cell cordinates: x={_x} : y={_y}?");
            return null;
        }

        return GetCell(_y * GetWidth() + _x);
    }

    public void SetOnCreateCellAction(Action<Cell> _onCell)
    {
        onCreateCell = _onCell;
    }

    public void CreateMap(int _width, int _height)
    {
        mapWidth = _width;
        mapHeight = _height;
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

        _result = _position;
        _surfaceType = ECellType.none;

        start = _position;
        start.y = 50.0f;
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 100.0f))
        {
            _result = hit.point;
            _result.y += y0;
            GameObject obj = hit.collider.gameObject;
            ISurface surface = obj.GetComponent<ISurface>();
            if (surface != null) _surfaceType = surface.GetCellType();
            return true;
        }
        else Debug.Log($"not found cell at {_position}");
        return false;
    }
    protected void CreateCell(int _x, int _y, Vector3 _position)
    {
        int cellNumber = CalcNum(_x, _y);
        int[] nearList = new int[9];
        for (int i = 0; i < 9; i++) nearList[i] = CalcNum(_x + xNearList[i], _y + yNearList[i]);

        if (CheckSurface(_position, out Vector3 position, out ECellType surfaceType))
        {
            Cell cell = new Cell(position, cellNumber, nearList);
            map[cellNumber] = cell;
            cell.SetBaseType(surfaceType);
            onCreateCell(cell);
            cell.SetColor(GetSurfaceColor(surfaceType));
        }
    }

    protected void SetStartCell(int _roomRow, int _roomCol)
    {
        if (_roomRow > mapHeight) Debug.LogError("room row above height!");
        if (_roomCol > mapWidth) Debug.LogError("room col above width!");
        startCellInRoom = _roomRow * mapWidth * width * height + _roomCol * width;
        //Debug.Log($"Row={_roomRow} Col={_roomCol} : Start number is {startCellInRoom}");
    }
    public abstract void Build(int _roomCol, int _roomRow, Vector3 _basePosition);

    public Vector3 GetDirectionVector(EMapDirection _direction)
    {
        return direction[(int)_direction];
    }
}

public class CellSquareCalculator : CellCoordsCalculator
{
    public CellSquareCalculator() : base(10,13)
    {
    }

    public override void Build(int _roomCol, int _roomRow, Vector3 _basePosition)
    {
        Vector3 cellPosition = Vector3.zero;
        SetStartCell(_roomRow, _roomCol);

        _basePosition.x -= CRoom.GetSizeX() / 2.0f;
        _basePosition.z -= CRoom.GetSizeZ() / 2.0f;

        cellPosition.y = y0;

        for (int col = 0; col < width; col++)
        {
            cellPosition.x = col + x0;
            for (int row = 0; row < height; row++)
            {
                cellPosition.z = row + z0;
                CreateCell(col, row, cellPosition + _basePosition);
            }
        }
    }
}

public class CellHexCalculator : CellCoordsCalculator
{
    public CellHexCalculator() : base(10,15)
    {
        direction[(int)EMapDirection.northeast] = new Vector3(0.866f, -1, 0.5f);
        direction[(int)EMapDirection.southeast] = new Vector3(0.866f, -1, -0.5f);
        direction[(int)EMapDirection.southwest] = new Vector3(-0.866f, -1, -0.5f);
        direction[(int)EMapDirection.northwest] = new Vector3(-0.866f, -1, 0.5f);
    }

    private void CorrectNearList(bool _isOddRow)
    {
        if (_isOddRow)
        {
            xNearList[(int)EMapDirection.northeast] = 0;
            xNearList[(int)EMapDirection.southeast] = 0;
            xNearList[(int)EMapDirection.northwest] = -1;
            xNearList[(int)EMapDirection.southwest] = -1;
       }
        else
        {
            xNearList[(int)EMapDirection.northeast] = 1;
            xNearList[(int)EMapDirection.southeast] = 1;
            xNearList[(int)EMapDirection.northwest] = 0;
            xNearList[(int)EMapDirection.southwest] = 0;
        }
    }
    public override void Build(int _roomCol, int _roomRow, Vector3 _basePosition)
    {
        Vector3 cellPosition = new Vector3(0, 0, 0);
        SetStartCell(_roomRow, _roomCol);

        _basePosition.x -= CRoom.GetSizeX() / 2.0f;
        _basePosition.z -= CRoom.GetSizeZ() / 2.0f;

        cellPosition.y = y0;
        bool isOddRow = ((_roomRow * height) % 2) == 0;

        for (int row = 0; row < height; row++)
        {
            float offset;
            offset = isOddRow ? 0 : 0.5f;
            CorrectNearList(isOddRow);
            cellPosition.z = row*0.866f + z0;
            for (int col = 0; col < width; col++)
            {
                cellPosition.x = col + x0 + offset;
                CreateCell(col, row, cellPosition + _basePosition);
            }
            isOddRow = !isOddRow;
        }
    }
}