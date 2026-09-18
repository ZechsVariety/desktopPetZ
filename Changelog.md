# DesktopPet-Z Changelog



This is the changelog for my changes in DesktopPet-Z only. See "Vanilla Changelog.md" for other DesktopPet updates.



### v0.0 (FILL THIS OUT)

### v0.1 (FILL THIS OUT)

### v0.2 (FILL THIS OUT)



### v0.3 (FILL THIS OUT)

* (WIP) Flinging



v0.3.1 (WIP)

* Made it so that key-animations can play sound effects



### V0.4 (WIP)

* Pets can now interact with each other and the sides of windows! Try it out with the Social Sheep!

  * When pets collide, there are 3 main things that could happen:

    * 1\. Both pets play the same random animation
    * 2\. One pet mimics whatever animation the other pet is currently playing (ex: if a Social Sheep is sleeping, the others will get sleepy too!)
    * 3\. Both pets play different random animations
  * There are two new border types: windowSide and petSide.
  * Pets intentionally phase through windows if they don't have enough room to get past the screen borders
  * WIP info:

    * Pets currently don't detect right/left window borders when being tossed
* Changed left/right border detection so that it can happen even while pet is on top of a window. This is mainly so that pets still collide with each other when on a window, but it also just makes collisions a lot more predictable

  * EX: previously, if you brought a window to the right of the screen, the sheep could walk outside the screen border. Now it cannot, but you can still force it outside if you're evil like that.
  * As a result, you can now build obstacle courses out of windows :D
* Fixed bug where the "only" value of each border animation was not being respected(?)

  * EX: now, if the sheep hits the side of a window, only animations with the Only values "windowSide" and "none" can play

