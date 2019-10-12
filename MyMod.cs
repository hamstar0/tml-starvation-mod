using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Services.EntityGroups;
using System;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationMod : Mod {
		public static StarvationMod Instance { get; private set; }



		////////////////
		
		public StarvationConfig Config => ModContent.GetInstance<StarvationConfig>();



		////////////////

		public StarvationMod() {
			StarvationMod.Instance = this;
		}

		////////////////

		public override void Load() {
			EntityGroups.Enable();
		}

		public override void Unload() {
			StarvationMod.Instance = null;
		}

		////

		public override void AddRecipes() {
			StarvationItem.AddNewRecipes();
		}

		public override void PostAddRecipes() {
			StarvationItem.ApplyRecipeModifications();
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof(StarvationAPI), args );
		}
	}
}
