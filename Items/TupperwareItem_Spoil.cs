using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public static bool PredictMaxElapsedTicksOfItem( Item item, out int maxElapsedTicks ) {
			var mymod = StarvationMod.Instance;
			StarvationItem myitem = item.GetGlobalItem<StarvationItem>();

			bool isValid = myitem.ComputeMaxElapsedTicks( item, out maxElapsedTicks );
			float maxElapsedTicksScaled = (float)maxElapsedTicks * mymod.Config.TupperwareSpoilageDurationScale;

			maxElapsedTicks = (int)maxElapsedTicksScaled;
			return isValid;
		}



		////////////////

		private StarvationItem GetCachedModItem() {
			if( this._CachedItem == null || this._CachedItem.type != this.StoredItemType ) {
				this._CachedItem = new Item();
				this._CachedItem.SetDefaults( this.StoredItemType, true );
			}

			var myitem = this._CachedItem.GetGlobalItem<StarvationItem>();
			float _ = 0f;
			myitem.ResetTimestampAndMaxStackSize( this._CachedItem );

			return myitem;
		}


		////////////////

		public bool ComputeMaxElapsedTicks( out int maxElapsedTicks ) {
			if( this.StoredItemStackSize == 0 ) {
				maxElapsedTicks = 0;
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			StarvationItem myitem = this.GetCachedModItem();

			if( !myitem.ComputeMaxElapsedTicks( this._CachedItem, out maxElapsedTicks ) ) {
				return false;
			}
			float maxElapsedTicksScaled = (float)maxElapsedTicks * mymod.Config.TupperwareSpoilageDurationScale;

			maxElapsedTicks = (int)maxElapsedTicksScaled;
			return true;
		}


		public bool ComputeElapsedTicks( out int elapsedTicks ) {
			if( this.StoredItemStackSize == 0 ) {
				elapsedTicks = 0;
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			long now = SystemHelpers.TimeStampInSeconds();
			int elapsedSeconds = (int)(now - this.TimestampInSeconds);

			elapsedTicks = elapsedSeconds * 60;
			//float elapsedTicksScaled = (float)elapsedTicks * mymod.Config.TupperwareSpoilageRateScale;
			return true;
		}


		public bool ComputeTimeLeftPercent( out float timeLeftPercent ) {
			int elapsedTicks, maxElapsedTicks;
			if( !this.ComputeElapsedTicks(out elapsedTicks) ) {
				timeLeftPercent = 1f;
				return false;
			}
			if( !this.ComputeMaxElapsedTicks( out maxElapsedTicks ) ) {
				timeLeftPercent = 1f;
				return false;
			}

			float elapsedPercent = (float)elapsedTicks / (float)maxElapsedTicks;

			timeLeftPercent = Math.Max( 1f - elapsedPercent, 0f );
			return true;
		}


		////////////////

		public float ComputeAveragedTimeLeftByPercent( float timeLeftPercent ) {
			float newTimeLeftPercent;

			if( this.StoredItemStackSize > 0 ) {
				float myTimeLeftPercent;
				if( !this.ComputeTimeLeftPercent( out myTimeLeftPercent ) ) {
					throw new ModHelpersException( "Could not compute time left." );
				}

				newTimeLeftPercent = (myTimeLeftPercent * (float)this.StoredItemStackSize) + timeLeftPercent;
				newTimeLeftPercent /= this.StoredItemStackSize + 1;
			} else {
				newTimeLeftPercent = timeLeftPercent;
			}

			return newTimeLeftPercent;
		}


		////////////////

		public void SetTimeLeftByPercent( int maxElapsedSeconds, float timeLeftPercent ) {
			float elapsedPercent = 1f - timeLeftPercent;
			int elapsedTicks = (int)( (float)maxElapsedSeconds * elapsedPercent );
			int elapsedSeconds = elapsedTicks / 60;

			long now = SystemHelpers.TimeStampInSeconds();

			this.TimestampInSeconds = now - elapsedSeconds;
		}
	}
}
