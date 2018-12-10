//-----------------------------------------------------------------------------
//  Platformer Starter Kit
//  Copyright (C) Phillip O'Shea
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
//
// Audio
//
//------------------------------------------------------------------------------

datablock AudioDescription( SoundOnce )
{
    volume    = 1.0;
    type      = 0;
    isLooping = false;
    is3D      = false;
};

datablock AudioDescription( SoundLoop )
{
    volume    = 1.0;
    type      = 0;
    isLooping = true;
    is3D      = false;
};

datablock AudioDescription( MusicLoop )
{
    volume    = 1.0;
    type      = 1;
    isLooping = true;
    is3D      = false;
};

datablock AudioProfile( PepperPickupSound )
{
    filename    = "./data/audio/general/pepper_pickup.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( CheckPointPickupSound )
{
    filename    = "./data/audio/general/checkpoint_reached.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( MushroomBounceSound )
{
    filename    = "./data/audio/dragon/land_mushroom.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( IndoorMusic )
{
    filename    = "./data/audio/music/indoor_music.wav";
    description	= MusicLoop;
    preload     = true;

    Alternate   = "OutdoorMusic";
};

datablock AudioProfile( OutdoorMusic )
{
    filename    = "./data/audio/music/outdoor_music.wav";
    description	= MusicLoop;
    preload     = true;

    Alternate   = "IndoorMusic";
};

datablock AudioProfile( LevelCompleteMusic )
{
    filename    = "./data/audio/music/finish_level_loop.wav";
    description	= MusicLoop;
    preload     = true;
};