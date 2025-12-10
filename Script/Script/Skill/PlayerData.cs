using System.Collections.Generic;
using System.Numerics;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public int hp;
    public int battery;
    public List<string> inventory = new List<string>();
}