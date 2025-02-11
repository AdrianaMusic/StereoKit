---
layout: default
title: Input
description: Input from the system come from this class! Hands, eyes, heads, mice and pointers!
---
# Input

Input from the system come from this class! Hands, eyes,
heads, mice and pointers!




## Static Fields and Properties

|  |  |
|--|--|
|[Pose]({{site.url}}/Pages/Reference/Pose.html) [Eyes]({{site.url}}/Pages/Reference/Input/Eyes.html)|If the device has eye tracking hardware and the app has permission to use it, then this is the most recently tracked eye pose. Check `Input.EyesTracked` to see if the pose is up-to date, or if it's a leftover!  You can also check `SK.System.eyeTrackingPresent` to see if the hardware is capable of providing eye tracking.  On Flatscreen when the MR sim is still enabled, then eyes are emulated using the cursor position when the user holds down Alt.|
|[BtnState]({{site.url}}/Pages/Reference/BtnState.html) [EyesTracked]({{site.url}}/Pages/Reference/Input/EyesTracked.html)|If eye hardware is available and app has permission, then this is the tracking state of the eyes. Eyes may move out of bounds, hardware may fail to detect eyes, or who knows what else!  On Flatscreen when MR sim is still enabled, this will report whether the user is simulating eye input with the Alt key.|
|[Pose]({{site.url}}/Pages/Reference/Pose.html) [Head]({{site.url}}/Pages/Reference/Input/Head.html)|The position and orientation of the user's head! This is the center point between the user's eyes, NOT the center of the user's head. Forward points the same way the user's face is facing.|
|[Mouse]({{site.url}}/Pages/Reference/Mouse.html) [Mouse]({{site.url}}/Pages/Reference/Input/Mouse.html)|Information about this system's mouse, or lack thereof!|


## Static Methods

|  |  |
|--|--|
|[Hand]({{site.url}}/Pages/Reference/Input/Hand.html)|Retreives all the information about the user's hand! StereoKit will always provide hand information, however sometimes that information is simulated, like in the case of a mouse, or controllers.  Note that this is a copy of the hand information, and it's a good chunk of data, so it's a good idea to grab it once and keep it around for the frame, or at least function, rather than asking for it again and again each time you want to touch something.|
|[HandClearOverride]({{site.url}}/Pages/Reference/Input/HandClearOverride.html)|Clear out the override status from Input.HandOverride, and restore the user's control over it again.|
|[HandMaterial]({{site.url}}/Pages/Reference/Input/HandMaterial.html)|Set the Material used to render the hand! The default material uses an offset of 10 to ensure it gets drawn overtop of other elements.|
|[HandOverride]({{site.url}}/Pages/Reference/Input/HandOverride.html)|This allows you to completely override the hand's pose information! It is still treated like the user's hand, so this is great for simulating input for testing purposes. It will remain overridden until you call Input.HandClearOverride.|
|[HandSolid]({{site.url}}/Pages/Reference/Input/HandSolid.html)|Does StereoKit register the hand with the physics system? By default, this is true. Right now this is just a single block collider, but later will involve per-joint colliders!|
|[HandVisible]({{site.url}}/Pages/Reference/Input/HandVisible.html)|Sets whether or not StereoKit should render the hand for you. Turn this to false if you're going to render your own, or don't need the hand itself to be visible.|
|[Key]({{site.url}}/Pages/Reference/Input/Key.html)|Keyboard key state! On desktop this is super handy, but even standalone MR devices can have bluetooth keyboards, or even just holographic system keyboards!|

