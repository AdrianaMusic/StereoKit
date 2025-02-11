#pragma once

#include "../stereokit.h"
#include "../libraries/sk_gpu.h"
#include "assets.h"
#include "shader.h"

namespace sk {

struct shaderargs_data_t {
	skg_bind_t   buffer_bind;
	size_t       buffer_size;
	skg_buffer_t buffer_gpu;
	bool         buffer_dirty;
	void        *buffer;
	tex_t       *textures;
	skg_bind_t  *texture_binds;
	int32_t      texture_count;
};

struct _material_t {
	asset_header_t    header;
	shader_t          shader;
	shaderargs_data_t args;
	transparency_     alpha_mode;
	cull_             cull;
	bool32_t          wireframe;
	depth_test_       depth_test;
	bool32_t          depth_write;
	int32_t           queue_offset;
	skg_pipeline_t    pipeline;
};

struct _material_buffer_t {
	int32_t      size;
	skg_buffer_t buffer;
};

void   material_destroy   (material_t material);
size_t material_param_size(material_param_ type);

extern _material_buffer_t material_buffers[14];

} // namespace sk