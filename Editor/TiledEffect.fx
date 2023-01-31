#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_3
	#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

float2 textureSize;
float2 targetSize;
float4 padding;
float2 mode;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float process_axis(float coord, float targetMin, float targetMax, float targSize, float textureMin, float textureMax, float textSize, int m)
{
    if (coord < targetMin)
    {
        float c = coord / targetMin * textureMin;
        return c / textSize;
    }
    if (coord > targetMax)
    {
        float c = (coord - targetMax) / (targSize - targetMax) * (textSize - textureMax) + textureMax;
        return c / textSize;
    }
    return m * ((coord - targetMin) / (targetMax - targetMin) * (textureMax - textureMin) + textureMin) / textSize + (1 - m) * (float(int(coord - targetMin) % int(textureMax - textureMin)) + textureMin) / textSize;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 coord = float2(input.TextureCoordinates.x * targetSize.x, input.TextureCoordinates.y * targetSize.y);

	coord = float2(
        process_axis(coord.x, padding.x, targetSize.x - padding.z, targetSize.x, padding.x, textureSize.x - padding.z, textureSize.x, mode.x),
        process_axis(coord.y, padding.y, targetSize.y - padding.w, targetSize.y, padding.y, textureSize.y - padding.w, textureSize.y, mode.y)
    );
    return tex2D(SpriteTextureSampler, coord) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};