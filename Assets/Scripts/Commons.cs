[System.Serializable]
public class GameSettings
{
    public RunSettings Runs;
    public WeaponSettings Weapon;
    public RoomSettings Room;
    public EnemySettings Enemies;
    public MusicSettings Music;

}
[System.Serializable]
public class RoomSettings
{
    public float LavaCoolDown;
    public float LavaDPS;
    public float GasBottleFuse = 1.5f;
    public float GasBottleExplosionForce = 100.0f;
    public float GasBottleExplosionRadius = 2.0f;
    public float GasBottlePenalty = 10.0f;
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
    public float ReactDistance = 10.0f;
    public float MeleeReach = 1.3f;
    public float StartLife = 100.0f;
    public float RavMoveSpeed = 5.0f;
    public float RavMeleeCooldown = 1.0f;
    public float RavMeleePenalty = 5.0f;
    public float RavMeleeRadius = 0.3f;
    public float RavStepDistance = 0.8f;
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
    internal int RoomCleared = -1;
    internal int RoomFailed = 0;
    internal float GainedPenaltyTime = 0.0f;
    internal int roomresets = 0;

    override public string ToString()
    {
        string NL = System.Environment.NewLine;
        return $"Rooms Cleared  {RoomCleared}{NL}" +
            $"Rooms Reset {roomresets} times{NL}" +
            $"Rooms Failed  {RoomFailed}{NL}" +
            $"Gained Clear Time  {GainedClearTime}{NL}" +
            $"Penalty Time  {GainedPenaltyTime}";
    }
}

[System.Serializable]
public class AudioTierSettings
{
    public UnityEngine.AudioClip Clip = null;
    public float ThresholdTime = 0.0f;
    public float MaxVolume = 1.0f;
    public float FadeInTime = 3.0f;
    public float FadeOutTime = 3.0f;
}
[System.Serializable]
public class MusicSettings
{
    [UnityEngine.SerializeField]
    private AudioTierSettings musicT1 = null;
    [UnityEngine.SerializeField]
    private AudioTierSettings musicT2 = null;
    [UnityEngine.SerializeField]
    private AudioTierSettings musicT3 = null;

    public AudioTierSettings T1 { get => musicT1; set => musicT1 = value; }
    public AudioTierSettings T2 { get => musicT2; set => musicT2 = value; }
    public AudioTierSettings T3 { get => musicT3; set => musicT3 = value; }
}
