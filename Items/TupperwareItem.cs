using HamstarHelpers.Helpers.DotNetHelpers;
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
		private long Timestamp;

		private Item _CachedItem = null;


		////////////////

		public override bool CloneNewInstances => true;



		////////////////

		public override ModItem Clone() {
			var clone = (TupperwareItem)base.Clone();
			clone.StoredItemType = this.StoredItemType;
			clone.StoredItemStackSize = this.StoredItemStackSize;
			clone.Timestamp = this.Timestamp;
			clone._CachedItem = this._CachedItem;
			return clone;
		}

		public override ModItem Clone( Item item ) {
			var clone = (TupperwareItem)base.Clone( item );
			clone.StoredItemType = this.StoredItemType;
			clone.StoredItemStackSize = this.StoredItemStackSize;
			clone.Timestamp = this.Timestamp;
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
			this.Timestamp = SystemHelpers.TimeStampInSeconds() - tag.GetInt( "duration" );
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "stack", this.StoredItemStackSize },
				{ "type", this.StoredItemType },
				{ "duration", (int)(SystemHelpers.TimeStampInSeconds() - this.Timestamp) }
			};
		}

		public override void NetRecieve( BinaryReader reader ) {
			this.StoredItemStackSize = reader.ReadInt32();
			this.StoredItemType = reader.ReadInt32();
			this.Timestamp = SystemHelpers.TimeStampInSeconds() - reader.ReadInt32();
		}

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( (int)this.StoredItemStackSize );
			writer.Write( (int)this.StoredItemType );
			writer.Write( (int)(SystemHelpers.TimeStampInSeconds() - this.Timestamp) );
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
			float maxFreshnessDuration = (float)myitem.ComputeMaxFreshnessDuration( this._CachedItem );
			float currentDuration = (float)(SystemHelpers.TimeStampInSeconds() - this.Timestamp);

			return 1f - (currentDuration / maxFreshnessDuration);
		}
	}
}
