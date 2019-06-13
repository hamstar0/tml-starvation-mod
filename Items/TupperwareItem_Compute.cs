using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


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

		public float ComputeMaxElapsedTicks() {
			if( this.StoredItemStackSize == 0 ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;
			StarvationItem myitem = this.GetCachedModItem();

			float maxTicks = myitem.ComputeMaxElapsedTicks( this._CachedItem );
			float maxTicksScaled = maxTicks / mymod.Config.TupperwareSpoilageRateScale;

			return maxTicksScaled;
		}


		public float ComputeElapsedTicks() {
			if( this.StoredItemStackSize == 0 ) {
				return -1;
			}

			var mymod = (StarvationMod)this.mod;

			long now = SystemHelpers.TimeStampInSeconds();
			float elapsedSeconds = (float)(now - this.TimestampInSeconds);
			float elapsedTicks = elapsedSeconds * 60;

			return elapsedTicks;
		}


		public bool ComputeTimeLeftPercent( out float timeLeftPercent ) {
			float elapsedTicks = this.ComputeElapsedTicks();
			float maxElapsedTicks = this.ComputeMaxElapsedTicks();
			if( elapsedTicks == -1 || maxElapsedTicks == -1 ) {
				timeLeftPercent = maxElapsedTicks;
				return false;
			}

			float elapsedPercent = elapsedTicks / maxElapsedTicks;
			timeLeftPercent = Math.Max( 1f - elapsedPercent, 0f );

			return true;
		}
	}
}
