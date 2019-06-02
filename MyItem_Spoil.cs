using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void OnConsumeItem( Item item, Player player ) {
			if( this.NeedsSaving( item ) && item.buffType == BuffID.WellFed ) {
				this.ModifyBuffDuration( item, player );
			}
		}


		////////////////

		public int ComputeRemainingFreshnessDurationTicks( Item item ) {
			if( !this.NeedsSaving( item ) ) {
				return -1;
			}

			if( this.Timestamp == 0 ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;
			long now = SystemHelpers.TimeStampInSeconds();
			int spoilageTicks;

			if( item.buffType == BuffID.WellFed ) {
				spoilageTicks = (int)( (now - this.Timestamp) * mymod.Config.FoodSpoilageRateScale );
				int buffTime = item.buffTime - (spoilageTicks * 60);
				
				return buffTime;
			} else {
				spoilageTicks = (int)( (now - this.Timestamp) * mymod.Config.FoodIngredientSpoilageRatePerSecond );

				return Math.Max( 0, mymod.Config.FoodIngredientSpoilageTickDuration - (spoilageTicks * 60) );
			}
		}

		public int ComputeMaxFreshnessDurationTicks( Item item ) {
			if( !this.NeedsSaving( item ) ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;
			int ticks;

			if( item.buffType == BuffID.WellFed ) {
				ticks = (int)MathHelper.Clamp( item.buffTime, mymod.Config.FoodSpoilageMinDuration, mymod.Config.FoodSpoilageMaxDuration );
			} else {
				ticks = mymod.Config.FoodIngredientSpoilageTickDuration;
			}

			return ticks;
		}


		////////////////

		private void ModifyBuffDuration( Item item, Player player ) {
			var mymod = (StarvationMod)this.mod;
			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 && mymod.Config.FoodSpoilageRateScale > 0f ) {
				int newBuffTime = this.ComputeRemainingFreshnessDurationTicks( item );

				if( newBuffTime != -1 ) {
					if( player.buffTime[buffIdx] < newBuffTime ) {
						player.buffTime[buffIdx] = newBuffTime;
					} else if( player.buffTime[buffIdx] == item.buffTime ) {
						player.buffTime[buffIdx] = newBuffTime;
					}
				}
			}
		}
	}
}
