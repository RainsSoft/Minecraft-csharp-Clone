namespace net.minecraft.src
{
	class WorldBlockPositionType
	{
		public int PosX;
        public int PosY;
        public int PosZ;

		/// <summary>
		/// Counts down 80 ticks until the position is accepted from the receive queue into the world.
		/// </summary>
        public int AcceptCountdown;
        public int BlockID;
        public int Metadata;
	}
}