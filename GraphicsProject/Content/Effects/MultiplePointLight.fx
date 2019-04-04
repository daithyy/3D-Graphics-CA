#define NUM_LIGHTS 3 // Max number of lights
#define NUM_TEXTURES 2 // Max number of diffuse textures

matrix World;
matrix View;
matrix Projection;

float Angle;

float3 AmbientColor = float3(0.15f, 0.15f, 0.15f);
float3 DiffuseColor = float3(1, 1, 1);
float3 LightColor[NUM_LIGHTS];

float3 SpecularColor;
float SpecularPower;

float3 Position[NUM_LIGHTS];
float Attenuation[NUM_LIGHTS];
float FallOff[NUM_LIGHTS];

float3 CameraPosition;

Texture2D ModelTexture;
Texture2D AlternateTexture;
Texture2D NormalTexture;
Texture2D SpecularTexture;

bool IsAlternate = false;

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
    float3 lightPos1 : TEXCOORD3;
    float3 lightPos2 : TEXCOORD4;
    float3 lightPos3 : TEXCOORD5;
};

// Vertex Shader
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

    // Determine the light positions based on the position of the lights and the position of the vertex in the world.
    output.lightPos1 = Position[0].xyz - _world.xyz;
    output.lightPos2 = Position[1].xyz - _world.xyz;
    output.lightPos3 = Position[2].xyz - _world.xyz;
	
    // Normalize the light position vectors.
    output.lightPos1 = normalize(output.lightPos1);
    output.lightPos2 = normalize(output.lightPos2);
    output.lightPos3 = normalize(output.lightPos3);

    float cRotation = cos(Angle);
    float sRotation = sin(Angle);

    output.UV = mul(output.UV, float2x2(cRotation, -sRotation, sRotation, cRotation));
    
    return output;
}

// Pixel Shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float3 _color = DiffuseColor;
    float3 _lightingColor = AmbientColor;
    float4 _finalColor;

    float3 _textureColor[NUM_TEXTURES];
    _textureColor[0] = ModelTexture.Sample(TextureSampler, input.UV);
    _textureColor[1] = AlternateTexture.Sample(TextureSampler, input.UV);

    float3 _normalColor = NormalTexture.Sample(TextureSampler, input.UV);

    float3 _lightDirection[NUM_LIGHTS];
    float3 _distance[NUM_LIGHTS];    
    float3 _reflection;

    float4 _specular;
    float4 _specularIntensity;

    float _angle[NUM_LIGHTS];
    float _atten[NUM_LIGHTS];

    // Shift from [0,1] to [-1,1] range
    _normalColor = _normalColor * 2 - 1;
    
    if (!IsAlternate)
        _color *= _textureColor[0];
    else
        _color *= _textureColor[1];

    for (int i; i < NUM_LIGHTS; i++)
    {
        _lightDirection[i] = normalize(Position[i] - input.WorldPosition);
        _distance[i] = distance(Position[i], input.WorldPosition);

        // Reflectance function
        _angle[i] = saturate(dot(_normalColor, _lightDirection[i]));

        // Inverse Square Color
        _atten[i] = 1 - pow(clamp(_distance[i] / Attenuation[i], 0, 1), FallOff[i]);

        _lightingColor += saturate(_angle[i] * _atten[i] * LightColor[i]);

        _finalColor = float4(_lightingColor * _color, 1);

        if (_angle[i] > 0.0f)
        {
		    // Sample the pixel from the specular map texture.
            _specularIntensity = SpecularTexture.Sample(TextureSampler, input.UV);

		    // Calculate the reflection vector based on the light intensity, normal vector, and light direction.
            _reflection = normalize(2 * _angle[i] * _normalColor - _lightDirection[i]);

		    // Determine the amount of specular light based on the reflection vector, viewing direction, and specular power.
            _specular = pow(saturate(dot(_reflection, input.ViewDirection)), SpecularPower);

		    // Use the specular map to determine the intensity of specular light at this pixel.
            _specular = _specular * _specularIntensity;

		    // Add the specular component last to the output color.
            _finalColor = saturate(_finalColor + _specular);
        }
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