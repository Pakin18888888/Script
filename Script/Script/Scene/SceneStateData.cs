using System.Collections.Generic;

[System.Serializable]
public class SceneStateData
{
    public HashSet<string> openedDoors = new HashSet<string>();
    public HashSet<string> openedDrawers = new HashSet<string>();
    public HashSet<string> openedLockers = new HashSet<string>();
    public HashSet<string> collectedItem = new HashSet<string>();
    public HashSet<string> playedJumpScare = new HashSet<string>();
}