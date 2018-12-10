//-----------------------------------------------------------------------------
//  TGB Menu System
//  Copyright (C) Phillip O'Shea
//  
//  Main Menu - 
//-----------------------------------------------------------------------------

// Load GUI
	exec ("~/gui/MainMenu.gui");

//------------------------------------------------------------------------------

function _loadMainMenu()
{
	// Load scripts
	exec ( "./MenuSystem.cs" );
	exec ( "./PreferenceMethods.cs" );
	exec ( "./OptionsMenu.cs" );
	
	//
	Canvas.setContent(MainMenuGui);
	
	//
	_setPreference( "LevelFolder", expandFilename( "~/data/levels" ) );
	_loadMenuConfigurationData( "./MainMenu.xml" );

	//
	MainMenuGuiSceneWindow.loadLevel( expandFilename( "~/data/levels/demoMenu.t2d" ) );

	// Display the splash screen
	//Canvas.hideCursor();
	_newFadeSequence("~/data/images/TGB_800.png 1000 2000 1000" TAB
					 " 1000 0 0", "Canvas.showCursor();");
	
	// Load the preference settings
	_loadOptionsPreferences();
}

//-----------------------------------------------------------------------------

function _newFadeSequence(%sequenceList, %completionScript)
{
	$FadeSequenceNumber = 0;
	%fieldCount = getFieldCount(%sequenceList);
	for (%i = 0; %i < %fieldCount; %i++)
		$FadeSequence[%i] = getField(%sequenceList, %i);

	$FadeSequenceMaxNumber        = %fieldCount;
	$FadeSequenceCompletionScript = %completionScript;

	_nextFadeSequence();
}

function _nextFadeSequence()
{
	%index = $FadeSequenceNumber;
	if (%index == $FadeSequenceMaxNumber)
	{
		eval( $FadeSequenceCompletionScript );
		return;
	}
	
	%bitmap  = getWord($FadeSequence[%index], 0);
	%fadeIn  = getWord($FadeSequence[%index], 1);
	%wait    = getWord($FadeSequence[%index], 2);
	%fadeOut = getWord($FadeSequence[%index], 3);
	
	_newFadeControl(%bitmap, %fadeIn, %wait, %fadeOut);
	
	$FadeSequenceNumber += 1;
}

function _newFadeControl(%bitmap, %in, %wait, %out)
{
	%windowSize  = Canvas.getContent().getExtent();
	%fadeControl = new GuiFadeinBitmapCtrl()
	{
      Profile     = "GuiDefaultProfile";
      HorizSizing = "width";
      VertSizing  = "height";
      
      Extent = %windowSize;
      
      bitmap      = %bitmap;
      fadeinTime  = %in;
      waitTime    = %wait;
      fadeoutTime = %out;
   };
   
   %fadeControl.schedule(1000, "checkStatus");

   Canvas.getContent().add(%fadeControl);
}

function GuiFadeinBitmapCtrl::checkStatus(%this)
{
	if (%this.done)
	{
		if (%this.isMethod("onComplete"))
			%this.onComplete();
			
		return;
	}
	
	%this.schedule(100, "checkStatus");
}

function GuiFadeinBitmapCtrl::onComplete(%this)
{
	_nextFadeSequence();
	%this.delete();
}

//-----------------------------------------------------------------------------

function _newGame()
{
	// Hide the cursor again
	//Canvas.hideCursor();
	
	// Fade out
	_newFadeSequence(" 0 0 1000", "_loadGameLevel();");
}

//-----------------------------------------------------------------------------

function _loadGameLevel()
{
	// Load the level into the scenewindow
	sceneWindow2D.loadLevel( expandFilename("~/data/levels/demoLevel.t2d") );
	
	// Set the content
	Canvas.setContent(mainScreenGui);
	
	// Fade the level in
	_newFadeSequence(" 500 0 0", "");
	
	//
	GlobalActionMap.bind(keyboard, "escape", _triggerReturnToMenu);
}

function _triggerReturnToMenu(%val)
{
	if (%val)
	{
		// Fade out
		_newFadeSequence(" 0 0 500", "_returnToMenu();");
	}
}

function _returnToMenu()
{
	// End the level
	sceneWindow2D.endLevel();
	
	// Set the content
	Canvas.setContent(MainMenuGui);
	
	// Fade the menu in
	_newFadeSequence(" 1000 0 0", "Canvas.showCursor();");
	
	// Play some music for the menu
	playMusic(IndoorMusic);
	
	GlobalActionMap.unbind(keyboard, "escape");
}

//-----------------------------------------------------------------------------

function MenuCameraMount::onLevelLoaded(%this)
{
	MainMenuGuiSceneWindow.mount(%this);
}

function MenuCameraMount::onWorldLimit(%this)
{
	MainMenuGuiSceneWindow.disMount();
	MainMenuGuiSceneWindow.setCurrentCameraPosition("-100 0");
	
	%this.setPositionX(-100);
	MainMenuGuiSceneWindow.mount(%this);
}