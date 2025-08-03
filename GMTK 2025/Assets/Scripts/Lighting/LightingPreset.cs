using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset",  menuName = "ScriptableObjects/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DayTwoAmbientColor;
    public Gradient DirectionalColor;
    public Gradient DayTwoDirectionalColor;
    public Gradient FogColor;

    [FormerlySerializedAs("SunRise")] public Gradient DayOne;
    public Gradient DayTwo;

    public Vector2 dayOneSunAngles;
    public Vector2 dayTwoSunAngles;
}
