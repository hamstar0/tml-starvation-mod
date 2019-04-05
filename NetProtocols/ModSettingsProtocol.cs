using HamstarHelpers.Components.Network;


namespace Starvation.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public StarvationConfigData ModSettings;



		////////////////

		private ModSettingsProtocol() { }


		////////////////

		protected override void InitializeServerSendData( int who ) {
			this.ModSettings = StarvationMod.Instance.Config;
		}

		////////////////
		
		protected override void ReceiveReply() {
			StarvationMod.Instance.ConfigJson.SetData( this.ModSettings );
		}
	}
}
