# THE LIST

(not any list, THE LIST)

- [x] modify default template, get rid of all the trash (moving around, hands, objects, allat)
- [ ] different obstacles - check with levelmanager how quick to move towards players, and a seperate thing for when passed the player - e.g. reduce life
  - [x] one ring - have your head inside
  - [x] block that stays a little bit, has a number on it, every head bash counts it down. destroy it. if you dont it eventually turns red and explodes
  - [x] some obstacle to avoid, your head shouldnt be in there
- [ ] MusicManager - plays song, has a function that always returns current millisecond
	- [x] customizable start wait time, milliseconds start negative.
	- [x] when you headbang play noise not song
	- [x] headbang detector -> seperate cause headbangs are used for gameplay & menus
	- [ ] win screen when you finish song -> headbang -> main menu
- [ ] LevelManager - has a schedule of what type of obstacle to spawn at what millisecond (makes sure that considering speed it will arrive then, doesnt just spawn it at that millisecond). also has info on how quick everything should be going
- [ ] LifeManager - gets displayed on hud & reduced when an obstacle is failed
	- [x] display lives
	- [x] decrement lives when hitting obstacle
	- [ ] game over screen when you lose -> headbang -> main menu
- [x] main menu, headbang to start game

## potentially

- [ ] Tube obstacle (buncha rings, you gotta have your head inside them)
