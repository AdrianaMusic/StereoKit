#pragma once

#include "../render.h"

namespace sk {
bool uwp_init(const char *app_name);
void uwp_step_begin();
void uwp_step_end();
void uwp_render(render_list_t render_list);
void uwp_vsync();
void uwp_shutdown();

bool  uwp_get_mouse(vec2 &out_pos);
void  uwp_set_mouse(vec2 window_pos);
float uwp_get_scroll();
bool  uwp_mouse_button(int button);
bool  uwp_key_down(int vk);
}