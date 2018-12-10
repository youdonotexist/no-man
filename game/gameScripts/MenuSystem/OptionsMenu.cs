//-----------------------------------------------------------------------------
//  TGB Menu System
//  Copyright (C) Phillip O'Shea
//  
//  Options Menu - 
//-----------------------------------------------------------------------------

// Load GUI
	exec ("~/gui/OptionsMenu.gui");

// Variables
	
	
// Functions
	

//-----------------------------------------------------------------------------

function _loadOptionsPreferences()
{
	$InputMap::Count = 0;
	
	// The two files to load
	%optionsFile[0] = expandFilename( "./OptionsMenuDefaults.xml" );
	%optionsFile[1] = expandFilename( "./OptionsMenuPreferences.xml" );
	
	%xmlFile = new ScriptObject()
	{
		class = "XML";
	};
	
	%maxFiles = 2;
	for (%i = 0; %i < %maxFiles; %i++)
	{
		if ( %xmlFile.beginRead(%optionsFile[%i]) )
		{
			if ( %xmlFile.readClassBegin( "MenuConfiguration" ) )
			{
				// Input map
				if ( %xmlFile.readClassBegin( "InputMap" ) )
				{
					_loadInputMap(%xmlFile);
					%xmlFile.readClassEnd();
				}
				
				// Video settings
				if ( %xmlFile.readClassBegin( "VideoSettings" ) )
				{
					_loadVideoSettings(%xmlFile);
					%xmlFile.readClassEnd();
				}
				
				// Audio settings
				if ( %xmlFile.readClassBegin( "AudioSettings" ) )
				{
					_loadAudioSettings(%xmlFile);
					%xmlFile.readClassEnd();
				}
			}
			
			%xmlFile.endRead();
		}
	}
	
	%xmlFile.delete();
	
	//
	_applyPreferenceVideoSettings();
	_applyPreferenceAudioSettings();
}

function _loadInputMap(%xmlFile)
{
	// Loop through all of the classes in this tree
	for (%className = %xmlFile.readNextClass(); %className !$= ""; %className = %xmlFile.readNextClass())
	{
		// Select the index
		%index = $InputMap::Count;
		
		// Check input commands
		if (%className $= "InputCommand")
		{
			%label   = %xmlFile.readField( "Label" );
			%command = %xmlFile.readField( "Command" );
			%value   = %xmlFile.readField( "Value" );

			_setPreference("InputMap::"   @ %command, %value);
			_setPreference("InputLabel::" @ %command, %label);
			
			// Check to see if there is already a command in place
			for (%i = 0; %i < $InputMap::Count; %i++)
			{
				if ($InputMap::Command[%i] $= %command)
				{
					%index = %i;
					break;
				}
			}
		}

		// Store the details
		$InputMap::Type[%index]    = %className;
		$InputMap::Text[%index]    = %xmlFile.fileObject.getText();
		$InputMap::Label[%index]   = %label;
		$InputMap::Command[%index] = %command;
		
		// Check if it is a new field
		if (%index == $InputMap::Count)
			$InputMap::Count += 1;
		
		// End of this class
		%xmlFile.readClassEnd();
	}
}

function _loadVideoSettings(%xmlFile)
{
	// Load the settings
	%mode   = %xmlFile.readField( "Mode" );
	%driver = %xmlFile.readField( "Driver" );
	
	%resolution = %xmlFile.readField( "Resolution" );
	%depth      = %xmlFile.readField( "Depth" );
	%vSync      = %xmlFile.readField( "VerticalSync" );
	
	%screenshot = %xmlFile.readField( "Screenshot" );
	if (%screenshot $= "")
		%screenshot = %xmlFile.readField( "screenShot" );
	
	_setVideoPreferences(%mode, %driver, %resolution, %depth, %vSync, %screenshot);
}

function _setVideoPreferences(%mode, %driver, %resolution, %depth, %vSync, %screenshot)
{
	if (%mode !$= "") 		_setPreference("VideoSettings::Mode", 			%mode);
	if (%driver !$= "") 	_setPreference("VideoSettings::Driver", 		%driver);
	if (%resolution !$= "") _setPreference("VideoSettings::Resolution", 	%resolution);
	if (%depth !$= "") 		_setPreference("VideoSettings::Depth", 			%depth);
	if (%vSync !$= "") 		_setPreference("VideoSettings::VerticalSync", 	%vSync);
	if (%screenshot !$= "") _setPreference("VideoSettings::Screenshot", 	%screenshot);
}

function _loadAudioSettings(%xmlFile)
{
	%masterEnabled 	= %xmlFile.readField( "MasterEnabled" );
	%masterVolume  	= %xmlFile.readField( "MasterVolume" );
	
	%sfxEnabled 	= %xmlFile.readField( "SFXEnabled" );
	%sfxVolume  	= %xmlFile.readField( "SFXVolume" );
	
	%musicEnabled 	= %xmlFile.readField( "MusicEnabled" );
	%musicVolume  	= %xmlFile.readField( "MusicVolume" );
	
	_setAudioPreferences(%masterEnabled, %masterVolume, %sfxEnabled, %sfxVolume, %musicEnabled, %musicVolume);
}

