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
			int freshnessDuration = this.ComputeRemainingFreshnessDurationTicks( item );
			if( freshnessDuration == -1 ) {
				return;
			}

			int maxFreshnessDuration = this.ComputeMaxFreshnessDurationTicks( item );
			if( maxFreshnessDuration == -1 ) {
				return;
			}

			float freshness = (float)freshnessDuration / (float)maxFreshnessDuration;
			if( freshness >= 1f ) {
				return;
			}

			float spoilage = 1f - freshness;

			int spoiledAmt = ( maxFreshnessDuration - freshnessDuration ) / 60;
			string spoiledFmt;

			if( spoiledAmt <= 60 ) {
				spoiledFmt = spoiledAmt + "s";
			} else {
				spoiledFmt = ( spoiledAmt / 60 ) + "m";
			}

			var tip1 = new TooltipLine( this.mod,
				"SpoilageRate",
				"Loses " + mymod.Config.FoodSpoilageRatePerSecond + "s duration to spoilage every second"
			);

			string tip2Text;
			Color tip2Color;
			if( spoilage < 1f ) {
				tip2Text = spoiledFmt + " of 'Well Fed' duration lost.";
				tip2Color = Color.Lerp( Color.Lime, Color.Red, spoilage );
			} else {
				tip2Text = "Spoiled!";
				tip2Color = new Color( 64, 96, 32 );
			}

			var tip2 = new TooltipLine( this.mod, "SpoilageAmount", tip2Text );
			tip2.overrideColor = tip2Color;

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );

			if( mymod.Config.DebugModeInfo ) {
				tooltips.Add( new TooltipLine( mymod, "SpoilageDEBUG", "maxf:"+maxFreshnessDuration+", f:"+freshnessDuration ) );
			}
		}
	}
}
