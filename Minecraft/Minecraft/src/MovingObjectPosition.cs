namespace net.minecraft.src
{

	public class MovingObjectPosition
	{
		/// <summary>
		/// What type of ray trace hit was this? 0 = block, 1 = entity </summary>
		public EnumMovingObjectType TypeOfHit;

		/// <summary>
		/// x coordinate of the block ray traced against </summary>
		public int BlockX;

		/// <summary>
		/// y coordinate of the block ray traced against </summary>
		public int BlockY;

		/// <summary>
		/// z coordinate of the block ray traced against </summary>
		public int BlockZ;

		/// <summary>
		/// Which side was hit. If its -1 then it went the full length of the ray trace. Bottom = 0, Top = 1, East = 2, West
		/// = 3, North = 4, South = 5.
		/// </summary>
		public int SideHit;

		/// <summary>
		/// The vector position of the hit </summary>
		public Vec3D HitVec;

		/// <summary>
		/// The hit entity </summary>
		public Entity EntityHit;

		public MovingObjectPosition(int par1, int par2, int par3, int par4, Vec3D par5Vec3D)
		{
			TypeOfHit = EnumMovingObjectType.TILE;
			BlockX = par1;
			BlockY = par2;
			BlockZ = par3;
			SideHit = par4;
			HitVec = Vec3D.CreateVector(par5Vec3D.XCoord, par5Vec3D.YCoord, par5Vec3D.ZCoord);
		}

		public MovingObjectPosition(Entity par1Entity)
		{
			TypeOfHit = EnumMovingObjectType.ENTITY;
			EntityHit = par1Entity;
			HitVec = Vec3D.CreateVector(par1Entity.PosX, par1Entity.PosY, par1Entity.PosZ);
		}
	}

}