using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void OnConsumeItem( Item item, Player player ) {
			if( !this.NeedsSaving( item ) ) { return; }

			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 ) {
				int newBuffTime = this.ComputeBuffTime( item );

Main.NewText( "item's: "+item.buffTime+", player's: "+player.buffTime[buffIdx]+", newBuffTime: "+newBuffTime );
				if( player.buffTime[buffIdx] < newBuffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				} else if( player.buffTime[buffIdx] == item.buffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				}
			}
		}


		////////////////

		public int ComputeBuffTime( Item item ) {
			if( !this.NeedsSaving( item ) ) { return -1; }

			int buffTime = item.buffTime - this.AverageDuration;

			return Math.Max( 1, buffTime );
		}
	}
}
