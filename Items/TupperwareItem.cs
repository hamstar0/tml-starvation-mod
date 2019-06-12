using HamstarHelpers.Helpers.DebugHelpers;
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
		private long TimestampInSeconds;

		private Item _CachedItem = null;


		////////////////

		public int MyLastInventoryPosition { get; private set; }

		////

		public override bool CloneNewInstances => true;



		////////////////

		public override ModItem Clone() {
			var clone = (TupperwareItem)base.Clone();
			clone.StoredItemType = this.StoredItemType;
			clone.StoredItemStackSize = this.StoredItemStackSize;
			clone.TimestampInSeconds = this.TimestampInSeconds;
			clone._CachedItem = this._CachedItem;
			return clone;
		}

		public override ModItem Clone( Item item ) {
			var clone = (TupperwareItem)base.Clone( item );
			clone.StoredItemType = this.StoredItemType;
			clone.StoredItemStackSize = this.StoredItemStackSize;
			clone.TimestampInSeconds = this.TimestampInSeconds;
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
			if( !tag.ContainsKey( "stack" ) ) {
				return;
			}

			this.StoredItemStackSize = tag.GetInt( "stack" );
			this.StoredItemType = tag.GetInt( "type" );
			this.TimestampInSeconds = SystemHelpers.TimeStampInSeconds() - tag.GetInt( "duration" );
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "stack", this.StoredItemStackSize },
				{ "type", this.StoredItemType },
				{ "duration", (int)(SystemHelpers.TimeStampInSeconds() - this.TimestampInSeconds) }
			};
		}

		public override void NetRecieve( BinaryReader reader ) {
			this.StoredItemStackSize = reader.ReadInt32();
			this.StoredItemType = reader.ReadInt32();
			this.TimestampInSeconds = SystemHelpers.TimeStampInSeconds() - reader.ReadInt32();
		}

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( (int)this.StoredItemStackSize );
			writer.Write( (int)this.StoredItemType );
			writer.Write( (int)( SystemHelpers.TimeStampInSeconds() - this.TimestampInSeconds ) );
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			this.MyLastInventoryPosition = -1;

			for( int i = 0; i < player.inventory.Length; i++ ) {
				if( player.inventory[i] == this.item ) {
					this.MyLastInventoryPosition = i;
					break;
				}
			}
		}
	}
}
