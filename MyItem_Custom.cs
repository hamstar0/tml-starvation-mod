using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public void ApplyWellFedModifiers( Item item ) {
			var mymod = (StarvationMod)this.mod;

			switch( item.type ) {
			case ItemID.CookedMarshmallow:
				if( mymod.Config.CustomCookedMarshmallowWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomCookedMarshmallowWellFedDuration;
				}
				break;
			case ItemID.BowlofSoup:
				if( mymod.Config.CustomBowlOfSoupWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomBowlOfSoupWellFedDuration;
				}
				break;
			case ItemID.PumpkinPie:
				if( mymod.Config.CustomPumpkinPieWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomPumpkinPieWellFedDuration;
				}
				break;
			case ItemID.CookedFish:
				if( mymod.Config.CustomCookedFishWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomCookedFishWellFedDuration;
				}
				break;
			case ItemID.CookedShrimp:
				if( mymod.Config.CustomCookedShrimpWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomCookedShrimpWellFedDuration;
				}
				break;
			case ItemID.Sashimi:
				if( mymod.Config.CustomSashimiWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomSashimiWellFedDuration;
				}
				break;
			case ItemID.GrubSoup:
				if( mymod.Config.CustomGrubSoupWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomGrubSoupWellFedDuration;
				}
				break;
			case ItemID.PadThai:
				if( mymod.Config.CustomPadThaiWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomPadThaiWellFedDuration;
				}
				break;
			case ItemID.Pho:
				if( mymod.Config.CustomPhoWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomPhoWellFedDuration;
				}
				break;
			case ItemID.GingerbreadCookie:
				if( mymod.Config.CustomGingerbreadCookieWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomGingerbreadCookieWellFedDuration;
				}
				break;
			case ItemID.SugarCookie:
				if( mymod.Config.CustomSugarCookieWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomSugarCookieWellFedDuration;
				}
				break;
			case ItemID.ChristmasPudding:
				if( mymod.Config.CustomChristmasPuddingWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomChristmasPuddingWellFedDuration;
				}
				break;
			case ItemID.Bacon:
				if( mymod.Config.CustomBaconWellFedDuration > 0 ) {
					item.buffTime = mymod.Config.CustomBaconWellFedDuration;
				}
				break;
			}
		}
	}
}
