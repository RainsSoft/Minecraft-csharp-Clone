namespace net.minecraft.src
{

	public class EntityGiantZombie : EntityMob
	{
		public EntityGiantZombie(World par1World) : base(par1World)
		{
			Texture = "/mob/zombie.png";
			MoveSpeed = 0.5F;
			AttackStrength = 50;
			YOffset *= 6F;
			SetSize(Width * 6F, Height * 6F);
		}

		public override int GetMaxHealth()
		{
			return 100;
		}

		/// <summary>
		/// Takes a coordinate in and returns a weight to determine how likely this creature will try to path to the block.
		/// Args: x, y, z
		/// </summary>
		public override float GetBlockPathWeight(int par1, int par2, int par3)
		{
			return WorldObj.GetLightBrightness(par1, par2, par3) - 0.5F;
		}
	}

}