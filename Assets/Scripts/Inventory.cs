using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool isGodMode = false;
    int rupeeCount = 0;
    int keyCount = 0;
    int bombCount = 0;

    public void AddRupees(int numRupees)
    {
        rupeeCount += numRupees;
    }

    public int GetRupees()
    {
        return rupeeCount;
    }

    public void AddKey()
    {
        keyCount++;
    }

    public void AddBombs(int numBombs)
    {
        bombCount += numBombs;
    }

    public bool useKey()
    {
        if (isGodMode)
        {
            return true;
        }
        if (keyCount == 0)
        {
            return false;
        }
        else
        {
            keyCount--;
            return true;
        }
    }

    public int GetKeys()
    {
        return keyCount;
    }

    public int GetBombs()
    {
        return bombCount;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isGodMode)
            {
                isGodMode = false;
            }
            else
            {
                isGodMode = true;
            }
        }
    }
}
