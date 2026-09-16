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

* Pets can now detect the side borders of windows and other pets!

  * Technical info:

    * There are two new border types: windowSide and petSide.
    * Pets intentionally phase through windows if they don't have enough room to get past the screen borders
  * WIP info:

    * Pets currently don't detect right/left window borders when being tossed
* Fixed bug where the "only" value of each border animation was not being respected.

  * EX: if the sheep hits the side of a window, only animations with the Only values "windowSide" and "none" can play

