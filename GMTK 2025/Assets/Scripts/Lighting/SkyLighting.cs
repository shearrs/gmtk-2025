using LostResort.SignalShuttles;
using LostResort.Timers;
using UnityEngine;

public class SkyLighting : MonoBehaviour
{
    [SerializeField]
    private DayTimer dayTimer;

    [SerializeField]
    private Light DirectionalLight;

    [SerializeField]
    private LightingPreset Preset;

    private float dayOneStartAngle;
    private Vector3 dayOneStartAxis;
    private float dayOneEndAngle;
    private Vector3 dayOneEndAxis;
    private float dayTwoStartAngle;
    private Vector3 dayTwoStartAxis;
    private float dayTwoEndAngle;
    private Vector3 dayTwoEndAxis;

    private void Start()
    {
        var start1 = Quaternion.Euler(new Vector3(Preset.dayOneSunAngles.x, 170, 0));
        var end1 = Quaternion.Euler(new Vector3(Preset.dayOneSunAngles.y, 170, 0));

        start1.ToAngleAxis(out dayOneStartAngle, out dayOneStartAxis);
        end1.ToAngleAxis(out dayOneEndAngle, out dayOneEndAxis);

        var start2 = Quaternion.Euler(new Vector3(Preset.dayTwoSunAngles.x, 170, 0));
        var end2 = Quaternion.Euler(new Vector3(Preset.dayTwoSunAngles.y, 170, 0));

        start2.ToAngleAxis(out dayTwoStartAngle, out dayTwoStartAxis);
        end2.ToAngleAxis(out dayTwoEndAngle, out dayTwoEndAxis);

        SignalShuttle.Emit(new OnGameStart());
    }

    private void Update()
    {
        UpdateLighting(dayTimer.TimeElapsedPercent);
    }

    private void UpdateLighting(float timePercent)
    {
        if (dayTimer.dayOne)
        {
            RenderSettings.skybox.SetColor("_Tint", Preset.DayOne.Evaluate(timePercent));
            RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            float angle = Mathf.LerpUnclamped(dayOneStartAngle, dayOneEndAngle, timePercent);
            Vector3 axis = Vector3.SlerpUnclamped(dayOneStartAxis, dayOneEndAxis, timePercent);

            DirectionalLight.transform.localRotation = Quaternion.AngleAxis(angle, axis);
        }
        else
        {
            RenderSettings.skybox.SetColor("_Tint", Preset.DayTwo.Evaluate(timePercent));
            RenderSettings.ambientLight = Preset.DayTwoAmbientColor.Evaluate(timePercent);
            DirectionalLight.color = Preset.DayTwoDirectionalColor.Evaluate(timePercent);

            float angle = Mathf.LerpUnclamped(dayTwoStartAngle, dayTwoEndAngle, timePercent);
            Vector3 axis = Vector3.SlerpUnclamped(dayTwoStartAxis, dayTwoEndAxis, timePercent);

            DirectionalLight.transform.localRotation = Quaternion.AngleAxis(angle, axis);
        }

        DynamicGI.UpdateEnvironment(); // Optional, updates lighting if you're baking GI
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
    }

    //try to find a directional light if one has not been signed
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
            DirectionalLight = RenderSettings.sun;
        else
        {
            Light[] lights = FindObjectsByType<Light>(sortMode: FindObjectsSortMode.None);
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    break;
                }
            }
        }
    }
}
