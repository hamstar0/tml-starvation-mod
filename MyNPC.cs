using HamstarHelpers.Helpers.ItemHelpers;
using Starvation.Items;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation {
	class MyNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
			var mymod = (StarvationMod)this.mod;

			if( mymod.Config.TupperwareDropsNpcIdsAndChances.ContainsKey( npc.type ) ) {
				if( Main.rand.NextFloat() < mymod.Config.TupperwareDropsNpcIdsAndChances[npc.type] ) {
					ItemHelpers.CreateItem( npc.Center, mymod.ItemType<TupperwareItem>(), 1, TupperwareItem.Width, TupperwareItem.Height );
				}
			}
		}

		////

		public override void SetupShop( int npcType, Chest shop, ref int nextSlot ) {
			var mymod = (StarvationMod)this.mod;

			if( mymod.Config.TupperwareSellsFromMerchantByNpcId != 0 && npcType == mymod.Config.TupperwareSellsFromMerchantByNpcId ) {
				var tupperware = new Item();
				tupperware.SetDefaults( mymod.ItemType<TupperwareItem>(), false );

				shop.item[ nextSlot++ ] = tupperware;
			}
		}
	}
}
