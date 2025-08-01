void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    Color = 1.0f;
    Attenuation = 1.0f;
#else
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation;
#endif
}

void AdditionalLights_float(float3 WorldPos, float3 WorldNormal, out float3 Color, out float Brightness)
{
    Color = 0.0f;
    Brightness = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();
    
    for (int i = 0; i < lightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);

        Color += light.color * light.distanceAttenuation;
        
        float ndotl = dot(WorldNormal, light.direction);
        
        Brightness += ndotl * light.distanceAttenuation;
    }
    
    Brightness = saturate(Brightness);
#endif
}