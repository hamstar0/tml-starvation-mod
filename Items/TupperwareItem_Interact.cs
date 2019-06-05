﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public override bool ConsumeItem( Player player ) {
			return false;
		}

		public override bool CanRightClick() {
			/*Item myItem = this.item;
			Timers.SetTimer( "TupperwareRightClickStackRevert_"+this.item.GetHashCode(), 2, () => {
				myItem.stack = 1;
				return false;
			} );

			this.item.stack = 2;*/
			return this.StoredItemType > 0 && this.StoredItemStackSize > 0;
		}

		public override void RightClick( Player player ) {
			var mymod = (StarvationMod)this.mod;

			int newItemIdx = ItemHelpers.CreateItem( player.Center, this.StoredItemType, 1, 16, 16 );
			Item newItem = Main.item[newItemIdx];
			var newItemInfo = newItem.GetGlobalItem<StarvationItem>();

			float freshPercent = this.ComputeContainedItemsFreshnessPercent();
			float maxFreshness = newItemInfo.ComputeMaxFreshnessDurationTicks( newItem );
			float remainingDuration = maxFreshness * freshPercent;

			newItemInfo.Timestamp = (int)(this.Timestamp - maxFreshness + remainingDuration);

			this.StoredItemStackSize--;
		}


		////////////////
		
		public bool CanAddItem( Item item ) {
			if( (this.StoredItemStackSize > 0 && this.StoredItemType != -1) && this.StoredItemType != item.type ) {
				return false;
			}
			if( item.stack > 1 ) {
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			if( this.StoredItemStackSize >= mymod.Config.TupperwareMaxStackSize ) {
				return false;
			}

			var myitem = item.GetGlobalItem<StarvationItem>();
			if( myitem.ComputeRemainingFreshnessDurationTicks(item) <= 0 ) {
				return false;
			}

			return true;
		}
		
		internal void AddItem( Item item ) {
			if( this.StoredItemStackSize > 0 && this.StoredItemType != item.type ) {
				throw new HamstarException("Tupperware cannot hold this type of item.");
			}

			var myitem = item.GetGlobalItem<StarvationItem>();

			if( this.StoredItemStackSize > 0 ) {
				this.Timestamp = ((long)this.StoredItemStackSize * this.Timestamp) + myitem.Timestamp;
				this.Timestamp /= (long)this.StoredItemStackSize + 1L;
			} else {
				this.Timestamp = myitem.Timestamp;
			}

			this.StoredItemType = item.type;
			this.StoredItemStackSize++;
		}
	}
}
