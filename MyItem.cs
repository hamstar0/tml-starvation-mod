using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public int AverageDuration = 0;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			var clone = (StarvationItem)base.Clone( item, itemClone );

			if( this.NeedsSaving(item) ) {
				clone.AverageDuration = this.AverageDuration;
			}

			return clone;
		}


		public override bool NeedsSaving( Item item ) {
			var mymod = (StarvationMod)this.mod;

			return mymod.Config.FoodSpoilageEnabled && item.buffType == BuffID.WellFed;
		}

		public override void Load( Item item, TagCompound tags ) {
			if( this.NeedsSaving( item ) ) {
				if( tags.ContainsKey( "duration" ) ) {
					this.AverageDuration = tags.GetInt( "duration" );
				}
			}
		}


		public override TagCompound Save( Item item ) {
			if( this.NeedsSaving( item ) ) {
				return new TagCompound {
					{ "duration", (int)this.AverageDuration }
				};
			}
			return new TagCompound();
		}

		////

		public override void NetReceive( Item item, BinaryReader reader ) {
			if( this.NeedsSaving( item ) ) {
				this.AverageDuration = reader.ReadInt32();
			}
		}

		public override void NetSend( Item item, BinaryWriter writer ) {
			if( this.NeedsSaving( item ) ) {
				writer.Write( (Int32)this.AverageDuration );
			}
		}


		////////////////

		public override void SetDefaults( Item item ) {
			if( this.NeedsSaving( item ) ) {
				item.maxStack = 1;
			}
		}


		////////////////

		public override void Update( Item item, ref float gravity, ref float maxFallSpeed ) {
			if( this.NeedsSaving( item ) ) {
				this.AverageDuration++;

				item.maxStack = 1;
			}
		}

		public override void UpdateInventory( Item item, Player player ) {
			if( this.NeedsSaving( item ) ) {
				this.AverageDuration++;

				item.maxStack = 1;
			}
		}
	}
}
