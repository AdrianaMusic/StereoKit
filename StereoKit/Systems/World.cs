﻿using System;
using System.Runtime.InteropServices;

namespace StereoKit
{
	/// <summary>World contains information about the real world around the 
	/// user. This includes things like play boundaries, scene understanding,
	/// and other various things.</summary>
	public static class World
	{
		/// <summary>This refers to the play boundary, or guardian system
		/// that the system may have! Not all systems have this, so it's
		/// always a good idea to check this first!</summary>
		public static bool HasBounds  => NativeAPI.world_has_bounds();
		/// <summary>This is the size of a rectangle within the play
		/// boundary/guardian's space, in meters if one exists. Check
		/// `World.BoundsPose` for the center point and orientation of the
		/// boundary, and check `World.HasBounds` to see if it exists at all!
		/// </summary>
		public static Vec2 BoundsSize => NativeAPI.world_get_bounds_size();
		/// <summary>This is the orientation and center point of the system's
		/// boundary/guardian. This can be useful to find the floor height!
		/// Not all systems have a boundary, so be sure to check 
		/// `World.HasBounds` first.</summary>
		public static Pose BoundsPose => NativeAPI.world_get_bounds_pose();

		/// <summary>Converts a Windows Mirage spatial node GUID into a Pose
		/// based on its current position and rotation! Check
		/// StereoKitApp.System.spatialBridge to see if this is available to
		/// use. Currently only on HoloLens, good for use with the Windows
		/// QR code package.</summary>
		/// <param name="spatialNodeGuid">A Windows Mirage spatial node GUID
		/// aquired from a windows MR API call.</param>
		/// <returns>A Pose representing the current orientation of the
		/// spatial node.</returns>
		public static Pose FromSpatialNode(Guid spatialNodeGuid)
			=> NativeAPI.world_from_spatial_graph(spatialNodeGuid.ToByteArray());

		public static Pose FromPerceptionAnchor(object perceptionSpatialAnchor)
		{
			IntPtr unknown = Marshal.GetIUnknownForObject(perceptionSpatialAnchor);
			Pose   result  = NativeAPI.world_from_perception_anchor(unknown);
			Marshal.Release(unknown);
			return result;
		}
	}
}
