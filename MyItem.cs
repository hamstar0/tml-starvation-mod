using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.EntityGroups;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public long Timestamp;

		private int Ticks = 0;
		private DateTime PrevDate = DateTime.UtcNow;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			var clone = (StarvationItem)base.Clone( item, itemClone );

			if( this.NeedsSaving( item ) ) {
				clone.Timestamp = this.Timestamp;
			}

			return clone;
		}


		public override bool NeedsSaving( Item item ) {
			var mymod = (StarvationMod)this.mod;

			if( mymod.Config.FoodSpoilageEnabled ) {
				if( item.buffType == BuffID.WellFed ) {
					return true;
				}

				if( mymod.Config.FoodIngredientsAlsoSpoil ) {
					var itemGrps = EntityGroups.ItemGroups;

					if( itemGrps.ContainsKey("Any Food Ingredient") && itemGrps["Any Food Ingredient"].Contains( item.type ) ) {
						switch( item.type ) {
						case ItemID.Pumpkin:
						case ItemID.BlinkrootSeeds:
						case ItemID.Hay:
							break;
						default:
							return true;
						}
					}
				}
			}

			return false;
		}

		public override void Load( Item item, TagCompound tags ) {
			if( this.NeedsSaving( item ) ) {
				this.Timestamp = SystemHelpers.TimeStampInSeconds();

				if( tags.ContainsKey( "duration" ) ) {
					this.Timestamp -= tags.GetInt( "duration" );
				}
			}
		}


		public override TagCompound Save( Item item ) {
			if( this.NeedsSaving( item ) ) {
				return new TagCompound {
					{ "duration", (int)(SystemHelpers.TimeStampInSeconds() - this.Timestamp) }
				};
			}
			return new TagCompound();
		}

		////

		public override void NetReceive( Item item, BinaryReader reader ) {
			if( this.NeedsSaving( item ) ) {
				this.Timestamp = SystemHelpers.TimeStampInSeconds() - reader.ReadInt32();
			}
		}

		public override void NetSend( Item item, BinaryWriter writer ) {
			if( this.NeedsSaving( item ) ) {
				writer.Write( (Int32)SystemHelpers.TimeStampInSeconds() - this.Timestamp );
			}
		}


		////////////////

		public override void SetDefaults( Item item ) {
			if( this.NeedsSaving( item ) ) {
				item.maxStack = 1;

				if( item.buffType == BuffID.WellFed ) {
					this.ApplyWellFedModifiers( item );
				}
			}
		}


		////////////////

		public override void Update( Item item, ref float gravity, ref float maxFallSpeed ) {
			if( this.NeedsSaving( item ) ) {
				item.maxStack = 1;
			}
		}

		public override void UpdateInventory( Item item, Player player ) {
			if( this.NeedsSaving( item ) ) {
				item.maxStack = 1;
			}
		}
	}
}
