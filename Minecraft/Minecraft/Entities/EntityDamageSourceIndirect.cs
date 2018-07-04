namespace net.minecraft.src
{

	public class EntityDamageSourceIndirect : EntityDamageSource
	{
		private Entity IndirectEntity;

		public EntityDamageSourceIndirect(string par1Str, Entity par2Entity, Entity par3Entity) : base(par1Str, par2Entity)
		{
			IndirectEntity = par3Entity;
		}

		public override Entity GetSourceOfDamage()
		{
			return DamageSourceEntity;
		}

		public override Entity GetEntity()
		{
			return IndirectEntity;
		}
	}

}