using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void OnConsumeItem( Item item, Player player ) {
			if( this.NeedsSaving( item ) && item.buffType == BuffID.WellFed ) {
				this.ModifyBuffDuration( item, player );
			}
		}


		////////////////

		public bool ComputeElapsedTicks( Item item, out int elapsedTicks ) {
			if( !this.NeedsSaving( item ) ) {
				elapsedTicks = 0;
				return false;
			}

			if( this.TimestampInSeconds == 0 ) {
				elapsedTicks = 0;
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			long now = SystemHelpers.TimeStampInSeconds();
			int elapsedSeconds = (int)(now - this.TimestampInSeconds);

			elapsedTicks = (int)elapsedSeconds * 60;
			return true;
		}

		public bool ComputeMaxElapsedTicks( Item item, out int maxElapsedTicks ) {
			if( !this.NeedsSaving( item ) ) {
				maxElapsedTicks = 0;
				return false;
			}

			var mymod = (StarvationMod)this.mod;

			if( item.buffType == BuffID.WellFed ) {
				maxElapsedTicks = (int)MathHelper.Clamp(
					item.buffTime,
					mymod.Config.FoodSpoilageMinTickDuration,
					mymod.Config.FoodSpoilageMaxTickDuration
				);
				maxElapsedTicks = (int)((float)maxElapsedTicks * mymod.Config.FoodSpoilageDurationScale);
			} else {
				maxElapsedTicks = mymod.Config.FoodIngredientSpoilageTickDuration;
			}

			return true;
		}


		public bool ComputeTimeLeftPercent( Item item, out float timeLeftPercent ) {
			int elapsedTicks, maxElapsedTicks;

			if( !this.ComputeElapsedTicks(item, out elapsedTicks) ) {
				timeLeftPercent = 1f;
				return false;
			}
			if( !this.ComputeMaxElapsedTicks(item, out maxElapsedTicks) ) {
				timeLeftPercent = 1f;
				return false;
			}

			float timeLeftTicks = maxElapsedTicks - elapsedTicks;
			timeLeftPercent = MathHelper.Clamp( timeLeftTicks / maxElapsedTicks, 0f, 1f );

			return true;
		}


		////////////////

		public bool SetTimeLeftByPercent( Item item, float timeLeftPercent ) {
			int maxElapsedTicks;
			if( !this.ComputeMaxElapsedTicks(item, out maxElapsedTicks) ) {
				throw new ModHelpersException( "Could not compute max elapsed ticks." );
			}

			float elapsedPercent = 1f - timeLeftPercent;
			int elapsedTicks = (int)((float)maxElapsedTicks * elapsedPercent);
			int elapsedSeconds = elapsedTicks / 60;
			long now = SystemHelpers.TimeStampInSeconds();

			this.TimestampInSeconds = now - elapsedSeconds;

			return true;
		}
		

		////////////////

		private void ModifyBuffDuration( Item item, Player player ) {
			var mymod = (StarvationMod)this.mod;
			int buffIdx = player.FindBuffIndex( BuffID.WellFed );

			if( buffIdx >= 0 && mymod.Config.FoodSpoilageDurationScale > 0f ) {
				int elapsedTicks, maxElapsedTicks = -1;
				if( !this.ComputeElapsedTicks( item, out elapsedTicks ) ) {
					return;
				}
				if( !this.ComputeMaxElapsedTicks( item, out maxElapsedTicks ) ) {
					return;
				}

				int newBuffTime = maxElapsedTicks - elapsedTicks;

				if( player.buffTime[buffIdx] < newBuffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				} else if( player.buffTime[buffIdx] == item.buffTime ) {
					player.buffTime[buffIdx] = newBuffTime;
				}
			}
		}
	}
}
