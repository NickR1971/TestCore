using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 GetPosition() => position;
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
        if (obj == null) Debug.Log($"Cell #{number} mark not instantiated!");
        else obj.GetComponent<Renderer>().material.color = _color;
    }
}

