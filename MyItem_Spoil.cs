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

		public int ComputeMaxElapsedTicks( Item item ) {
			if( !this.NeedsSaving( item ) ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;
			int ticks;

			if( item.buffType == BuffID.WellFed ) {
				ticks = (int)MathHelper.Clamp( item.buffTime, mymod.Config.FoodSpoilageMinTickDuration, mymod.Config.FoodSpoilageMaxTickDuration );
			} else {
				ticks = mymod.Config.FoodIngredientSpoilageTickDuration;
			}

			return ticks;
		}

		public int ComputeTimeLeftTicks( Item item ) {
			if( !this.NeedsSaving( item ) ) {
				return -1;
			}

			if( this.TimestampInSeconds == 0 ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;
			long now = SystemHelpers.TimeStampInSeconds();
			float elapsedSeconds = (float)( now - this.TimestampInSeconds );

			if( item.buffType == BuffID.WellFed ) {
				int spoilageSeconds = (int)( elapsedSeconds * mymod.Config.FoodSpoilageRateScale );
				int buffTime = item.buffTime - (spoilageSeconds * 60);
				
				return (int)MathHelper.Clamp( buffTime, 0, this.ComputeMaxElapsedTicks(item) );
			} else {
				int spoilageSeconds = (int)( elapsedSeconds * mymod.Config.FoodIngredientSpoilageRateScale );
				int spoilTime = mymod.Config.FoodIngredientSpoilageTickDuration - ( spoilageSeconds * 60 );

				return (int)MathHelper.Clamp( spoilTime, 0, this.ComputeMaxElapsedTicks(item) );
			}
		}


		////////////////

		private void ModifyBuffDuration( Item item, Player player ) {
			var mymod = (StarvationMod)this.mod;
			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 && mymod.Config.FoodSpoilageRateScale > 0f ) {
				int newBuffTime = this.ComputeTimeLeftTicks( item );

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
