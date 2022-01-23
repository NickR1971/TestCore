using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRecordContainer : MonoBehaviour
{
    [SerializeField] private GameObject record0;
    private GameObject[] records;


    public void CreateListSave(string[] _saveNames, string[] _comments)
    {
        records = new GameObject[_saveNames.Length];
        record0.SetActive(true);
        record0.GetComponent<CRecord>().ResetTemplate();
        for (int i = 0; i < _saveNames.Length; i++)
        {
            records[i] = Instantiate(record0, Vector3.zero, Quaternion.identity, transform);
            records[i].GetComponent<CRecord>().InitSave(_saveNames[i],_comments[i]);
        }
        record0.GetComponent<CRecord>().InitZero();
    }

    public void CreateListLoad(string[] _saveNames, string[] _comments)
    {
        records = new GameObject[_saveNames.Length];
        record0.SetActive(true);
        record0.GetComponent<CRecord>().ResetTemplate();
        for (int i = 0; i < _saveNames.Length; i++)
        {
            records[i] = Instantiate(record0, Vector3.zero, Quaternion.identity, transform);
            records[i].GetComponent<CRecord>().InitLoad(_saveNames[i],_comments[i]);
        }
         record0.SetActive(false);
   }

    public void DestroyRecords()
    {
        for (int i = 0; i < records.Length; i++)
        {
            Destroy(records[i]);
        }
        records = null;
    }
}
