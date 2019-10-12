using HamstarHelpers.Helpers.Items;
using Starvation.Items;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Starvation {
	class MyNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
			var mymod = (StarvationMod)this.mod;
			var npcDef = new NPCDefinition( npc.type );

			if( mymod.Config.TupperwareDropsNpcIdsAndChances.ContainsKey(npcDef) ) {
				if( Main.rand.NextFloat() < mymod.Config.TupperwareDropsNpcIdsAndChances[npcDef] ) {
					ItemHelpers.CreateItem( npc.Center, ModContent.ItemType<TupperwareItem>(), 1, TupperwareItem.Width, TupperwareItem.Height );
				}
			}
		}

		////

		public override void SetupShop( int npcType, Chest shop, ref int nextSlot ) {
			var mymod = (StarvationMod)this.mod;

			if( mymod.Config.TupperwareSellsFromMerchantByNpc != null ) {
				if( npcType == mymod.Config.TupperwareSellsFromMerchantByNpc.Type ) {
					var tupperware = new Item();
					tupperware.SetDefaults( ModContent.ItemType<TupperwareItem>(), false );

					shop.item[nextSlot++] = tupperware;
				}
			}
		}
	}
}
