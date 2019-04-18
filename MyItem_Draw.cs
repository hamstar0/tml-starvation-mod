using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( !this.NeedsSaving( item ) ) { return; }

			int spoiledAmt = (item.buffTime - this.ComputeBuffTime(item)) / 60;
			string spoilage;

			if( spoiledAmt <= 60 ) {
				spoilage = spoiledAmt + "s";
			} else {
				spoilage = (spoiledAmt / 60) + "m";
			}

			var tip = new TooltipLine( this.mod, "SpoilageTip", "Lost "+ spoilage + " Well Fed duration." );
			tip.overrideColor = Color.Red;

			tooltips.Add( tip );
		}


		public override void PostDrawInInventory( Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale ) {
			// TODO: Draw spoilage indication + is spoiled state
		}
	}
}
