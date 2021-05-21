using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadModule
{
    public void Load(string saveJson);

    public string SaveJson();
}
