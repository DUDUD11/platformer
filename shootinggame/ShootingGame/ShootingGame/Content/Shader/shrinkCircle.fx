#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


float4 filterColor;
float param1;
float2 framesize;
float2 origin;

texture SpriteTexture;



sampler SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
	float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float xrad = 0.5f / framesize.x;
	float yrad = 0.5f / framesize.y;



    float2 center = float2(origin.x * 2 * xrad , origin.y * 2 * yrad) + float2(xrad,yrad) ; 

    float2 distance = input.TextureCoordinates - center;
	distance.x = distance.x * framesize.x;
	distance.y = distance.y * framesize.y;

    if (length(distance) > param1 * 0.5f && texColor.a != 0)
	{
        texColor =  filterColor;
    }

	return texColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}