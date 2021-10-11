using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRoom : MonoBehaviour
{
    private IDungeon dungeon = null;
    private GameObject cellPrefab = null;
    private const float x0 = -4.5f;
    private const float y0 = 0.001f;
    private const float z0 = 0;

    private void Start()
    {
        if (dungeon == null) Debug.Log("Not init CRoom before start!");
        
        BuildHex();
        //BuildSquare();
    }
    private void BuildSquare()
    {
        int i, j;
        float x, y, z;
        GameObject cell;
        Vector3 cellPosition = new Vector3(0, 0, 0);

        y = y0;
        cellPosition.y = y;
        for (i = 0; i < 10; i++)
        {
            cell = Instantiate(cellPrefab, transform);
            x = (float)i + x0;
            z = z0;
            cellPosition.x = x;
            cellPosition.z = z;
            cell.transform.position = cellPosition;
            for (j = 1; j <= 6; j++)
            {
                z = z0 + ((float)j);
                cell = Instantiate(cellPrefab, transform);
                cellPosition.z = z;
                cell.transform.position = cellPosition;
                cell = Instantiate(cellPrefab, transform);
                z = z0 - ((float)j);
                cellPosition.z = z;
                cell.transform.position = cellPosition;
            }
        }
    }

    private void BuildHex()
    {
        int i, j;
        float x, y, z;
        GameObject cell;
        Vector3 cellPosition = new Vector3(0, 0, 0);

        y = y0;
        cellPosition.y = y;
        for (i = 0; i < 10; i++)
        {
            cell = Instantiate(cellPrefab, transform);
            x = (float)i + x0;
            z = z0;
            cellPosition.x = x;
            cellPosition.z = z;
            cell.transform.position = cellPosition;
            for (j = 1; j <= 7; j++)
            {
                z = z0 + ((float)j) * 0.866f;
                cell = Instantiate(cellPrefab, transform);
                cellPosition.x = x + ((j % 2) != 0 ? 0.5f : 0.0f);
                cellPosition.z = z;
                cell.transform.position = cellPosition;
                cell = Instantiate(cellPrefab, transform);
                z = z0 - ((float)j) * 0.866f;
                cellPosition.z = z;
                cell.transform.position = cellPosition;
            }
        }
    }
    public void Init(IDungeon _dungeon, GameObject _cellPrefab)
    {
        dungeon = _dungeon;
        cellPrefab = _cellPrefab;
    }
}
