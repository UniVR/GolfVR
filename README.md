# GolfVR #

**The first Golf Game for Virtual Reality !**

This game should be played with a **VR headset** and a **Wii Controller** (wii controller not yet supported).

## Discussion ##

* For the moment when the club hit the ball the speed of the club is not used to determine the speed of the ball. It is because when we apply the *Mesh Collider* on the club, the *Rigid Body* must be set to *Kinematic* and we can not get the magnitude caused by the collision with the ball. Do we have to set a normal *Box Collider* on the club or do we ignore the magnitude and compute the velocity of the club in the code?

## Developers notes ##

For the moment only the scene GolfVR.unity is working and show the game. This scene may be loaded by the GUI in MainMenu.unity scene.


**# Ball Scripts**
* DetectTerrainType.cs: Detect the terrain under the ball and adapt the physics of the ball depending on it (grass, sand, ...). It is paremetred by layer of texture in the terrain.
* BallScript.cs: (Non parametrable) this script detect when the club hit the ball and take the Porperties of the club to handle the physics resulting (see below) 


**# Club Scripts**
* Shoot.cs : Give the parameters of the club shoot (time loading/shooting, angle min/max and information about the current state, ...).
* ClubProperties.cs:Ttheses parameters may be different for each clubs. Give the force that the club will give to the ball when hitted.

## Notes ##
This project use wiidevicelibrary in order to connect to the wiimote https://github.com/FrozenCow/wiidevicelibrary. Wiimote libs are inspired by coding4Fun article: http://channel9.msdn.com/coding4fun/articles/Managed-Library-for-Nintendos-Wiimote

If you are using the new Wiimote Controller on windows (with wii motion included) follow theses steps: https://wiki.dolphin-emu.org/index.php?title=Wii_Remote_Plus_%28RVL-CNT-01-TR%29_Connection_Guide Or: http://touchmote.net/wiimotetr

Configure Unity projects for GitHub Thanks to this post: http://www.strichnet.com/using-git-with-3d-games/

Remove file from commit (file > 100 Mb): http://stackoverflow.com/questions/12481639/remove-files-from-git-commit
