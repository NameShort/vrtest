using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPos : MonoBehaviour
{
    public List<GameObject> Listinven = new List<GameObject>();
    public List<GameObject> inven = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < Listinven.Count; i++)
        {
            Instantiate(Listinven[i], inven[i].transform.position, inven[i].transform.rotation).transform.parent = inven[i].transform;
        }

    }

    void Update()
    {
       
    }
}
