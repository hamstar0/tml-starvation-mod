using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	class StarvationBuff : GlobalBuff {
		public override void ModifyBuffTip( int type, ref string tip, ref int rare ) {
			if( type == BuffID.WellFed ) {
				var mymod = (StarvationMod)this.mod;

				float addedMaxHp = Main.LocalPlayer.statLifeMax - 100;
				float rate = mymod.Config.WellFedDrainRate + (addedMaxHp * mymod.Config.AddedWellFedDrainRatePerMaxHealthOver100);
				rate *= 100;

				tip += "\nDepletion rate (based on max HP): " + rate.ToString("N2") + "%";
			}
		}
	}
}
