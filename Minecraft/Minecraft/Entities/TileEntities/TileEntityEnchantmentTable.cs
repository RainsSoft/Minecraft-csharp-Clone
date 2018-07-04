using System;

namespace net.minecraft.src
{
	public class TileEntityEnchantmentTable : TileEntity
	{
		/// <summary>
		/// Used by the render to make the book 'bounce' </summary>
		public int TickCount;

		/// <summary>
		/// Value used for determining how the page flip should look. </summary>
		public float PageFlip;

		/// <summary>
		/// The last tick's pageFlip value. </summary>
		public float PageFlipPrev;
		public float Field_40061_d;
		public float Field_40062_e;

		/// <summary>
		/// The amount that the book is open. </summary>
		public float BookSpread;

		/// <summary>
		/// The amount that the book is open. </summary>
		public float BookSpreadPrev;
		public float BookRotation2;
		public float BookRotationPrev;
		public float BookRotation;
		private static Random Rand = new Random();

		public TileEntityEnchantmentTable()
		{
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public override void UpdateEntity()
		{
			base.UpdateEntity();
			BookSpreadPrev = BookSpread;
			BookRotationPrev = BookRotation2;
			EntityPlayer entityplayer = WorldObj.GetClosestPlayer(XCoord + 0.5F, YCoord + 0.5F, ZCoord + 0.5F, 3);

			if (entityplayer != null)
			{
				double d = entityplayer.PosX - (double)((float)XCoord + 0.5F);
				double d1 = entityplayer.PosZ - (double)((float)ZCoord + 0.5F);
				BookRotation = (float)Math.Atan2(d1, d);
				BookSpread += 0.1F;

				if (BookSpread < 0.5F || Rand.Next(40) == 0)
				{
					float f3 = Field_40061_d;

					do
					{
						Field_40061_d += Rand.Next(4) - Rand.Next(4);
					}
					while (f3 == Field_40061_d);
				}
			}
			else
			{
				BookRotation += 0.02F;
				BookSpread -= 0.1F;
			}

			for (; BookRotation2 >= (float)Math.PI; BookRotation2 -= ((float)Math.PI * 2F))
			{
			}

			for (; BookRotation2 < -(float)Math.PI; BookRotation2 += ((float)Math.PI * 2F))
			{
			}

			for (; BookRotation >= (float)Math.PI; BookRotation -= ((float)Math.PI * 2F))
			{
			}

			for (; BookRotation < -(float)Math.PI; BookRotation += ((float)Math.PI * 2F))
			{
			}

			float f;

			for (f = BookRotation - BookRotation2; f >= (float)Math.PI; f -= ((float)Math.PI * 2F))
			{
			}

			for (; f < -(float)Math.PI; f += ((float)Math.PI * 2F))
			{
			}

			BookRotation2 += f * 0.4F;

			if (BookSpread < 0.0F)
			{
				BookSpread = 0.0F;
			}

			if (BookSpread > 1.0F)
			{
				BookSpread = 1.0F;
			}

			TickCount++;
			PageFlipPrev = PageFlip;
			float f1 = (Field_40061_d - PageFlip) * 0.4F;
			float f2 = 0.2F;

			if (f1 < -f2)
			{
				f1 = -f2;
			}

			if (f1 > f2)
			{
				f1 = f2;
			}

			Field_40062_e += (f1 - Field_40062_e) * 0.9F;
			PageFlip = PageFlip + Field_40062_e;
		}
	}

}