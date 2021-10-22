using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ECellType
{
    none = 0, ground = 1, stone = 2, water = 3, ice = 4, wood = 5
}

public interface ISurface
{
    ECellType GetCellType();
}

public class Surface : MonoBehaviour, ISurface
{
    [SerializeField]private ECellType cType;
    public ECellType GetCellType() => cType;
}
