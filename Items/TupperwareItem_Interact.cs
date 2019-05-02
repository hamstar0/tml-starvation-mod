using HamstarHelpers.Helpers.ItemHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public override bool CanRightClick() {
			return this.StoredItemType != 0 && this.StoredItemStackSize > 0;
		}

		public override void RightClick( Player player ) {
			var mymod = (StarvationMod)this.mod;
			int itemId = ItemHelpers.CreateItem( player.Center, this.StoredItemType, 1, 16, 16 );
			var itemInfo = Main.item[ itemId ].GetGlobalItem<StarvationItem>();
			//float spoilagePercent = (float)this.SpoilageAmount / (float)mymod.Config.FoodIngredientSpoilageDuration;

			itemInfo.DurationOfExistence = this.DurationOfExistence;

			this.StoredItemStackSize--;
		}


		////////////////
		
		public bool CanAddItem( Item item ) {
			if( this.StoredItemType != -1 && this.StoredItemType != item.type ) {
				return false;
			}
			if( item.stack > 1 ) {
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			if( this.StoredItemStackSize >= this.item.maxStack ) {
				return false;
			}

			var myitem = item.GetGlobalItem<StarvationItem>();
			if( myitem.ComputeRemainingFreshnessDuration(item) <= 0 ) {
				return false;
			}

			return true;
		}
		
		internal void AddItem( Item item ) {
			this.StoredItemStackSize++;
		}
	}
}
