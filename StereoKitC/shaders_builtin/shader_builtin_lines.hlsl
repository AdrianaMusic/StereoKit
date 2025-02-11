#include "stereokit.hlsli"

//--name        = sk/lines
//--color:color = 1,1,1,1
//--diffuse = white
float4       color;
Texture2D    diffuse   : register(t0);
SamplerState diffuse_s : register(s0);

struct vsIn {
	float4 pos  : SV_Position;
	float3 norm : NORMAL0;
	float2 uv   : TEXCOORD0;
	float4 col  : COLOR0;
};
struct psIn {
	float4 pos   : SV_POSITION;
	float2 uv    : TEXCOORD0;
	float4 color : COLOR0;
	uint view_id : SV_RenderTargetArrayIndex;
};

psIn vs(vsIn input, uint id : SV_InstanceID) {
	psIn o;
	o.view_id = id % sk_view_count;
	id        = id / sk_view_count;

	float  magnitude = length(input.norm);
	float4 view      = mul(float4(input.pos.xyz, 1), sk_view[o.view_id]);
	float3 norm      = mul(float4(input.norm,    0), sk_view[o.view_id]).xyz;
	view.xy         += normalize(float2(norm.y, -norm.x))*magnitude;
	o.pos            = mul(view, sk_proj[o.view_id]);

	o.uv    = input.uv;
	o.color = input.col * color;
	return o;
}
float4 ps(psIn input) : SV_TARGET {
	float4 col = diffuse.Sample(diffuse_s, input.uv);

	col = col * input.color;

	return col;
}