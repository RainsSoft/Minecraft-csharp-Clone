using System;

namespace net.minecraft.src
{

	public abstract class EntityAgeable : EntityCreature
	{
		public EntityAgeable(World par1World) : base(par1World)
		{
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(12, new int?(0));
		}

		/// <summary>
		/// The age value may be negative or positive or zero. If it's negative, it get's incremented on each tick, if it's
		/// positive, it get's decremented each tick. Don't confuse this with EntityLiving.getAge. With a negative value the
		/// Entity is considered a child.
		/// </summary>
		public virtual int GetGrowingAge()
		{
			return DataWatcher.GetWatchableObjectInt(12);
		}

		/// <summary>
		/// The age value may be negative or positive or zero. If it's negative, it get's incremented on each tick, if it's
		/// positive, it get's decremented each tick. With a negative value the Entity is considered a child.
		/// </summary>
		public virtual void SetGrowingAge(int par1)
		{
			DataWatcher.UpdateObject(12, Convert.ToInt32(par1));
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetInteger("Age", GetGrowingAge());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetGrowingAge(par1NBTTagCompound.GetInteger("Age"));
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();
			int i = GetGrowingAge();

			if (i < 0)
			{
				i++;
				SetGrowingAge(i);
			}
			else if (i > 0)
			{
				i--;
				SetGrowingAge(i);
			}
		}

		/// <summary>
		/// If Animal, checks if the age timer is negative
		/// </summary>
		public override bool IsChild()
		{
			return GetGrowingAge() < 0;
		}
	}

}