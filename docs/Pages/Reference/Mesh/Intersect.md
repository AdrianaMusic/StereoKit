---
layout: default
title: Mesh.Intersect
description: Checks the intersection point of this ray and a Mesh with collision data stored on the CPU. A mesh without collision data will always return false. Ray must be in model space, intersection point will be in model space too. You can use the inverse of the mesh's world transform matrix to bring the ray into model space, see the example in the docs!
---
# [Mesh]({{site.url}}/Pages/Reference/Mesh.html).Intersect

<div class='signature' markdown='1'>
bool Intersect([Ray]({{site.url}}/Pages/Reference/Ray.html) modelSpaceRay, Vec3& modelSpaceAt)
</div>

|  |  |
|--|--|
|[Ray]({{site.url}}/Pages/Reference/Ray.html) modelSpaceRay|Ray must be in model space, the             intersection point will be in model space too. You can use the             inverse of the mesh's world transform matrix to bring the ray             into model space, see the example in the docs!|
|Vec3& modelSpaceAt|The intersection point of the ray and             the mesh, if an intersection occurs. This is in model space, and             must be transformed back into world space later.|
|RETURNS: bool|True if an intersection occurs, false otherwise!|

Checks the intersection point of this ray and a Mesh
with collision data stored on the CPU. A mesh without collision
data will always return false. Ray must be in model space,
intersection point will be in model space too. You can use the
inverse of the mesh's world transform matrix to bring the ray
into model space, see the example in the docs!




## Examples

## Ray Mesh Intersection
Here's an example of casting a Ray at a mesh someplace in world space,
transforming it into model space, calculating the intersection point,
and displaying it back in world space.

![Ray Mesh Intersection]({{site.url}}/img/screenshots/RayMeshIntersect.jpg)

```csharp
Mesh sphereMesh = Default.MeshSphere;
Mesh boxMesh    = Mesh.GenerateRoundedCube(Vec3.One*0.2f, 0.05f);
Pose boxPose    = new Pose(0,     0,     -0.5f,  Quat.Identity);
Pose castPose   = new Pose(0.25f, 0.21f, -0.36f, Quat.Identity);

public void Update()
{
	// Draw our setup, and make the visuals grab/moveable!
	UI.Handle("Box",  ref boxPose,  boxMesh.Bounds);
	UI.Handle("Cast", ref castPose, sphereMesh.Bounds*0.03f);
	boxMesh   .Draw(Default.MaterialUI, boxPose .ToMatrix());
	sphereMesh.Draw(Default.MaterialUI, castPose.ToMatrix(0.03f));
	Lines.Add(castPose.position, boxPose.position, Color.White, 0.01f);

	// Create a ray that's in the Mesh's model space
	Matrix transform = boxPose.ToMatrix();
	Ray    ray       = transform
		.Inverse
		.Transform(Ray.FromTo(castPose.position, boxPose.position));

	// Draw a sphere at the intersection point, if the ray intersects 
	// with the mesh.
	if (ray.Intersect(boxMesh, out Vec3 at))
	{
		sphereMesh.Draw(Default.Material, Matrix.TS(transform.Transform(at), 0.02f));
	}
}
```

