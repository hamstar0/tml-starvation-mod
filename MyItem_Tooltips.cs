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

			float freshnessPercent;
			if( !this.ComputeTimeLeftPercent( item, out freshnessPercent ) ) {
				return;
			}
			int elapsedTicks;
			if( !this.ComputeElapsedTicks(item, out elapsedTicks) ) {
				return;
			}

			int spoiledAmt = elapsedTicks / 60;
			float spoilagePercent = 1f - freshnessPercent;
			string spoiledFmt;

			if( spoiledAmt <= 60 ) {
				spoiledFmt = spoiledAmt + "s";
			} else {
				spoiledFmt = ( spoiledAmt / 60 ) + "m";
			}

			var tip1 = new TooltipLine( this.mod,
				"SpoilageRate",
				"Loses " + Math.Round( 1f / mymod.Config.FoodSpoilageDurationScale, 2 ) + "s freshness every second"
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
				int maxElapsedTicks;
				this.ComputeMaxElapsedTicks( item, out maxElapsedTicks );
				tooltips.Add( new TooltipLine( mymod, "SpoilageDEBUG", "maxelapsed:"+ maxElapsedTicks + ", elasped:"+elapsedTicks+", (fresh%:"+(freshnessPercent*100f)+")" ) );
			}
		}
	}
}
