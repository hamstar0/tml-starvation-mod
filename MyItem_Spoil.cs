using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void OnConsumeItem( Item item, Player player ) {
			if( this.NeedsSaving( item ) ) {
				this.ModifyBuffDuration( item, player );
			}
		}


		////////////////

		public int ComputeBuffTime( Item item ) {
			if( !this.NeedsSaving( item ) ) { return -1; }

			var mymod = (StarvationMod)this.mod;

			int buffTime = item.buffTime - (int)( (float)this.AverageDuration * mymod.Config.FoodSpoilageRate );

			return Math.Max( 0, buffTime );
		}


		////////////////

		private void ModifyBuffDuration( Item item, Player player ) {
			var mymod = (StarvationMod)this.mod;
			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 && mymod.Config.FoodSpoilageRate > 0 ) {
				int newBuffTime = this.ComputeBuffTime( item );

				if( player.buffTime[buffIdx] < newBuffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				} else if( player.buffTime[buffIdx] == item.buffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				}
			}
		}
	}
}
