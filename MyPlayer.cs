using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Starvation.NetProtocols;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	class StarvationPlayer : ModPlayer {
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
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>();
		}

		private void OnConnectServer() {
		}


		////////////////

		private bool IsStarving = false;
		private int HurtDelay = 0;


		public override bool CloneNewInstances => false;


		////////////////

		public override void PreUpdate() {
			Player plr = this.player;
			if( plr.whoAmI != Main.myPlayer ) { return; }
			if( plr.dead ) { return; }

			var mymod = (StarvationMod)this.mod;
			int buff_idx = plr.FindBuffIndex( BuffID.WellFed );
			bool is_starving = false;

			if( buff_idx == -1 ) {
				if( this.HurtDelay-- < 0 ) {
					this.HurtDelay = mymod.Config.StarvationHarmRate;
					this.HungerHurt();
				}
				is_starving = true;
			} else {
				if( plr.buffTime[buff_idx] > ( mymod.Config.WellFedDrainRate + 1 ) ) {
					plr.buffTime[buff_idx] -= mymod.Config.WellFedDrainRate;
				}
			}

			if( is_starving && is_starving != this.IsStarving ) {
				Main.NewText( "You're starving! Find food quickly.", Color.Red );
			}
			this.IsStarving = is_starving;
		}

		////

		public override void OnRespawn( Player player ) {
			player.AddBuff( BuffID.WellFed, 60 * 60 * 3 );
		}

		////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			if( !mediumcoreDeath ) {
				Item soup = new Item();
				soup.SetDefaults( ItemID.BowlofSoup, true );
				soup.stack = 3;

				items.Add( soup );
			}
		}


		////////////////

		public void HungerHurt() {
			var mymod = (StarvationMod)this.mod;
			Player plr = this.player;

			CombatText.NewText( plr.getRect(), CombatText.LifeRegenNegative, mymod.Config.StarvationHarm, false, true );

			plr.statLife -= mymod.Config.StarvationHarm;
			if( plr.statLife <= 0 ) {
				plr.KillMe( PlayerDeathReason.ByCustomReason( plr.name + " starved to death." ), 10f, 0, false );
			}
		}
	}
}
