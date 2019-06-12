using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		private StarvationItem GetCachedModItem() {
			if( this._CachedItem == null || this._CachedItem.type != this.StoredItemType ) {
				this._CachedItem = new Item();
				this._CachedItem.SetDefaults( this.StoredItemType, true );
			}

			return this._CachedItem.GetGlobalItem<StarvationItem>();
		}


		////////////////

		public float ComputeMaxFreshnessDurationTicks() {
			if( this.StoredItemStackSize == 0 ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;
			StarvationItem myitem = this.GetCachedModItem();

			return myitem.ComputeMaxFreshnessDurationTicks(this._CachedItem) / mymod.Config.TupperwareSpoilageRateScale;
		}


		public float ComputeContainedItemsFreshnessPercent() {
			if( this.StoredItemStackSize == 0 ) {
				return -1;
			}

			float maxFreshnessTickDuration = (float)this.ComputeMaxFreshnessDurationTicks();
			if( maxFreshnessTickDuration == -1 ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;

			float currentTickDuration = (float)( SystemHelpers.TimeStampInSeconds() - this.TimestampInSeconds ) * 60f;

			return Math.Max( 1f - (currentTickDuration / maxFreshnessTickDuration), 0f );
		}
	}
}
