---
layout: default
title: Model.RecalculateBounds
description: Examines the subsets as they currently are, and rebuilds the bounds based on that! This is normally done automatically, but if you modify a Mesh that this Model is using, the Model can't see it, and you should call this manually.
---
# [Model]({{site.url}}/Pages/Reference/Model.html).RecalculateBounds

<div class='signature' markdown='1'>
void RecalculateBounds()
</div>

Examines the subsets as they currently are, and rebuilds
the bounds based on that! This is normally done automatically,
but if you modify a Mesh that this Model is using, the Model
can't see it, and you should call this manually.



