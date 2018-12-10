//-----------------------------------------------------------------------------
//  TGB Menu System
//  Copyright (C) Phillip O'Shea
//  
//  Menu System
//-----------------------------------------------------------------------------

// Variables
	$MenuConfiguration::GUIControl = MainMenuGui;
	
// Functions
	function _getGuiControl()		{ return $MenuConfiguration::GUIControl; }
	function _setGuiControl(%obj)	{ $MenuConfiguration::GUIControl = %obj; }

//------------------------------------------------------------------------------

function mMin(%a, %b)
{
	if (%a < %b)
		return %a;
	
	return %b;
}

function mMax(%a, %b)
{
	if (%a > %b)
		return %a;
	
	return %b;
}

function explode(%string, %char)
{
	if (!isObject(explode))
	{
		new ScriptObject(explode);
	}

	%explodeCount = 0;
	%lastFound = 0;

	%endChar = strLen(%string);	
	%charLen = strLen(%char);

	for (%i = 0; %i < %endChar; %i++)
	{
		%charToCheck = getSubStr(%string, %i, %charLen);
		if (%charToCheck $= %char)
		{
			explode.contents[%explodeCount] = getSubStr(%string, %lastFound, (%i-%lastFound)); 
			%lastFound = %i + %charLen;
			%explodeCount++;
		}	
	}

	explode.contents[%explodeCount] = getSubStr(%string, %lastFound, (%i-%lastFound)); 
	explode.count = %explodeCount + 1;	

	return explode;
}

//------------------------------------------------------------------------------

function _loadMenuConfigurationData(%dataFile)
{
	%xmlFile = new ScriptObject()
	{
		class = "XML";
	};
	
	%dataFile = expandFileName( %dataFile );
	
	if( %xmlFile.beginRead(%dataFile) )
	{
		if( %xmlFile.readClassBegin( "MenuConfiguration" ) )
		{
			_loadMenuData(%xmlFile);
			
			%xmlFile.readClassEnd();
		}
		
		%xmlFile.endRead();
	}
	
	%xmlFile.delete();
}

function _loadMenuData(%xmlFile)
{
	for (%className = %xmlFile.readNextClass(); %className !$= ""; %className = %xmlFile.readNextClass())
	{
		%testString = getSubStr(%className, strlen(%className) - strlen("Element"), strlen("Element"));
		if ( %testString $= "Element" )
		{
			// Type of objects in stack
			%elementType = getSubStr(%className, 0, strlen(%className) - strlen("Element"));
			
			// Create the new element
			_newGuiElement(%xmlFile, _getGuiControl(), %elementType);
		}
		
		%testString = getSubStr(%className, strlen(%className) - strlen("Stack"), strlen("Stack"));
		if ( %testString $= "Stack" )
		{
			// Type of objects in stack
			%elementType = getSubStr(%className, 0, strlen(%className) - strlen("Stack"));
			
			// Create the new stack
			%stackForm = _newGuiStack(%xmlFile, %elementType);
			
			while ( %xmlFile.readClassBegin( %elementType @ "Element", %stackForm.getCount() ) )
			{
				_newGuiElement(%xmlFile, %stackForm, %elementType);
				%xmlFile.readClassEnd();
			}
		}
		
		%xmlFile.readClassEnd();
	}
}

//------------------------------------------------------------------------------

function _newGuiStack(%xmlFile, %elementType)
{
	// Create the form
	%guiForm = new GuiControl()
	{
		Profile = GuiTransparentProfile;
	};
	
	// Load any defined fields
	_loadElementFields(%xmlFile, %guiForm);
	
	// Load default element fields
	if ( %xmlFile.readClassBegin("StackDefaults") )
	{
		_loadElementFields(%xmlFile, %guiForm, false, "mElement");
		%xmlFile.readClassEnd();
	}
	
	_getGuiControl().add(%guiForm);

	return %guiForm;
}

function _loadStackDefaultFields(%xmlFile, %guiForm, %guiElement)
{
	for (%i = 0; %i < %guiForm.getDynamicFieldCount(); %i++)
	{
		%fieldName = %guiForm.getDynamicField(%i);
		if ( getSubStr(%fieldName, 0, strlen("mElement")) !$= "mElement")
			continue;

		%fieldValue = %guiForm.getFieldValue(%fieldName);
		%fieldName  = getSubStr(%fieldName, strlen("mElement"), strlen(%fieldName));
		if (%guiElement.isMethod("set" @ %fieldName))
			eval ("%guiElement.set" @ %fieldName @ "(" @ %fieldValue @ ");");
	}
}

//------------------------------------------------------------------------------

function _newGuiElement(%xmlFile, %guiForm, %elementType)
{
	// Create the specified object
	eval ("%guiElement = new Gui" @ %elementType @ "Ctrl();");
	
	//
	%index = %guiForm.getCount();

	// Set the default position
	if (VectorLen(%guiForm.mElementMargin) > 0.0)
	{
		%elementPosition = t2dVectorScale(%guiForm.mElementMargin, %index);
		%guiElement.setPosition(%elementPosition.x, %elementPosition.y);
	}
	
	// Set the default extent
	%elementExtent = %guiElement.getExtent();
	%formExtent    = %guiForm.getExtent();
	%guiElement.setExtent(mMin(%elementExtent.x, %formExtent.x), mMin(%elementExtent.y, %formExtent.y));
	
	// Load the stack's defaults
	_loadStackDefaultFields(%xmlFile, %guiForm, %guiElement);
	
	// Load all of the element's details
	_loadElementFields(%xmlFile, %guiElement);

	// Add the new element to the form
	%guiForm.add(%guiElement);
	
	// Return it's id
	return %guiElement;
}

function _loadElementFields(%xmlFile, %guiElement, %process, %prefix)
{
	if (%process $= "") %process = true;
		
	// Loop through all of the fields
	for (%fieldName = %xmlFile.readNextClass(); %fieldName !$= ""; %fieldName = %xmlFile.readNextClass())
	{
		// Get the value
		%fieldValue = %xmlFile.fileObject.getText();
		
		// Use an assignment function or store the value
		if (%process && %guiElement.isMethod("set" @ %fieldName))
			eval ("%guiElement.set" @ %fieldName @ "(" @ %fieldValue @ ");");
		else
			%guiElement.setFieldValue(%prefix @ %fieldName, %fieldValue);
		
		%xmlFile.readClassEnd();
	}
}