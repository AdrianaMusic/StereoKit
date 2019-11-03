﻿
namespace StereoKit
{
    public class Renderer
    {
        public static void Add(Mesh mesh, Material material, Transform transform)
            => NativeAPI.render_add_mesh_tr(mesh._meshInst, material._materialInst, transform._transformInst, Color.White);
        public static void Add(Mesh mesh, Material material, Transform transform, Color color)
            => NativeAPI.render_add_mesh_tr(mesh._meshInst, material._materialInst, transform._transformInst, color);

        public static void Add(Mesh mesh, Material material, Matrix transform)
            => NativeAPI.render_add_mesh(mesh._meshInst, material._materialInst, ref transform, Color.White);
        public static void Add(Mesh mesh, Material material, Matrix transform, Color color)
            => NativeAPI.render_add_mesh(mesh._meshInst, material._materialInst, ref transform, color);

        public static void Add(Model model, Transform transform)
            => NativeAPI.render_add_model_tr(model._modelInst, transform._transformInst, Color.White);
        public static void Add(Model model, Transform transform, Color color)
            => NativeAPI.render_add_model_tr(model._modelInst, transform._transformInst, color);
        
        public static void Add(Model model, Matrix transform)
            => NativeAPI.render_add_model(model._modelInst, ref transform, Color.White);
        public static void Add(Model model, Matrix transform, Color color)
            => NativeAPI.render_add_model(model._modelInst, ref transform, color);

        public static void SetCamera(Camera camera)
            => NativeAPI.render_set_camera(camera._cameraInst);
        
        public static void SetView(Transform cameraTransform)
            => NativeAPI.render_set_view(cameraTransform._transformInst);
        
        public static void SetSkytex(Tex2D skyTexture, bool showSky = true)
            => NativeAPI.render_set_skytex(skyTexture._texInst, showSky?1:0);
        
        public static void Blit(Tex2D toRendertarget, Material material)
            => NativeAPI.render_blit(toRendertarget._texInst, material._materialInst);
        
        public static void BeginCapture(int width = 500, int height = 500, int fps = 24)
            => NativeAPI.render_begin_capture(width, height, fps);

        public static void EndCapture()
            => NativeAPI.render_end_capture();
    }
}