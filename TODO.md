# THE LIST

(not any list, THE LIST)

- [x] modify default template, get rid of all the trash (moving around, hands, objects, allat)
- [x] different obstacles - check with levelmanager how quick to move towards players, and a seperate thing for when passed the player - e.g. reduce life
  - [x] one ring - have your head inside
  - [x] block that stays a little bit, has a number on it, every head bash counts it down. destroy it. if you dont it eventually turns red and explodes
  - [x] some obstacle to avoid, your head shouldnt be in there
- [x] MusicManager - plays song, has a function that always returns current millisecond
	- [x] customizable start wait time, milliseconds start negative.
	- [x] when you headbang play noise not song
	- [x] headbang detector -> seperate cause headbangs are used for gameplay & menus
	- [x] win screen when you finish song -> headbang -> main menu
- [x] LevelManager - has a schedule of what type of obstacle to spawn at what millisecond (makes sure that considering speed it will arrive then, doesnt just spawn it at that millisecond). also has info on how quick everything should be going
- [x] LifeManager - gets displayed on hud & reduced when an obstacle is failed
	- [x] display lives
	- [x] decrement lives when hitting obstacle
	- [x] game over screen when you lose -> headbang -> main menu
- [x] main menu, headbang to start game


# POLISH

## calum
 - [x] have the rings be rings and all the obstacles be actual shapes
 - [x] glow obstacles now use a set colour function 
 - [ ] first obstacle has little crosses to show have to go through it, and the wall 
 - [X] correct font for time display, and no time: its very apparent
 - [ ] explosion when you headbang too slowly
 - [ ] every time you go through the tube have the colour change 
 - [X] Chnage the bang obstacle to use actual rings

## freddy
 - [x] Lives with Hearts that stop glowing when theyre gone
 - [x] get rid of the text in the bangs once theyre completed as well
 - [ ] score system
 - [x] countdown before things start
 - [x] improved yeah! whoo! wow! sound effects when you headbang
 - [ ] basalt deltas dust air effect
 - [ ] sound effect for losing, winning, starting game, passing through a ring successfully, missing a ring, hitting a wall



- [ ] build and publish