using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Starvation.Items;
using Starvation.NetProtocols;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	class StarvationPlayer : ModPlayer {
		private bool IsStarving = false;
		private int HurtDelay = 0;

		private Item PrevSelectedItem = null;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnConnectServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			} else if( Main.netMode == 1 ) {
				this.OnConnectClient();
			}
		}


		////////////////

		private void OnConnectSingle() {
		}

		private void OnConnectClient() {
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
		}

		private void OnConnectServer() {
		}



		////////////////

		public override void PreUpdate() {
			Player plr = this.player;
			if( plr.whoAmI != Main.myPlayer ) { return; }
			if( plr.dead ) { return; }

			var mymod = (StarvationMod)this.mod;
			int buffIdx = plr.FindBuffIndex( BuffID.WellFed );
			bool isStarving = false;

			if( buffIdx == -1 ) {
				if( this.HurtDelay-- < 0 ) {
					this.HurtDelay = mymod.Config.StarvationHarmDelay;
					this.HungerHurt();
				}
				isStarving = true;
			} else {
				if( plr.buffTime[buffIdx] > ( mymod.Config.WellFedDrainRate + 1 ) ) {
					float mul = mymod.Config.AddedWellFedDrainRateMultiplierPerMaxHealthOver100;
					float addDrain = mul * (float)Math.Max( 0, this.player.statLifeMax - 100 );
					plr.buffTime[buffIdx] -= mymod.Config.WellFedDrainRate + (int)addDrain;
				}
			}

			if( isStarving && isStarving != this.IsStarving ) {
				Main.NewText( "You're starving! Find food quickly.", Color.Red );
				if( this.player.statLifeMax == 400 && mymod.Config.CraftableUnlifeCrystal ) {
					Main.NewText( "Tip: Craft Unlife Crystals to reduce max hunger rate (but also max health).", new Color( 96, 96, 96 ) );
				}
			}
			this.IsStarving = isStarving;
		}


		public override void PostUpdate() {
			Player plr = this.player;
			if( plr.whoAmI != Main.myPlayer ) { return; }

			if( Main.mouseItem != null && !Main.mouseItem.IsAir ) {
				if( plr.HeldItem.type == this.mod.ItemType<TupperwareItem>() ) {
					if( this.PrevSelectedItem != null ) {
						this.AttemptTupperwareAddCurrentItem( plr, this.PrevSelectedItem );
						this.PrevSelectedItem = null;
					}
				} else {
					this.PrevSelectedItem = plr.HeldItem;
				}
			} else {
				this.PrevSelectedItem = null;
			}
		}


		////////////////

		public override void OnRespawn( Player player ) {
			var mymod = (StarvationMod)this.mod;
			player.AddBuff( BuffID.WellFed, mymod.Config.RespawnWellFedDuration );
		}

		////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			var mymod = (StarvationMod)this.mod;

			if( !mediumcoreDeath && mymod.Config.PlayerStarterSoup > 0 ) {
				Item soup = new Item();
				soup.SetDefaults( ItemID.BowlofSoup, true );
				soup.stack = mymod.Config.PlayerStarterSoup;

				items.Add( soup );
			}
		}


		////////////////

		public void HungerHurt() {
			var mymod = (StarvationMod)this.mod;
			Player plr = this.player;

			float mul = mymod.Config.AddedStarvationHarmMultiplierPerMaxHealthOver100;
			float addedHarm = mul * (float)Math.Max( 0, plr.statLifeMax - 100 );
			int harm = mymod.Config.StarvationHarm + (int)addedHarm;

			CombatText.NewText( plr.getRect(), CombatText.LifeRegenNegative, harm, false, true );

			plr.statLife -= harm;
			if( plr.statLife <= 0 ) {
				plr.KillMe( PlayerDeathReason.ByCustomReason( plr.name + " starved to death." ), 10f, 0, false );
			}
		}


		////////////////

		private bool AttemptTupperwareAddCurrentItem( Player player, Item item ) {
			var tupperItem = (TupperwareItem)player.HeldItem.modItem;
			if( tupperItem == null ) {
				return false;
			}

			bool isAdded = false;
			
			if( tupperItem.CanAddItem( item ) ) {
				for( int i = 0; i < player.inventory.Length; i++ ) {
					Item invItem = player.inventory[i];
					if( invItem == null || invItem.IsAir ) {
						continue;
					}
					if( !invItem.IsNotTheSameAs( item ) ) {
						tupperItem.AddItem( item );
						player.inventory[ i ] = new Item();
						isAdded = true;
						break;
					}
				}
			}

			return isAdded;
		}
	}
}
