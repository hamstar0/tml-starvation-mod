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

		private Item _CachedItem = null;


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

		public float ComputeFreshnessPercent() {
			if( this.StoredItemCount == 0 ) {
				return 0;
			}
			
			if( this._CachedItem == null || this._CachedItem.type != this.PerishableItemId ) {
				this._CachedItem = new Item();
				this._CachedItem.SetDefaults( this.PerishableItemId, true );
			}

			var myitem = this._CachedItem.GetGlobalItem<StarvationItem>();
			int spoilageDuration = myitem.ComputeMaxFreshnessDuration( this._CachedItem );

			return (float)this.DurationOfExistence / (float)spoilageDuration;
		}
	}
}
