//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( PlayerActorBaseTemplate )
{
    SuperClass        = "PlayerClass";
    Layer             = "1";
    
    _Behavior0        = "ControllerBehavior";
};

datablock t2dSceneObjectDatablock ( WaterConfig )
{
	imageMap = whiteImageMap;
};

datablock AudioDescription( MusicLoop )
{
    volume    = 100.0;
    type      = 1;
    isLooping = true;
    is3D      = false;
};

datablock AudioProfile( GoodMusic )
{
    filename    = "game/data/audio/noman.wav";
    description	= MusicLoop;
    preload     = true;

    Alternate   = "GooderMusic";
};


datablock AudioProfile( GooderMusic )
{
    filename    = "./data/audio/noman.ogg";
    description	= MusicLoop;
    preload     = true;

    Alternate   = "GoodMusic";
};

//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( BoomBotActorTemplate : PlayerActorBaseTemplate )
{
    Class             = "BoomBotClass";
    ActorType         = "BoomBot";
    
    Size              = "20.000 20.000";
    CollisionPolyList = "-0.300 -0.500 0.300 -0.500 0.400 0.250 0.000 0.770 -0.300 0.400";

    Gravity           = "0 150";
    UpdateDirection   = "0";

    AnimationData     = "BoomBotActorAnimationData";
    _Behavior0        = "ControllerBehavior\tkeyLeft\tkeyboard a\tkeyRight\tkeyboard d\tkeyUp\tkeyboard w\tkeyDown\tkeyboard s";
};

datablock SimDataBlock( BoomBotActorAnimationData )
{
    // t2dShape3D properties
    Shape                 = "game/data/shapes/players/BoomBot/BoomBot.dts";
    ShapeRotation         = "0.0 0.0 4.71239";
    ShapeScale            = "0.4 0.4 0.4";

    ShapeRotationRight    = "0.0 0.0 4.71239";
    ShapeRotationLeft     = "0.0 0.0 1.57080";

    IdleAnimation         = "root";
    RunBackwardsAnimation = "back";
};

//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( DragonActorTemplate : PlayerActorBaseTemplate )
{
    Class             = "DragonPlayer";
    ActorType         = "Dragon";
    
    Size              = "16.000 16.000";
    CollisionPolyList = "-0.300 -0.250 0.300 -0.250 0.400 0.250 0.000 0.480 -0.300 0.400";
};

datablock t2dSceneObjectDatablock ( SadActorTemplate : PlayerActorBaseTemplate )
{
	Class             = "SadPlayer";
    ActorType         = "Sad";
	
      CollisionPolyList = "-1.000 -0.746 1.000 -0.746 1.000 0.565 -0.516 0.913 -1.000 0.717";
	SIze			  = "10.0 11.0";
	CollisionResponseMode = "CLAMP";
	SrcBlendFactor = "ONE";
	DstBlendFactor = "SRC_ALPHA";
	_Behavior0        = "ControllerBehavior\tkeyLeft\tkeyboard a\tkeyRight\tkeyboard d\tkeyUp\tkeyboard w\tkeyDown\tkeyboard s";
	      SrcBlendFactor = "ONE";
      DstBlendFactor = "SRC_ALPHA";
      BlendColor = "0 0.0862745 1 1";
};

datablock t2dSceneObjectDatablock( CavemanActorTemplate : PlayerActorBaseTemplate )
{
    Class             = "CavemanPlayer";
    ActorType         = "Caveman";
    
    Size              = "20.000 20.000";
    CollisionPolyList = "-0.300 -0.250 0.300 -0.250 0.400 0.250 0.000 0.480 -0.300 0.400";
};

//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock (SpecialWordConfig)
{
	class = "SpecialWord";
};

datablock t2dSceneObjectDatablock ( SpecialObjectConfig )
{
	class = SpecialObject;
};

datablock t2dSceneObjectDatablock( DrillActorTemplate )
{
    Class             = "DrillClass";
    Size              = "16.000 12.000";
    Layer             = "2";
    CollisionPolyList = "-0.550 -0.500 0.170 -0.500 0.170 0.220 -0.190 0.700 -0.550 0.220";
    ActorType         = "Drill";
    _Behavior0        = "AIControllerBehavior\tAIType\tDrill";
    
    AnimationData     = "DrillAnimationData";
    SoundData         = "DrillSoundData";
    
    MaxMoveSpeed      = 16;
    GroundAccel       = 1000;
    GroundDecel       = 1000;
    
    AllowRespawn      = false;
    DeathTimeOut      = 500;
    
    InnerRadius       = 20;
    OuterRadius       = 80;
};

datablock SimDataBlock( DrillAnimationData )
{
    RunAnimation      = "DrillIdleAnimation";
};

datablock SimDataBlock( DrillSoundData )
{
    RunSound          = "DrillIdleSound";
};

datablock t2dSceneObjectDatablock( DrillHeadTemplate )
{
    Size              = "16.000 12.000";
    CollisionPolyList = "0.170 -0.400 0.700 0.000 0.170 0.150";
    _Behavior0        = "AreaDamageBehavior\tPlayerOnly\t1\tAmount\t20";
};

datablock t2dSceneObjectDatablock( PepperTemplate )
{
    AnimationName     = "yummy_pepperAnimation";
    Size              = "6.000 6.000";
    Layer             = "5";
    _Behavior0        = "PickupBehavior";
    _Behavior1        = "PepperPickupBehavior";
};

//-----------------------------------------------------------------------------

new GuiControlProfile( CrossHairProfile )
{
    Opaque = "0";
    Border = "0";
    Modal  = "0";
};