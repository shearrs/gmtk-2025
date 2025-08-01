using System.Linq.Expressions;
using UnityEngine;

[ExecuteAlways]
public class Lighting : MonoBehaviour
{
   [SerializeField]
   private Light DirectionalLight;
   
   [SerializeField]
   private LightingPreset Preset;
   
   [SerializeField, Range(0, 24)]
   private float TimeOfDay;

   private void Update()
   {
      if (Preset == null)
      {
         Debug.Log("Preset is null");
         return;
      }

      if (Application.isPlaying)
      {
         TimeOfDay += Time.deltaTime;
         TimeOfDay %= 24;
      }

      UpdateLighting(TimeOfDay / 24f);
   }
   private void UpdateLighting(float timePercent)
   {
      Debug.Log(timePercent);
      RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
      RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

      if (DirectionalLight != null)
      {
         DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
         DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
      }
      
   }

   //try to find a directional light if one has not been signed
   private void OnValidate()
   {
      if (DirectionalLight != null)
      {
         return;
      }

      if (RenderSettings.sun != null)
      {
         DirectionalLight = RenderSettings.sun;
      }
      else
      {
         Light[] lights = GameObject.FindObjectsOfType<Light>();
         foreach (Light light in lights)
         {
            if (light.type == LightType.Directional)
            {
               DirectionalLight = light;
            }
         }
      }
   }
}
