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
		public static bool CanSpoil( Item item ) {
			var mymod = StarvationMod.Instance;

			if( !mymod.Config.FoodSpoilageEnabled ) {
				return false;
			}

			if( item.buffType == BuffID.WellFed && item.buffTime > 0 ) {
				return true;
			}

			if( mymod.Config.FoodIngredientsAlsoSpoil ) {
				var itemGrps = EntityGroups.ItemGroups;

				if( itemGrps.ContainsKey("Any Food Ingredient") && itemGrps["Any Food Ingredient"].Contains( item.type ) ) {
					switch( item.type ) {
					case ItemID.Pumpkin:
					case ItemID.BlinkrootSeeds:
					case ItemID.Hay:
					case ItemID.Mushroom:
					case ItemID.Bowl:
						break;
					default:
						return true;
					}
				}
			}
			
			return false;
		}



		////////////////

		public long TimestampInSeconds;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			var clone = (StarvationItem)base.Clone( item, itemClone );

			if( this.NeedsSaving( item ) ) {
				clone.TimestampInSeconds = this.TimestampInSeconds;
			}

			return clone;
		}


		public override bool NeedsSaving( Item item ) {
			return StarvationItem.CanSpoil( item );
		}

		public override void Load( Item item, TagCompound tags ) {
			if( this.NeedsSaving( item ) ) {
				this.TimestampInSeconds = SystemHelpers.TimeStampInSeconds();

				if( tags.ContainsKey( "duration" ) ) {
					this.TimestampInSeconds -= tags.GetInt( "duration" );
				}
			}
		}


		public override TagCompound Save( Item item ) {
			if( this.NeedsSaving( item ) ) {
				return new TagCompound {
					{ "duration", (int)(SystemHelpers.TimeStampInSeconds() - this.TimestampInSeconds) }
				};
			}
			return new TagCompound();
		}

		////

		public override void NetReceive( Item item, BinaryReader reader ) {
			if( this.NeedsSaving( item ) ) {
				this.TimestampInSeconds = SystemHelpers.TimeStampInSeconds() - reader.ReadInt32();
			}
		}

		public override void NetSend( Item item, BinaryWriter writer ) {
			if( this.NeedsSaving( item ) ) {
				writer.Write( (Int32)SystemHelpers.TimeStampInSeconds() - this.TimestampInSeconds );
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
			this.ResetTimestampAndMaxStackSize( item );
		}

		public override void UpdateInventory( Item item, Player player ) {
			this.ResetTimestampAndMaxStackSize( item );
		}


		////////////////

		public void ResetTimestampAndMaxStackSize( Item item ) {
			if( this.NeedsSaving( item ) ) {
				item.maxStack = 1;
				if( this.TimestampInSeconds == 0 ) {
					this.TimestampInSeconds = SystemHelpers.TimeStampInSeconds();
				}
			}
		}
	}
}
