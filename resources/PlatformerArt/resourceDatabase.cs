//-----------------------------------------------------------------------------
//  Platformer Starter Kit
//  Copyright ( C ) Phillip O'Shea
//  
//  Platformer Art - All of the art used in the PSK Demo
//-----------------------------------------------------------------------------

// Create Resource Descriptor
$instantResource = new ScriptObject()
{
   Class          = "PlatformerArt";
   Name           = "PlatformerArt";
   User           = "TGB";
   LoadFunction	  = "PlatformerArt::LoadResource";
   UnloadFunction = "PlatformerArt::UnloadResource";
};

// Load Resource Function
function PlatformerArt::LoadResource( %this )
{
	exec( "./AudioData.cs" );
	
	//exec( "./CavemanData.cs" );
	//exec( "./DragonData.cs" );
	exec( "./SadData.cs" );
	//exec( "./DrillData.cs" );
	//exec( "./BoomBotData.cs" );
}

// Unload Resource Function
function PlatformerArt::UnloadResource( %this )
{
    // Remove all of the objects we use
    if ( isObject( %this.Data ) && %this.Data.getCount() > 0 )
    {      
        while ( %this.Data.getCount() > 0 )
        {
            %datablockObj = %this.Data.getObject( 0 );
            
            %this.Data.remove( %datablockObj );
            %datablockObj.delete();
        }
    }
}

// Resource Data
$instantResource.Data = new SimGroup() 
{
   //---------------------------------------------------------------------------
   // Graphics
   //---------------------------------------------------------------------------
   
};