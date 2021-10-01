using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeon : IService
{
    void Create(SaveData _data);
}

public class CDungeon : MonoBehaviour, IDungeon
{
    private IDialog dialog = null;
    private IGameConsole gameConsole = null;
    private SaveData data = null;
    private CRand buildSequence;

    private void Awake()
    {
        AllServices.Container.Register<IDungeon>(this);
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
    }

    //---------------------------------
    // IDungeon
     //---------------------------------
   public void Create(SaveData _data)
    {
        data = _data;
        if (dialog != null) BuildGame();
    }
}
