---
layout: default
title: SphericalHarmonics
description: Spherical Harmonics are kinda like Fourier, but on a sphere. That doesn't mean terribly much to me, and could be wrong, but check out [here](http.//www.ppsloan.org/publications/StupidSH36.pdf) for more details about how Spherical Harmonics work in this context!  However, the more prctical thing is, SH can be a function that describes a value over the surface of a sphere! This is particularly useful for lighting, since you can basically store the lighting information for a space in this value! This is often used for lightmap data, or a light probe grid, but StereoKit just uses a single SH for the entire scene. It's a gross oversimplification, but looks quite good, and is really fast! That's extremely great when you're trying to hit 60fps, or even 144fps.
---
# SphericalHarmonics

Spherical Harmonics are kinda like Fourier, but on a sphere.
That doesn't mean terribly much to me, and could be wrong, but check
out [here](http://www.ppsloan.org/publications/StupidSH36.pdf) for
more details about how Spherical Harmonics work in this context!

However, the more prctical thing is, SH can be a function that
describes a value over the surface of a sphere! This is particularly
useful for lighting, since you can basically store the lighting
information for a space in this value! This is often used for
lightmap data, or a light probe grid, but StereoKit just uses a
single SH for the entire scene. It's a gross oversimplification, but
looks quite good, and is really fast! That's extremely great when
you're trying to hit 60fps, or even 144fps.



## Instance Methods

|  |  |
|--|--|
|[SphericalHarmonics]({{site.url}}/Pages/Reference/SphericalHarmonics/SphericalHarmonics.html)|Creates a SphericalHarmonic from an array of coefficients. Useful for loading stored data!|
|[Add]({{site.url}}/Pages/Reference/SphericalHarmonics/Add.html)|Adds a 'directional light' to the lighting approximation. This can be used to bake a multiple light setup, or accumulate light from a field of points.|
|[Brightness]({{site.url}}/Pages/Reference/SphericalHarmonics/Brightness.html)|Scales all the SphericalHarmonic's coefficients! This behaves as if you're modifying the brightness of the lighting this object represents.|
|[Sample]({{site.url}}/Pages/Reference/SphericalHarmonics/Sample.html)|Look up the color information in a particular direction!|
|[ToArray]({{site.url}}/Pages/Reference/SphericalHarmonics/ToArray.html)|Converts the SphericalHarmonic into an array of coefficients 9 long. Useful for storing calculated data!|



## Static Methods

|  |  |
|--|--|
|[FromLights]({{site.url}}/Pages/Reference/SphericalHarmonics/FromLights.html)|Creates a SphericalHarmonics approximation of the irradiance given from a set of directional lights!|

