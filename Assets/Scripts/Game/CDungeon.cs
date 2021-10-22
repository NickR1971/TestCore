using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeon : IService
{
    void Create(SaveData _data);
    int GetSequenceNumber(uint _max);
    IGameMap GetGameMap();
}

public class CDungeon : MonoBehaviour, IDungeon
{
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject cellPrefab;
    private IDialog dialog = null;
    private IGameConsole gameConsole = null;
    private SaveData data = null;
    private CRand buildSequence;
    private CellCoordsCalculator cellCalculator;
    private CRoom[] map;
    private int mapWidth = 10;
    private int mapHeight = 10;

    private void Awake()
    {
        AllServices.Container.Register<IDungeon>(this);
        cellCalculator = new CellHexCalculator();
        //cellCalculator = new CellSquareCalculator();
        map = new CRoom[mapWidth * mapHeight];
        cellCalculator.CreateMap(mapWidth, mapHeight);
    }

    private void Start()
    {
        dialog = AllServices.Container.Get<IDialog>();
        gameConsole = AllServices.Container.Get<IGameConsole>();
        if (data != null) BuildGame();
    }

    private int GetRoomNumber(int _x, int _y) => _y * mapWidth + _x;

    private bool CheckPosition(int _x, int _y)
    {
        if (_x < 1 || _y < 1) return false;
        if (_x > (mapWidth - 2) || _y > (mapHeight - 2)) return false; // don't use border rooms
        if (map[GetRoomNumber(_x, _y)] != null) return false;
        return true;
    }

    private bool CreateRoom(int _x, int _y)
    {
        if (CheckPosition(_x, _y) == false) return false;

        int roomNumber = GetRoomNumber(_x, _y);

        map[roomNumber] = Instantiate(floorPrefab, transform).GetComponent<CRoom>();
        map[roomNumber]
            .Init(this, cellPrefab, cellCalculator)
            .SetBasePosition(_x, _y)
            .Build();
        return true;
    }

    private int GetFreeNearList(int _x, int _y, out EMapDirection[] _freedir)
    {
        int n = 0;
        _freedir = new EMapDirection[4];

        if (CheckPosition(_x + 1, _y))
        {
            _freedir[n++] = EMapDirection.east;
        }

        if (CheckPosition(_x - 1, _y))
        {
            _freedir[n++] = EMapDirection.west;
        }

        if (CheckPosition(_x, _y + 1))
        {
            _freedir[n++] = EMapDirection.north;
        }

        if (CheckPosition(_x, _y - 1))
        {
            _freedir[n++] = EMapDirection.south;
        }
        Debug.Log($"Check x={_x} y={_y} n={n}");
        return n;
    }

    private int GenerateMapFrom(int _x,int _y, int _number)
    {
        EMapDirection[] dirFree;
        EMapDirection dir;
        int n, x, y;

        if ( !CreateRoom(_x, _y))  { Debug.LogError($"Can't creat room at x={_x} y={_y}!");  }
        _number--;
        while (_number > 0)
        {
            n = GetFreeNearList(_x, _y, out dirFree);

            if (n == 0) return _number;
            if (n == 1) dir = dirFree[0];
            else dir = dirFree[GetSequenceNumber((uint)n)];
            x = _x; y = _y;
            switch (dir)
            {
                case EMapDirection.north:
                    y++;
                    break;
                case EMapDirection.south:
                    y--;
                    break;
                case EMapDirection.east:
                    x++;
                    break;
                case EMapDirection.west:
                    x--;
                    break;
            }
            _number = GenerateMapFrom(x, y, _number);
        }
        return 0;
    }

    private void BuildGame()
    {
        buildSequence = new CRand(data.id);

        GenerateMapFrom(5, 5, 5);

    }

    //---------------------------------
    // IDungeon
    //---------------------------------
    public void Create(SaveData _data)
    {
        data = _data;
        if (dialog != null) BuildGame();
    }

    public int GetSequenceNumber(uint _max)
    {
        if (buildSequence == null)
        {
            Debug.LogError("Build sequence not initialized!");
            return 0;
        }
        return (int)buildSequence.Dice(_max)-1;
    }

    public IGameMap GetGameMap()
    {
        return cellCalculator;
    }
}
