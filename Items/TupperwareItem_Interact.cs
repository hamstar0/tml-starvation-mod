using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public override bool ConsumeItem( Player player ) {
			return false;
		}

		public override bool CanRightClick() {
			/*Item myItem = this.item;
			Timers.SetTimer( "TupperwareRightClickStackRevert_"+this.item.GetHashCode(), 2, () => {
				myItem.stack = 1;
				return false;
			} );

			this.item.stack = 2;*/
			return this.StoredItemType > 0 && this.StoredItemStackSize > 0;
		}

		public override void RightClick( Player player ) {
			var mymod = (StarvationMod)this.mod;

			this.UnstoreItem( player.Center );
		}


		////////////////
		
		public bool CanStoreItem( Item item ) {
			if( (this.StoredItemStackSize > 0 && this.StoredItemType != -1) && this.StoredItemType != item.type ) {
				return false;
			}
			if( item.stack > 1 ) {
				return false;
			}

			var mymod = (StarvationMod)this.mod;
			if( this.StoredItemStackSize >= mymod.Config.TupperwareMaxStackSize ) {
				return false;
			}

			var myitem = item.GetGlobalItem<StarvationItem>();

			float timeLeftPercent;
			if( !myitem.ComputeTimeLeftPercent(item, out timeLeftPercent) || timeLeftPercent <= 0 ) {
				return false;
			}

			if( this.StoredItemStackSize > 0 ) {
				float myTimeLeftPercent;
				if( !this.ComputeTimeLeftPercent( out myTimeLeftPercent ) || myTimeLeftPercent <= 0 ) {
					return false;
				}
			}

			return true;
		}
		

		////

		internal bool StoreItem( Item item ) {
			if( this.StoredItemStackSize > 0 && this.StoredItemType != item.type ) {
				Main.NewText( "Tupperware cannot hold this type of item.", Color.Red );
				return false;
			}

			int maxElapsedSeconds;
			if( this.StoredItemStackSize == 0 ) {
				if( !TupperwareItem.PredictMaxElapsedTicksOfItem(item, out maxElapsedSeconds) ) {
					Main.NewText( "Could not predict max elapsed time of item.", Color.Red );
					return false;
				}
			} else {
				if( !this.ComputeMaxElapsedTicks( out maxElapsedSeconds ) ) {
					Main.NewText( "Could not predict max elapsed time of own item.", Color.Red );
					return false;
				}
			}

			var myitem = item.GetGlobalItem<StarvationItem>();
			float itemTimeLeftPercent;
			if( !myitem.ComputeTimeLeftPercent(item, out itemTimeLeftPercent) ) {
				Main.NewText( "Could not compute time left of item.", Color.Red );
				return false;
			}

			float timeLeftPercent = this.ComputeAveragedTimeLeftByPercent( itemTimeLeftPercent );

			this.SetTimeLeftByPercent( maxElapsedSeconds, timeLeftPercent );
			
			this.StoredItemType = item.type;
			this.StoredItemStackSize++;
			 this.GetCachedModItem();   // Resets the timestamp

			return true;
		}


		public bool UnstoreItem( Vector2 position ) {
			if( this.StoredItemStackSize <= 0 ) {
				return false;
			}

			float timeLeftPercent;
			if( !this.ComputeTimeLeftPercent(out timeLeftPercent) ) {
				Main.NewText( "Could not compute time left of tupperware.", Color.Red );
				return false;
			}

			int newItemIdx = ItemHelpers.CreateItem( position, this.StoredItemType, 1, 16, 16 );
			Item newItem = Main.item[ newItemIdx ];

			var newItemInfo = newItem.GetGlobalItem<StarvationItem>();
			newItemInfo.SetTimeLeftByPercent( newItem, timeLeftPercent );

			this.StoredItemStackSize--;
			if( this.StoredItemStackSize == 0 ) {
				this._CachedItem = null;
			}

			return true;
		}
	}
}
