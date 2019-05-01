using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.EntityGroups;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public int DurationOfExistence = 0;

		private int Ticks = 0;
		private DateTime PrevDate = DateTime.UtcNow;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			var clone = (StarvationItem)base.Clone( item, itemClone );

			if( this.NeedsSaving( item ) ) {
				clone.DurationOfExistence = this.DurationOfExistence;
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
					if( EntityGroups.ItemGroups["Any Food Ingredient"].Contains( item.type ) ) {
						return true;
					}
				}
			}
			return false;
		}

		public override void Load( Item item, TagCompound tags ) {
			if( this.NeedsSaving( item ) ) {
				if( tags.ContainsKey( "duration" ) ) {
					this.DurationOfExistence = tags.GetInt( "duration" );
				}
			}
		}


		public override TagCompound Save( Item item ) {
			if( this.NeedsSaving( item ) ) {
				return new TagCompound {
					{ "duration", (int)this.DurationOfExistence }
				};
			}
			return new TagCompound();
		}

		////

		public override void NetReceive( Item item, BinaryReader reader ) {
			if( this.NeedsSaving( item ) ) {
				this.DurationOfExistence = reader.ReadInt32();
			}
		}

		public override void NetSend( Item item, BinaryWriter writer ) {
			if( this.NeedsSaving( item ) ) {
				writer.Write( (Int32)this.DurationOfExistence );
			}
		}


		////////////////

		public override void SetDefaults( Item item ) {
			if( this.NeedsSaving( item ) ) {
				item.maxStack = 1;

				this.ApplyWellFedModifiers( item );
			}
		}


		////////////////

		public override void Update( Item item, ref float gravity, ref float maxFallSpeed ) {
			if( this.NeedsSaving( item ) ) {
				this.UpdateDuration();
				item.maxStack = 1;
			}
		}

		public override void UpdateInventory( Item item, Player player ) {
			if( this.NeedsSaving( item ) ) {
				this.UpdateDuration();
				item.maxStack = 1;
			}
		}
		
		////

		private void UpdateDuration() {
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
	}
}
