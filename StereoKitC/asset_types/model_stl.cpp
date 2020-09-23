#include "model.h"
#include "../libraries/stref.h"
#include "../libraries/sort_list.h"
#include "../libraries/array.h"
#include "../math.h"

#include <stdio.h>

namespace sk {

struct stl_header_t {
	uint8_t header[80];
	uint32_t tri_count;
};

#pragma pack(1) 
struct stl_triangle_t {
	vec3 normal;
	vec3 verts[3];
	uint16_t attribute;
};

///////////////////////////////////////////

vind_t indexof(vec3 pt, vec3 normal, array_t<vert_t> *verts, hashmap_t<vec3, vind_t> *indmap) {
	vind_t  result = 0;
	int64_t id     = indmap->contains(pt);
	if (id < 0) {
		result = verts->add(vert_t{ pt, {}, {}, {255,255,255,255} });
		indmap->ensure(pt, result);
	} else {
		result = indmap->items[id];
	}
	verts->get(result).norm += normal;
	return result;
}

///////////////////////////////////////////

bool modelfmt_stl_binary(void *file_data, size_t, array_t<vert_t> *verts, array_t<vind_t> *faces) {
	stl_header_t *header = (stl_header_t *)file_data;
	hashmap_t<vec3, vind_t> indmap = {};

	stl_triangle_t *tris = (stl_triangle_t *)(((uint8_t *)file_data) + sizeof(stl_header_t));
	for (uint32_t i = 0; i < header->tri_count; i++) {
		faces->add(indexof(tris[i].verts[0], tris[i].normal, verts, &indmap));
		faces->add(indexof(tris[i].verts[1], tris[i].normal, verts, &indmap));
		faces->add(indexof(tris[i].verts[2], tris[i].normal, verts, &indmap));
	}

	indmap.free();
	return true;
}

///////////////////////////////////////////

bool modelfmt_stl_text(void *file_data, size_t, array_t<vert_t> *verts, array_t<vind_t> *faces) {
	hashmap_t<vec3, vind_t> indmap = {};
	
	vec3    normal     = {};
	vind_t  curr[4]    = {};
	int32_t curr_count = 0;

	stref_t data = stref_make((const char *)file_data);
	stref_t line = {};
	while (stref_nextline(data, line)) {
		stref_t word = {};
		if (!stref_nextword(line, word))
			continue;

		if (stref_equals(word, "facet")) {
			if (stref_nextword(line, word) && stref_equals(word, "normal")) {
				normal = {};
				if (stref_nextword(line, word)) normal.x = stref_to_f(word);
				if (stref_nextword(line, word)) normal.y = stref_to_f(word);
				if (stref_nextword(line, word)) normal.z = stref_to_f(word);
			}
		} else if (stref_equals(word, "endfacet")) {
			faces->add(curr[0]); faces->add(curr[1]); faces->add(curr[2]);
			if (curr_count == 4) {
				faces->add(curr[0]); faces->add(curr[2]); faces->add(curr[3]);
			}
			curr_count = 0;
		} else if (stref_equals(word, "vertex")) {
			if (curr_count != 4) {
				vec3 pt = {};
				if (stref_nextword(line, word)) pt.x = stref_to_f(word);
				if (stref_nextword(line, word)) pt.y = stref_to_f(word);
				if (stref_nextword(line, word)) pt.z = stref_to_f(word);
				curr[curr_count] = indexof(pt, normal, verts, &indmap);
				curr_count = mini(4, curr_count + 1);
			}
		}
	}

	indmap.free();
	return true;
}

///////////////////////////////////////////

bool modelfmt_stl(model_t model, const char *filename, void *file_data, size_t file_length, shader_t shader) {
	array_t<vert_t> verts = {};
	array_t<vind_t> faces = {};

	bool result = file_length > 5 && memcmp(file_data, "solid", sizeof(char) * 5) == 0 ?
		modelfmt_stl_text  (file_data, file_length, &verts, &faces) :
		modelfmt_stl_binary(file_data, file_length, &verts, &faces);

	// Normalize all the normals
	for (size_t i = 0; i < verts.count; i++)
		verts[i].norm = vec3_normalize(verts[i].norm);

	char id[512];
	snprintf(id, sizeof(id), "%s/mesh", filename);
	mesh_t mesh = mesh_create();
	mesh_set_id   (mesh, id);
	mesh_set_verts(mesh, &verts[0], verts.count);
	mesh_set_inds (mesh, &faces[0], faces.count);

	model_add_subset(model, mesh, shader == nullptr ? material_find(default_id_material) : material_create(shader), matrix_identity);

	mesh_release(mesh);

	verts.free();
	faces.free();
	return result;
}

}