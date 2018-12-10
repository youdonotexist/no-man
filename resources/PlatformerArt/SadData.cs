//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
//
// Audio
//
//------------------------------------------------------------------------------

datablock AudioProfile( DragonSpawnSound0 )
{
    filename    = "./data/audio/dragon/startup 1a.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonSpawnSound1 )
{
    filename    = "./data/audio/dragon/startup 1b.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonRunSound0 )
{
    filename    = "./data/audio/dragon/grass_step1.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonRunSound1 )
{
    filename    = "./data/audio/dragon/grass_step2.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonRunSound2 )
{
    filename    = "./data/audio/dragon/grass_step3.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonSlideSound )
{
    filename    = "./data/audio/dragon/slide.wav";
    description = "SoundLoop";
    preload     = true;
};

datablock AudioProfile( DragonJumpSound )
{
    filename     = "./data/audio/dragon/jump.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonLandSound )
{
    filename    = "";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonDamageSound )
{
    filename    = "./data/audio/dragon/ouch.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonDieSound0 )
{
    filename    = "./data/audio/dragon/die 1.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonDieSound1 )
{
    filename    = "./data/audio/dragon/die 2.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonClimbUpSound )
{
    filename    = "./data/audio/dragon/rope_climb.wav";
    description = "SoundOnce";
    preload     = true;
};

datablock AudioProfile( DragonClimbDownSound )
{
    filename    = "./data/audio/dragon/rope_slide.wav";
    description = "SoundLoop";
    preload     = true;
};

datablock AudioProfile( FireballImpactSound )
{
    filename    = "./data/audio/weapons/FireballImpact.wav";
    description = "SoundOnce";
    preload     = true;
};

//------------------------------------------------------------------------------
//
// Graphics
//
//------------------------------------------------------------------------------

   new t2dImageMapDatablock(SadIdleImageMap) {
      imageName = "./data/images/player.png";
      imageMode = "CELL";
      frameCount = "-1";
      filterMode = "SMOOTH";
      filterPad = "1";
      preferPerf = "1";
      cellRowOrder = "1";
      cellOffsetX = "0";
      cellOffsetY = "0";
      cellStrideX = "0";
      cellStrideY = "0";
      cellCountX = "-1";
      cellCountY = "-1";
      cellWidth = "183";
      cellHeight = "224";
      preload = "1";
      allowUnload = "0";
   };

new t2dAnimationDatablock( SadActionAnimation )
{
    imageMap = "SadIdleImageMap";
    animationFrames = "0";
    animationTime = "0.0333333";
    animationCycle = "0";
    randomStart = "0";
    startFrame = "0";
};

new t2dAnimationDatablock( SadClimbupAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadClimbDown_to_ClimbIdleAnimation :SadActionAnimation )
{

};

new t2dAnimationDatablock( SadClimbUp_to_ClimbIdleAnimation : SadActionAnimation)
{

};


new t2dAnimationDatablock( SadClimbIdleAnimation : SadActionAnimation)
{

};


new t2dAnimationDatablock( SadFallAnimation : SadActionAnimation)
{
    
};

new t2dAnimationDatablock( SadGlideAnimation : SadActionAnimation)
{
};

new t2dAnimationDatablock( SadDieAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadIdleAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadJumpAnimation : SadActionAnimation)
{

};


new t2dAnimationDatablock( SadJump_to_FallAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadRun_to_FallAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadFall_to_LandAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadrunJump_to_RunFallAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadClimbJump_to_FallAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadClimbJump_to_RunFallAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadFall_to_RunAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadFall_to_IdleAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadLandAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadRunAnimation : SadActionAnimation)
{

};


new t2dAnimationDatablock( SadRun_to_SlideAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadSlideAnimation : SadActionAnimation)
{

};


new t2dAnimationDatablock( SadSlide_to_IdleAnimation: SadActionAnimation)
{

};


new t2dAnimationDatablock( SadClimbDownAnimation : SadActionAnimation)
{

};

new t2dAnimationDatablock( SadSpawnAnimation : SadActionAnimation)
{

};


new t2dAnimationDatablock( SadDamageAnimation : SadActionAnimation)
{

};