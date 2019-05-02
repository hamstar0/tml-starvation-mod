using HamstarHelpers.Helpers.ItemHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;



		////////////////

		private int PerishableItemId = 0;
		private int StoredItemCount;
		private int DurationOfExistence;

		private int Ticks = 0;
		private DateTime PrevDate = DateTime.UtcNow;


		////////////////

		public override bool CloneNewInstances => true;



		////////////////

		public override void SetStaticDefaults() {
			var mymod = (StarvationMod)this.mod;

			this.DisplayName.SetDefault( "Tupperware" );

			string tooltip = "Stores a stack of a given food or ingredient";
			if( mymod.Config.FoodSpoilageEnabled ) {
				tooltip += "\nHelps preserve food against spoilage";
			}

			this.Tooltip.SetDefault( tooltip );
		}

		public override void SetDefaults() {
			this.item.width = TupperwareItem.Width;
			this.item.height = TupperwareItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
			this.item.rare = 2;
		}


		////////////////

		public override void Update( ref float gravity, ref float maxFallSpeed ) {
			this.UpdateSpoilage();
		}

		public override void UpdateInventory( Player player ) {
			this.UpdateSpoilage();
		}


		////////////////

		public override bool CanRightClick() {
			return this.PerishableItemId != 0 && this.StoredItemCount > 0;
		}

		public override void RightClick( Player player ) {
			var mymod = (StarvationMod)this.mod;
			int itemId = ItemHelpers.CreateItem( player.Center, this.PerishableItemId, 1, 16, 16 );
			var itemInfo = Main.item[ itemId ].GetGlobalItem<StarvationItem>();
			//float spoilagePercent = (float)this.SpoilageAmount / (float)mymod.Config.FoodIngredientSpoilageDuration;

			itemInfo.DurationOfExistence = this.DurationOfExistence;

			this.StoredItemCount--;
		}


		////////////////

		private void UpdateSpoilage() {
			DateTime now = DateTime.UtcNow;
			TimeSpan diff = now - this.PrevDate;

			if( diff.TotalSeconds >= 1d ) {
				this.PrevDate = now;
				this.Ticks = 0;
			} else if( this.Ticks < 60 ) {
				this.DurationOfExistence++;
				this.Ticks++;
			}
		}


		////////////////

		public bool CanAddItem( Item item ) {
			if( this.PerishableItemId != -1 && this.PerishableItemId != item.type ) {
				return false;
			}
			if( item.stack > 1 ) {
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			if( this.StoredItemCount >= this.item.maxStack ) {
				return false;
			}

			var myitem = item.GetGlobalItem<StarvationItem>();
			if( myitem.ComputeRemainingBuffTime(item) <= 0 ) {
				return false;
			}

			return true;
		}
		
		internal void AddItem( Item item ) {
			this.StoredItemCount++;
		}
	}
}
