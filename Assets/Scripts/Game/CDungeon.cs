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

    private void Awake()
    {
        AllServices.Container.Register<IDungeon>(this);
        cellCalculator = new CellHexCalculator();
        //cellCalculator = new CellSquareCalculator();

        cellCalculator.CreateMap(10, 10);
    }

    private void Start()
    {
        dialog = AllServices.Container.Get<IDialog>();
        gameConsole = AllServices.Container.Get<IGameConsole>();
        if (data != null) BuildGame();
    }

    private void BuildGame()
    {
        buildSequence = new CRand(data.id);
        Instantiate(floorPrefab, transform).GetComponent<CRoom>()
            .Init(this, cellPrefab, cellCalculator)
            .Build();
        Instantiate(floorPrefab, transform).GetComponent<CRoom>()
            .Init(this, cellPrefab, cellCalculator)
            .SetBasePosition(5, 6)
            .Build();
        Instantiate(floorPrefab, transform).GetComponent<CRoom>()
            .Init(this, cellPrefab, cellCalculator)
            .SetBasePosition(6,5)
            .Build();
        Instantiate(floorPrefab, transform).GetComponent<CRoom>()
            .Init(this, cellPrefab, cellCalculator)
            .SetBasePosition(6,6)
            .Build();
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
        return (int)buildSequence.Dice(_max);
    }

    public IGameMap GetGameMap()
    {
        return cellCalculator;
    }
}
