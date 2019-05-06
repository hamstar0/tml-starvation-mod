using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
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
			int spoilage;

			if( item.buffType == BuffID.WellFed ) {
				spoilage = (int)( (now - this.Timestamp) * mymod.Config.FoodSpoilageRatePerSecond );
				int buffTime = item.buffTime - (spoilage * 60);

				return Math.Max( 0, buffTime );
			} else {
				spoilage = (int)( (now - this.Timestamp) * mymod.Config.FoodIngredientSpoilageRatePerSecond );

				return Math.Max( 0, mymod.Config.FoodIngredientSpoilageTickDuration - (spoilage * 60) );
			}
		}

		public int ComputeMaxFreshnessDurationTicks( Item item ) {
			if( !this.NeedsSaving( item ) ) {
				return -1;
			}

			if( item.buffType == BuffID.WellFed ) {
				return item.buffTime;
			}

			var mymod = (StarvationMod)this.mod;
			return mymod.Config.FoodIngredientSpoilageTickDuration;
		}


		////////////////

		private void ModifyBuffDuration( Item item, Player player ) {
			var mymod = (StarvationMod)this.mod;
			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 && mymod.Config.FoodSpoilageRatePerSecond > 0f ) {
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
