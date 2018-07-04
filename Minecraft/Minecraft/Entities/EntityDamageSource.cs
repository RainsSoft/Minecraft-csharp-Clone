namespace net.minecraft.src
{

	public class EntityDamageSource : DamageSource
	{
		protected Entity DamageSourceEntity;

		public EntityDamageSource(string par1Str, Entity par2Entity) : base(par1Str)
		{
			DamageSourceEntity = par2Entity;
		}

		public override Entity GetEntity()
		{
			return DamageSourceEntity;
		}
	}

}