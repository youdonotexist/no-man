//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------
//
// This is the file you should define your custom gui profiles that are to be used
// in the editor.
//

new GuiControlProfile( GuiModelessTransparentProfile : GuiTransparentProfile )
{
    modal = false;
};

new GuiControlProfile( GuiExtraLifeCounter : GuiModelessTransparentProfile )
{
    fontType          = "David";
    fontColor         = "255 255 255";
    fontSize          = 72;
};

new GuiControlProfile( AmmoTextProfile : GuiText24Profile )
{
    justify = "right";
    fontColor = "255 255 255 255";
};