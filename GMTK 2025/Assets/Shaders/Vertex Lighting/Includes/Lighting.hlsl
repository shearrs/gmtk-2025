void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    Color = 1.0f;
    Attenuation = 1.0f;
#elif defined(LIGHTS_DISABLED)
    Direction = float3(0.0f, 0.0f, 0.0f);
    Color = 1.0f;
    Attenuation = 1.0f;
#else
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.shadowAttenuation;
#endif
}

void AdditionalLights_float(float3 WorldPos, float3 WorldNormal, out float3 Color)
{
    Color = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    #ifdef LIGHTS_DISABLED
    Color = 1.0f;
#elif defined(_ADDITIONAL_LIGHTS)
    int lightCount = GetAdditionalLightsCount();
    
    InputData inputData = (InputData)0;
    float4 screenPos = ComputeScreenPos(TransformWorldToHClip(WorldPos));
    inputData.normalizedScreenSpaceUV = screenPos.xy / screenPos.w;
    inputData.positionWS = WorldPos;
    
    LIGHT_LOOP_BEGIN(lightCount)
    Light light = GetAdditionalLight(lightIndex, WorldPos);

    Color += light.color * light.distanceAttenuation;
    LIGHT_LOOP_END
    #endif
#endif
}