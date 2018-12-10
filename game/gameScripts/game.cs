//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// startGame
// All game logic should be set up here. This will be called by the level builder when you
// select "Run Game" or by the startup process of your game to load the first level.
//---------------------------------------------------------------------------------------------
function startGame(%level)
{
	Canvas.setCursor(DefaultCursor);

	new ActionMap(moveMap);   
	moveMap.push();

	$enableDirectInput = true;
	activateDirectInput();
	enableJoystick();

	// Loads various functions required through the kit
	exec ("./GameMethods.cs");
	initialisePlatformerKit();
	
	// Load the main menu
	exec ( "./MenuSystem/MainMenu.cs" );
	_loadMainMenu();
	
	// Play some music for the menu
	playMusic(IndoorMusic);
}

//---------------------------------------------------------------------------------------------
// endGame
// Game cleanup should be done here.
//---------------------------------------------------------------------------------------------
function endGame()
{
	sceneWindow2D.endLevel();
	moveMap.pop();
	moveMap.delete();
}
