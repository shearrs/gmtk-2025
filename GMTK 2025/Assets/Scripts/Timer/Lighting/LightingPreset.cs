using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset",  menuName = "ScriptableObjects/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
