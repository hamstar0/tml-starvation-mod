using HamstarHelpers.Helpers.ItemHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public override bool CanRightClick() {
Main.NewText( "CanRightClick " + this.StoredItemType+" "+this.StoredItemStackSize);
			return this.StoredItemType > 0 && this.StoredItemStackSize > 0;
		}

		public override void RightClick( Player player ) {
			var mymod = (StarvationMod)this.mod;
			int itemIdx = ItemHelpers.CreateItem( player.Center, this.StoredItemType, 1, 16, 16 );
			var itemInfo = Main.item[ itemIdx ].GetGlobalItem<StarvationItem>();
			//float spoilagePercent = (float)this.SpoilageAmount / (float)mymod.Config.FoodIngredientSpoilageDuration;

			itemInfo.DurationOfExistence = this.DurationOfExistence;

			this.StoredItemStackSize--;
Main.NewText( "RightClick " + itemIdx + " "+this.StoredItemStackSize);
		}


		////////////////
		
		public bool CanAddItem( Item item ) {
			if( (this.StoredItemStackSize > 0 && this.StoredItemType != -1) && this.StoredItemType != item.type ) {
Main.NewText("1 "+this.StoredItemType);
				return false;
			}
			if( item.stack > 1 ) {
Main.NewText("2");
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			if( this.StoredItemStackSize >= mymod.Config.TupperwareMaxStackSize ) {
Main.NewText("3");
				return false;
			}

			var myitem = item.GetGlobalItem<StarvationItem>();
			if( myitem.ComputeRemainingFreshnessDuration(item) <= 0 ) {
Main.NewText("4 "+myitem.ComputeRemainingFreshnessDuration(item));
				return false;
			}

			return true;
		}
		
		internal void AddItem( Item item ) {
			this.StoredItemType = item.type;
			this.StoredItemStackSize++;
Main.NewText("added "+item.HoverName+" "+this.StoredItemStackSize);
		}
	}
}
