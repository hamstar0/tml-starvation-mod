using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.ItemHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public override bool CanRightClick() {
			return this.StoredItemType > 0 && this.StoredItemStackSize > 0;
		}

		public override void RightClick( Player player ) {
			var mymod = (StarvationMod)this.mod;
			int newItemIdx = ItemHelpers.CreateItem( player.Center, this.StoredItemType, 1, 16, 16 );
			var newItemInfo = Main.item[ newItemIdx ].GetGlobalItem<StarvationItem>();
			//float spoilagePercent = (float)this.SpoilageAmount / (float)mymod.Config.FoodIngredientSpoilageDuration;

			newItemInfo.Timestamp = this.Timestamp;

			this.StoredItemStackSize--;
			this.item.stack++;
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
			if( myitem.ComputeRemainingFreshnessDuration(item) <= 0 ) {
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
