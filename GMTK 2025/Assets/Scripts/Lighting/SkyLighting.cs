using System;
using System.Linq.Expressions;
using LostResort.SignalShuttles;
using LostResort.Timers;
using UnityEngine;

[ExecuteAlways]
public class Lighting : MonoBehaviour
{
   [SerializeField]
   private DayTimer dayTimer;
   
   [SerializeField]
   private Light DirectionalLight;
   
   [SerializeField]
   private LightingPreset Preset;
   
   //[SerializeField, Range(0, 24)]
   //private float TimeOfDay;

   private void Start()
   {
      SignalShuttle.Emit(new OnGameStart());
   }
   
   private void Update()
   {
      if (Preset == null)
      {
         Debug.Log("Preset is null");
         return;
      }


      if (!Application.isPlaying)
      {
         UpdateLighting(0.5f);
         return;
      }


      //Debug.Log(dayTimer.TimeElapsedPercent + "from lighting");
      UpdateLighting(dayTimer.TimeElapsedPercent);

      
      
   }
   private void UpdateLighting(float timePercent)
   {
      //Debug.Log(timePercent);
      RenderSettings.skybox.SetColor("_Tint",Preset.SunRise.Evaluate(timePercent));
      
      DynamicGI.UpdateEnvironment(); // Optional, updates lighting if you're baking GI

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
         Light[] lights = GameObject.FindObjectsByType<Light>(sortMode: FindObjectsSortMode.None);
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
