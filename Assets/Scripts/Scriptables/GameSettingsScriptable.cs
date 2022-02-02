using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettingsScriptable : ScriptableObject
{
    /// <summary>
    /// IMPORTANT: The game functionality and stack behavior is designed to work with multiples of 10.
    /// </summary>
    [Header("Betting Currency")]
    [Range(10, 200)]
    [Tooltip("Multiples of 10 Only")]
    public int initTotalAmmount = 100;
    [Range(1, 50)]
    [Tooltip("Multiples of 10 Only")]
    public int betAmmountMultiplier = 10;

    [Header("Betting Colors")]
    public Color32 betColorA;
    public string nameColorA;
    public Color32 betColorB;
    public string nameColorB;

    [Header("Chip Settings")]
    public GameObject chipPrefab;
    [Range(0.1f, 0.2f)]
    public float stackOffsetMultiplier;
    [Range(0.1f, 0.5f)]
    public float bulkOffsetMultiplier;
    public Color32[] chipColors;

    [Header("Audio Settings")]
    public EnableFXSounds enableFXSounds;
}

public enum EnableFXSounds
{
    Yes,
    No
}