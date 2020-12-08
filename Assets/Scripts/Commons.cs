[System.Serializable]
public class GameSettings
{
    public RunSettings Runs;
    public WeaponSettings Weapon;
}


[System.Serializable]
public class RunSettings
{

    public float StartTime = 60.0f;
    public ulong StartSeed = 1337;

}
[System.Serializable]
public class WeaponSettings
{
    public int MagSize = 4;
    public int FireRate = 60;
    public float EjectionForce = 22.0f;
    public float ReloadSpeed = 0.8f;

}

[System.Serializable]
public class Cmd
{
    public float forwardMove = 0.0f;
    public float rightMove = 0.0f;
    public bool wishJump = false;
    public bool crouch = false;
}

[System.Serializable]
public class RunStats
{
    internal float GainedClearTime = 0.0f;
    internal int RoomCleared = 0;
    internal int RoomFailed = 0;
    internal float GainedPenaltyTime;
    override public string ToString()
    {
        string NL = System.Environment.NewLine;
        return $"Rooms Cleared  {RoomCleared}{NL}" +
            $"Rooms Failed  {RoomFailed}{NL}" +
            $"Gained Clear Time  {GainedClearTime}{NL}" +
            $"Penalty Time  {GainedPenaltyTime}";
    }
}

