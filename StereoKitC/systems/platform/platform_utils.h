#pragma once

#include "../../stereokit.h"

#if   defined(__EMSCRIPTEN__)
	#define SK_OS_WEB
#elif defined(__ANDROID__)
	#define SK_OS_ANDROID
#elif defined(__linux__)
	#define SK_OS_LINUX
#elif defined(WINDOWS_UWP)
	#define SK_OS_WINDOWS_UWP
#elif defined(_WIN32)
	#define SK_OS_WINDOWS
#endif

#ifndef _countof
#define _countof(array) (sizeof(array) / sizeof(array[0]))
#endif

namespace sk {

void  platform_msgbox_err(const char *text, const char *header);
bool  platform_read_file (const char *filename, void **out_data, size_t *out_size);
bool  platform_get_cursor(vec2 &out_pos);
void  platform_set_cursor(vec2 window_pos);
float platform_get_scroll();
void  platform_debug_output(log_ level, const char *text);
void  platform_sleep       (int ms);
void  platform_default_font(char *fontname_buffer, size_t buffer_size);

bool  platform_keyboard_available();
void  platform_keyboard_show     (bool visible);
bool  platform_keyboard_visible  ();

}