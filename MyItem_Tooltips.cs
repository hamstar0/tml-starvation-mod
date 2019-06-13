using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( this.NeedsSaving( item ) ) {
				this.AddSpoilageTips( item, tooltips );
			}
		}


		////////////////

		private void AddSpoilageTips( Item item, List<TooltipLine> tooltips ) {
			var mymod = (StarvationMod)this.mod;
			int freshnessDuration = this.ComputeTimeLeftTicks( item );
			if( freshnessDuration == -1 ) {
				return;
			}

			int maxFreshnessDuration = this.ComputeMaxElapsedTicks( item );
			if( maxFreshnessDuration == -1 ) {
				return;
			}

			float freshnessPercent = (float)freshnessDuration / (float)maxFreshnessDuration;
			if( freshnessPercent >= 1f ) {
				return;
			}

			float spoilagePercent = 1f - freshnessPercent;

			int spoiledAmt = ( maxFreshnessDuration - freshnessDuration ) / 60;
			string spoiledFmt;

			if( spoiledAmt <= 60 ) {
				spoiledFmt = spoiledAmt + "s";
			} else {
				spoiledFmt = ( spoiledAmt / 60 ) + "m";
			}

			var tip1 = new TooltipLine( this.mod,
				"SpoilageRate",
				"Loses " + Math.Round( mymod.Config.FoodSpoilageRateScale, 2 ) + "s freshness every second"
			);

			string tip2Text;
			Color tip2Color;
			if( spoilagePercent < 1f ) {
				tip2Text = spoiledFmt + " of 'Well Fed' duration lost.";
				tip2Color = Color.Lerp( Color.Lime, Color.Red, spoilagePercent );
			} else {
				tip2Text = "Spoiled!";
				tip2Color = new Color( 64, 96, 32 );
			}

			var tip2 = new TooltipLine( this.mod, "SpoilageAmount", tip2Text );
			tip2.overrideColor = tip2Color;

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );

			if( mymod.Config.DebugModeInfo ) {
				tooltips.Add( new TooltipLine( mymod, "SpoilageDEBUG", "maxfresh:"+maxFreshnessDuration+", fresh:"+freshnessDuration ) );
			}
		}
	}
}
