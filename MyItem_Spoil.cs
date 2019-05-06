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

		public int ComputeRemainingFreshnessDuration( Item item ) {
			if( !this.NeedsSaving( item ) ) { return -1; }

			var mymod = (StarvationMod)this.mod;
			long now = SystemHelpers.TimeStampInSeconds();
			int spoilage;

			if( item.buffType == BuffID.WellFed ) {
				spoilage = (int)( (now - this.Timestamp) * mymod.Config.FoodSpoilageRate );
				int buffTime = item.buffTime - spoilage;

				return Math.Max( 0, buffTime );
			} else {
				spoilage = (int)( (now - this.Timestamp) * mymod.Config.FoodIngredientSpoilageRate );

				return mymod.Config.FoodIngredientSpoilageDuration - spoilage;
			}
		}

		public int ComputeMaxFreshnessDuration( Item item ) {
			if( !this.NeedsSaving( item ) ) { return -1; }

			if( item.buffType == BuffID.WellFed ) {
				return item.buffTime;
			}

			var mymod = (StarvationMod)this.mod;
			return mymod.Config.FoodIngredientSpoilageDuration;
		}


		////////////////

		private void ModifyBuffDuration( Item item, Player player ) {
			var mymod = (StarvationMod)this.mod;
			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 && mymod.Config.FoodSpoilageRate > 0f ) {
				int newBuffTime = this.ComputeRemainingFreshnessDuration( item );

				if( player.buffTime[buffIdx] < newBuffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				} else if( player.buffTime[buffIdx] == item.buffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				}
			}
		}
	}
}
