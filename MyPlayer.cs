using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Messages.Inbox;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Starvation.Items;
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

			var mymod = (StarvationMod)this.mod;

			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			} else if( Main.netMode == 1 ) {
				this.OnConnectClient();
			}

			if( mymod.Config.CraftableUnlifeCrystal && mymod.Config.AddedWellFedDrainRatePerTickMultiplierPerMaxHealthOver100 > 0 ) {
				if( mymod.Config.UnlifeCrystalReturnsLifeCrystal ) {
					InboxMessages.SetMessage( "StarvationUnlifeTip", "Craft and use Unlife Crystals to lower max HP to reduce hunger drain while traveling (produces Life Crystals on use).", false );
				} else {
					InboxMessages.SetMessage( "StarvationUnlifeTip", "Craft and use Unlife Crystals to lower max HP to reduce hunger drain while traveling.", false );
				}
			}
		}


		////////////////

		private void OnConnectSingle() {
		}

		private void OnConnectClient() {
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
					this.HurtDelay = mymod.Config.StarvationHarmRepeatDelayInTicks;
					this.HungerHurt();
				}
				isStarving = true;
			} else {
				if( plr.buffTime[buffIdx] > ( mymod.Config.WellFedAddedDrainPerTick + 1 ) ) {
					float mul = mymod.Config.AddedWellFedDrainRatePerTickMultiplierPerMaxHealthOver100;
					float addDrain = mul * (float)Math.Max( 0, this.player.statLifeMax - 100 );
					plr.buffTime[buffIdx] -= mymod.Config.WellFedAddedDrainPerTick + (int)addDrain;
				}
			}

			if( isStarving && isStarving != this.IsStarving ) {
				Main.NewText( "You're starving! Find food quickly.", Color.Red );
				if( this.player.statLifeMax == 400 && mymod.Config.CraftableUnlifeCrystal ) {
					Main.NewText( "Tip: Craft Unlife Crystals to reduce max hunger rate (but also max health).", new Color( 96, 96, 96 ) );
				}
			}

			this.IsStarving = isStarving;

			if( Timers.GetTimerTickDuration("StarvationInventoryRotCheck") <= 0 ) {
				Timers.SetTimer( "StarvationInventoryRotCheck", 60, () => {
					if( Main.gamePaused || !LoadHelpers.IsWorldSafelyBeingPlayed() ) {
						return false;
					}

					for( int i=0; i<player.inventory.Length; i++ ) {
						Item item = player.inventory[i];
						if( item == null || item.IsAir ) { continue; }

						if( RotItem.IsRotted(item) ) {
							if( !Main.mouseItem.IsAir && i == PlayerItemHelpers.VanillaInventorySelectedSlot ) {
								Main.mouseItem = new Item();
							}

							player.inventory[i] = new Item();
							ItemHelpers.CreateItem( player.Center, ModContent.ItemType<RotItem>(), item.stack, RotItem.Width, RotItem.Height );
						}
					}

					bool? _;
					Item[] myChest = PlayerItemHelpers.GetCurrentlyOpenChest( player, out _ );
					if( myChest != null ) {
						for( int i = 0; i < myChest.Length; i++ ) {
							Item item = myChest[i];
							if( item == null || item.IsAir ) { continue; }

							if( RotItem.IsRotted( item ) ) {
								myChest[i] = new Item();
								ItemHelpers.CreateItem( player.Center, ModContent.ItemType<RotItem>(), item.stack, RotItem.Width, RotItem.Height );
							}
						}
					}

					return false;
				} );
			}
		}


		public override void PostUpdate() {
			Player plr = this.player;
			if( plr.whoAmI != Main.myPlayer ) { return; }
			
			if( Main.mouseItem != null && !Main.mouseItem.IsAir ) {
				if( plr.HeldItem.type == ModContent.ItemType<TupperwareItem>() ) {
					if( this.PrevSelectedItem != null ) {
						this.AttemptTupperwareAddCurrentItem( plr );
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
			player.AddBuff( BuffID.WellFed, mymod.Config.RespawnWellFedTickDuration );
		}

		////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			var mymod = (StarvationMod)this.mod;

			if( !mediumcoreDeath ) {
				this.player.AddBuff( BuffID.WellFed, mymod.Config.RespawnWellFedTickDuration );

				if( mymod.Config.PlayerStarterSoup > 0 ) {
					Item soup = new Item();
					soup.SetDefaults( ItemID.BowlofSoup, true );
					soup.stack = mymod.Config.PlayerStarterSoup;

					items.Add( soup );
				}
			}
		}


		////////////////

		public void HungerHurt() {
			var mymod = (StarvationMod)this.mod;
			Player plr = this.player;

			float mul = mymod.Config.AddedStarvationHarmPerTickMultiplierPerMaxHealthOver100;
			float addedHarm = mul * (float)Math.Max( 0, plr.statLifeMax - 100 );
			int harm = mymod.Config.StarvationHarm + (int)addedHarm;

			CombatText.NewText( plr.getRect(), CombatText.LifeRegenNegative, harm, false, true );

			plr.statLife -= harm;
			if( plr.statLife <= 0 ) {
				plr.KillMe( PlayerDeathReason.ByCustomReason( plr.name + " starved to death." ), 10f, 0, false );
			}
		}


		////////////////

		private bool AttemptTupperwareAddCurrentItem( Player player ) {
			var tupperItem = Main.mouseItem.modItem as TupperwareItem;
			if( tupperItem == null ) {
				return false;
			}

			if( tupperItem.MyLastInventoryPosition == -1 ) {
				return false;
			}
			Item item = player.inventory[ tupperItem.MyLastInventoryPosition ];
			if( item == null ) {
				return false;
			}

			bool isAdded = false;
			
			if( tupperItem.CanStoreItem( item ) ) {
				tupperItem.StoreItem( item );

				player.inventory[ tupperItem.MyLastInventoryPosition ] = Main.mouseItem;
				Main.mouseItem = new Item();

				isAdded = true;
			}

			return isAdded;
		}
	}
}
