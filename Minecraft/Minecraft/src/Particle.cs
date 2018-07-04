using System;

namespace net.minecraft.src
{
	public class Particle
	{
		private static Random Rand = new Random();
		public double PosX;
		public double PosY;
		public double PrevPosX;
		public double PrevPosY;
		public double VelocityX;
		public double VelocityY;
		public double AccelScale;
		public bool IsDead;
		public int TimeTick;
		public int TimeLimit;
		public double TintRed;
		public double TintGreen;
		public double TintBlue;
		public double TintAlpha;
		public double PrevTintRed;
		public double PrevTintGreen;
		public double PrevTintBlue;
		public double PrevTintAlpha;

		public virtual void Update(GuiParticle par1GuiParticle)
		{
			PosX += VelocityX;
			PosY += VelocityY;
			VelocityX *= AccelScale;
			VelocityY *= AccelScale;
			VelocityY += 0.10000000000000001D;

			if (++TimeTick > TimeLimit)
			{
				SetDead();
			}

			TintAlpha = 2D - ((double)TimeTick / (double)TimeLimit) * 2D;

			if (TintAlpha > 1.0D)
			{
				TintAlpha = 1.0D;
			}

			TintAlpha = TintAlpha * TintAlpha;
			TintAlpha *= 0.5D;
		}

		public virtual void PreUpdate()
		{
			PrevTintRed = TintRed;
			PrevTintGreen = TintGreen;
			PrevTintBlue = TintBlue;
			PrevTintAlpha = TintAlpha;
			PrevPosX = PosX;
			PrevPosY = PosY;
		}

		public virtual void SetDead()
		{
			IsDead = true;
		}
	}

}