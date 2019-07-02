using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using HamstarHelpers.Services.EntityGroups;
using System;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationMod : Mod {
		public static StarvationMod Instance { get; private set; }



		////////////////
		
		public JsonConfig<StarvationConfigData> ConfigJson { get; private set; }
		public StarvationConfigData Config => this.ConfigJson.Data;



		////////////////

		public StarvationMod() {
			this.ConfigJson = new JsonConfig<StarvationConfigData>(
				StarvationConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new StarvationConfigData()
			);
		}

		////////////////

		public override void Load() {
			StarvationMod.Instance = this;

			EntityGroups.Enable();

			this.LoadConfig();
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.Config.SetDefaults();
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Starvation config " + this.Version.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Starvation updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
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
