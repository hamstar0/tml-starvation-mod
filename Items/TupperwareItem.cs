using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public static int Width = 20;
		public static int Height = 20;



		////////////////

		private int StoredItemType = 0;
		private int StoredItemStackSize;
		private int DurationOfExistence;

		private int Ticks = 0;
		private DateTime PrevDate = DateTime.UtcNow;

		private Item _CachedItem = null;


		////////////////

		public override bool CloneNewInstances => true;



		////////////////

		public override ModItem Clone() {
			var clone = (TupperwareItem)base.Clone();
			clone.StoredItemType = this.StoredItemType;
			clone.StoredItemStackSize = this.StoredItemStackSize;
			clone.DurationOfExistence = this.DurationOfExistence;
			clone.Ticks = this.Ticks;
			clone.PrevDate = this.PrevDate;
			clone._CachedItem = this._CachedItem;
			return clone;
		}

		public override ModItem Clone( Item item ) {
			var clone = (TupperwareItem)base.Clone( item );
			clone.StoredItemType = this.StoredItemType;
			clone.StoredItemStackSize = this.StoredItemStackSize;
			clone.DurationOfExistence = this.DurationOfExistence;
			clone.Ticks = this.Ticks;
			clone.PrevDate = this.PrevDate;
			clone._CachedItem = this._CachedItem;
			return clone;
		}


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

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("stack") ) {
				return;
			}

			this.StoredItemStackSize = tag.GetInt( "stack" );
			this.StoredItemType = tag.GetInt( "type" );
			this.DurationOfExistence = tag.GetInt( "duration" );
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "stack", this.StoredItemStackSize },
				{ "type", this.StoredItemType },
				{ "duration", this.DurationOfExistence }
			};
		}

		public override void NetRecieve( BinaryReader reader ) {
			this.StoredItemStackSize = reader.ReadInt32();
			this.StoredItemType = reader.ReadInt32();
			this.DurationOfExistence = reader.ReadInt32();
		}

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( (int)this.StoredItemStackSize );
			writer.Write( (int)this.StoredItemType );
			writer.Write( (int)this.DurationOfExistence );
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
			if( this.StoredItemStackSize == 0 ) {
				return 0;
			}
			
			if( this._CachedItem == null || this._CachedItem.type != this.StoredItemType ) {
				this._CachedItem = new Item();
				this._CachedItem.SetDefaults( this.StoredItemType, true );
			}

			var myitem = this._CachedItem.GetGlobalItem<StarvationItem>();
			int spoilageDuration = myitem.ComputeMaxFreshnessDuration( this._CachedItem );

			return 1f - ((float)this.DurationOfExistence / (float)spoilageDuration);
		}
	}
}
