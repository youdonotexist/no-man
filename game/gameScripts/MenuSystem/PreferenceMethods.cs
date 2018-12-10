//-----------------------------------------------------------------------------
//  TGB Menu System
//  Copyright (C) Phillip O'Shea
//  
//  Preference Methods - 
//-----------------------------------------------------------------------------

// Variables
	$GamePreferences = 0;

// Functions
	
	
//-----------------------------------------------------------------------------

function _setPreference(%prefName, %value)
{
	%explodeScript    = explode(%prefName, "::");
	%preferenceParent = %explodeScript.Contents[%explodeScript.Count - 2];
	if (%preferenceParent !$= "")
	{
		%preferenceObject = %preferenceParent @ "PreferenceScript";
		if (!isObject(%preferenceObject))
			new ScriptObject(%preferenceObject);

		%preferenceObject.setFieldValue(%explodeScript.Contents[%explodeScript.Count - 1], %value);
		return;
	}
	
	eval ("$GamePreferences::" @ %prefName @ " = \"" @ %value @ "\";");
}

function _getPreferenceCount(%prefName)
{
	%explodeScript    = explode(%prefName, "::");
	%preferenceParent = %explodeScript.Contents[%explodeScript.Count - 1];
	if (%preferenceParent !$= "")
	{
		%preferenceObject = %preferenceParent @ "PreferenceScript";
		if (!isObject(%preferenceObject))
			return 0;
		
		return %preferenceObject.getDynamicFieldCount();
	}
}

function _getPreferenceName(%prefName, %index)
{
	%explodeScript    = explode(%prefName, "::");
	%preferenceParent = %explodeScript.Contents[%explodeScript.Count - 1];
	if (%preferenceParent !$= "")
	{
		%preferenceObject = %preferenceParent @ "PreferenceScript";
		if (!isObject(%preferenceObject))
			return "";
		
		%fieldName = %explodeScript.Contents[%explodeScript.Count - 1];
		if (%index !$= "")
			%fieldName = %preferenceObject.getDynamicField(%index);
				
		return %fieldName;
	}
	
	return "";
}

function _getPreference(%prefName)
{
	%explodeScript    = explode(%prefName, "::");
	%preferenceParent = %explodeScript.Contents[%explodeScript.Count - 2];
	if (%preferenceParent !$= "")
	{
		%preferenceObject = %preferenceParent @ "PreferenceScript";
		if (!isObject(%preferenceObject))
			return "";
		
		%fieldName = %explodeScript.Contents[%explodeScript.Count - 1];
		if (%index !$= "")
			%fieldName = %preferenceObject.getDynamicField(%index);
				
		return %preferenceObject.getFieldValue(%fieldName);
	}
	
	eval ("return $GamePreferences::" @ %prefName @ ";");
}