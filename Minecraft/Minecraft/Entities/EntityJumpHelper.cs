namespace net.minecraft.src
{

	public class EntityJumpHelper
	{
		private EntityLiving Entity;
		private bool IsJumping;

		public EntityJumpHelper(EntityLiving par1EntityLiving)
		{
			IsJumping = false;
			Entity = par1EntityLiving;
		}

		public virtual void SetJumping()
		{
			IsJumping = true;
		}

		/// <summary>
		/// Called to actually make the entity jump if isJumping is true.
		/// </summary>
		public virtual void DoJump()
		{
			Entity.SetJumping(IsJumping);
			IsJumping = false;
		}
	}

}