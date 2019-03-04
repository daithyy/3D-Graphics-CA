matrix World;
matrix View;
matrix Projection;

float3 AmbientColor = float3(0.15f, 0.15f, 0.15f);
float3 DiffuseColor = float3(1, 1, 1);
float3 LightColor = float3(1, 1, 1);

float3 Position = float3(0, 0, 0);
float Attenuation = 40;
float FallOff = 2;

Texture2D ModelTexture;
Texture2D NormalTexture;

SamplerState TextureSampler
{
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
    float3 WorldPosition : TEXTCOORD2;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
    
    // From local to world
    float4 _world = mul(input.Position, World);
    float4 _view = mul(_world, View);
    float4 _projection = mul(_view, Projection);

    output.Position = _projection;
    output.UV = input.UV;
    output.WorldPosition = _world;
    
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float3 _color = DiffuseColor;
    float3 _textureColor = ModelTexture.Sample(TextureSampler, input.UV);
    float3 _lightingColor = AmbientColor;
    float3 _lightDirection = normalize(Position - input.WorldPosition);
    float3 _distance = distance(Position, input.WorldPosition);

    _color *= _textureColor;
    
    // Sample from Normal Map
    float3 _normal = NormalTexture.Sample(TextureSampler, input.UV);
    // Shift from [0,1] to [-1,1] range
    _normal = _normal * 2 - 1;

    // Reflectance function
    float3 _angle = saturate(dot(_normal, _lightDirection));
    
    // Inverse Square Color
    float _atten = 1 - pow(clamp(_distance / Attenuation, 0, 1), FallOff);

    _lightingColor += saturate(_angle * _atten * LightColor);

    return float4(_lightingColor * _color, 1);
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};