function _setAudioPreferences(%masterEnabled, %masterVolume, %sfxEnabled, %sfxVolume, %musicEnabled, %musicVolume)
{
	if (%masterEnabled !$= "")	_setPreference("AudioSettings::MasterEnabled", 	%masterEnabled);
	if (%masterVolume !$= "")	_setPreference("AudioSettings::MasterVolume", 	%masterVolume);
	if (%sfxEnabled !$= "")		_setPreference("AudioSettings::SFXEnabled", 	%sfxEnabled);
	if (%sfxVolume !$= "")		_setPreference("AudioSettings::SFXVolume", 		%sfxVolume);
	if (%musicEnabled !$= "")	_setPreference("AudioSettings::MusicEnabled", 	%musicEnabled);
	if (%musicVolume !$= "")	_setPreference("AudioSettings::MusicVolume", 	%musicVolume);
}

//-----------------------------------------------------------------------------

function _applyPreferenceVideoSettings()
{
	// Get the saved display settings
	%mode   = _getPreference("VideoSettings::Mode");
	%driver = _getPreference("VideoSettings::Driver");
	
	%resolution = _getPreference("VideoSettings::Resolution");
	%depth      = _getPreference("VideoSettings::Depth");
	%vSync      = _getPreference("VideoSettings::VerticalSync");
	
	%screenshot = _getPreference("VideoSettings::Screenshot");
	
	// Apply the settings
	_applyVideoSettings(%mode, %driver, %resolution, %depth, %vSync, %screenshot);
}

function _applyVideoSettings(%mode, %driver, %resolution, %depth, %vSync, %screenshot)
{
	if (!_checkVideoChanges(%mode, %driver, %resolution, %depth))
		setDisplayDevice(%driver, %resolution.X, %resolution.Y, %depth, %mode);
	
	setVerticalSync(%vSync);
	
	$Pref::Video::ScreenShotFormat = %screenshot;
}

function _checkVideoChanges(%mode, %driver, %resolution, %depth)
{
	// Get the saved display settings
	%currMode   = isFullscreen();
	%currDriver = $Pref::Video::DisplayDevice;
	
	%currResolution = getWords(getRes(), 0, 1);
	%currDepth      = getWord(getRes(), 2);
	
	return (%currMode == %mode && %currDriver $= %driver && %currResolution $= %resolution && %currDepth == %depth);
}

//-----------------------------------------------------------------------------

function _applyPreferenceAudioSettings()
{
	%sMasterVolume = _getPreference("AudioSettings::MasterEnabled") * _getPreference("AudioSettings::MasterVolume");
	%sSoundVolume  = %sMasterVolume * _getPreference("AudioSettings::SFXEnabled") * _getPreference("AudioSettings::SFXVolume");
	%sMusicVolume  = %sMasterVolume * _getPreference("AudioSettings::MusicEnabled") * _getPreference("AudioSettings::MusicVolume");

	$SoundManager::Sound::Volume = %sSoundVolume;
	$SoundManager::Music::Volume = %sMusicVolume;

	setChannelVolume($SoundManager::Sound::Channel, %sSoundVolume);
	setChannelVolume($SoundManager::Music::Channel, %sMusicVolume);
}

function _applyAudioSettings(%masterEnabled, %masterVolume, %sfxEnabled, %sfxVolume, %musicEnabled, %musicVolume)
{
	%sMasterVolume = %masterEnabled * %masterVolume;
	%sSoundVolume  = %sMasterVolume * %sfxEnabled * %sfxVolume;
	%sMusicVolume  = %sMasterVolume * %musicEnabled * %musicVolume;
	
	$SoundManager::Sound::Volume = %sSoundVolume;
	$SoundManager::Music::Volume = %sMusicVolume;
	
	setChannelVolume($SoundManager::Sound::Channel, %sSoundVolume);
	setChannelVolume($SoundManager::Music::Channel, %sMusicVolume);
}

//-----------------------------------------------------------------------------

function _saveOptionsPreferences()
{
	%optionsFile = expandFilename( "./OptionsMenuPreferences.xml" );
	
	%xmlFile = new ScriptObject()
	{
		class = "XML";
	};
	
	if( %xmlFile.beginWrite(%optionsFile) )
	{
		// Lead in
		%xmlFile.writeClassBegin( "MenuConfiguration" );
		
		// Save the settings
		_saveInputMap(%xmlFile);
		_saveSettings(%xmlFile, "VideoSettings");
		_saveSettings(%xmlFile, "AudioSettings");
		
		// Lead out
		%xmlFile.writeClassEnd();
		
		%xmlFile.endWrite();
	}
}

function _saveInputMap(%xmlFile)
{
	%xmlFile.writeClassBegin( "InputMap" );
	
	%commandCount = $InputMap::Count;
	for (%i = 0; %i < %commandCount; %i++)
	{
		if ($InputMap::Type[%i] $= "InputCommand")
		{
			%xmlFile.writeClassBegin($InputMap::Type[%i]);
			
			%xmlFile.writeField("Label", $InputMap::Label[%i]);
			%xmlFile.writeField("Command", $InputMap::Command[%i]);
			
			%keyBind = _getPreference("InputMap::" @ $InputMap::Command[%i]);
			%xmlFile.writeField("Value", strupr(%keyBind));
			
			%xmlFile.writeClassEnd();
		}
	}
	
	%xmlFile.writeClassEnd();
}

function _saveSettings(%xmlFile, %settingName)
{
	%xmlFile.writeClassBegin( %settingName );
	
	%settingCount = _getPreferenceCount( %settingName );
	for (%i = 0; %i < %settingCount; %i++)
	{
		%settingField = _getPreferenceName( %settingName, %i );
		%settingValue = _getPreference(%settingName @ "::" @ %settingField);
		
		%xmlFile.writeField(%settingField, %settingValue);
	}
	
	%xmlFile.writeClassEnd();
}