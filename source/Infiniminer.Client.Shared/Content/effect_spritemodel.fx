#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

struct VertexToPixel
{
    float4 Position   	: POSITION;    
    float2 TextureCoords: TEXCOORD0;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

//------- XNA-to-HLSL variables --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;

//------- Texture Samplers --------
Texture xTexture;
sampler TextureSampler = sampler_state
{
	texture = <xTexture>;
	magfilter = POINT;
	minfilter = ANISOTROPIC;
	mipfilter = NONE;
	AddressU = WRAP;
	AddressV = WRAP;
};

//------- Technique: Block --------

VertexToPixel BlockVS( float4 inPos : POSITION, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	return Output;    
}

PixelToFrame BlockPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);

	if(Output.Color.a < 0.5) discard;

	return Output;
}

technique SpriteModel
{
	pass Pass0
	{   
		VertexShader = compile VS_SHADERMODEL BlockVS();
		PixelShader  = compile PS_SHADERMODEL BlockPS();
	}
}
