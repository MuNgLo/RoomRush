[System.Serializable]
public class GameSettings
{
    public RunSettings Runs;
    public WeaponSettings Weapon;
    public RoomSettings Room;
    public EnemySettings Enemies;
}
[System.Serializable]
public class RoomSettings
{
    public float LavaCoolDown;
    public float LavaDPS;
}

[System.Serializable]
public class RunSettings
{

    public float StartTime = 60.0f;
    public ulong StartSeed = 1337;

}
[System.Serializable]
public class EnemySettings
{
    public float StunTime = 1.5f;
    public float RagDrag = 3.0f;
    public float RagPushModifier = 0.02f;
    public float RagMaxSpeed = 7.0f;
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

[System.Serializable]
public class AudioTierSettings
{

    public UnityEngine.AudioClip Clip = null;
    public float MaxVolume = 1.0f;
    public float FadeInTime = 3.0f;
    public float FadeOutTime = 3.0f;
}

