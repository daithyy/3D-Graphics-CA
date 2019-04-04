matrix World;
matrix View;
matrix Projection;

float3 AmbientColor = float3(0.15f, 0.15f, 0.15f);
float3 DiffuseColor = float3(1, 1, 1);
float3 LightColor = float3(1, 1, 1);

float4 SpecularColor;
float SpecularPower;

float3 Position = float3(0, 0, 0);
float Attenuation = 40;
float FallOff = 2;

float3 CameraPosition;

Texture2D ModelTexture;
Texture2D NormalTexture;
Texture2D SpecularTexture;

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
    float3 WorldPosition : TEXCOORD1;
    float3 ViewDirection : TEXCOORD2;
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
    output.ViewDirection = CameraPosition.xyz - _world.xyz;
    output.ViewDirection = normalize(output.ViewDirection);
    
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float3 _color = DiffuseColor;
    float3 _textureColor = ModelTexture.Sample(TextureSampler, input.UV);
    float3 _lightingColor = AmbientColor;
    float4 _finalColor;
    float3 _lightDirection = normalize(Position - input.WorldPosition);
    float3 _distance = distance(Position, input.WorldPosition);
    float3 _reflection;
    float4 _specular;
    float4 _specularIntensity;

    _color *= _textureColor;
    
    // Sample from Normal Map
    float3 _normal = NormalTexture.Sample(TextureSampler, input.UV);
    // Shift from [0,1] to [-1,1] range
    _normal = _normal * 2 - 1;

    // Reflectance function
    float _angle = saturate(dot(_normal, _lightDirection));
    
    // Inverse Square Color
    float _atten = 1 - pow(clamp(_distance / Attenuation, 0, 1), FallOff);

    _lightingColor += saturate(_angle * _atten * LightColor);

    _finalColor = float4(_lightingColor * _color, 1);

    if (_angle > 0.0f)
    {
		// Sample the pixel from the specular map texture.
        _specularIntensity = SpecularTexture.Sample(TextureSampler, input.UV);

		// Calculate the reflection vector based on the light intensity, normal vector, and light direction.
        _reflection = normalize(2 * _angle * _normal - _lightDirection);

		// Determine the amount of specular light based on the reflection vector, viewing direction, and specular power.
        _specular = pow(saturate(dot(_reflection, input.ViewDirection)), SpecularPower);

		// Use the specular map to determine the intensity of specular light at this pixel.
        _specular = _specular * _specularIntensity;

		// Add the specular component last to the output color.
        _finalColor = saturate(_finalColor + _specular);
    }

    return _finalColor;
}

technique SpecMapTechnique
{
    pass P0
    {
        VertexShader = compile vs_5_0 MainVS();
        PixelShader = compile ps_5_0 MainPS();
    }
};