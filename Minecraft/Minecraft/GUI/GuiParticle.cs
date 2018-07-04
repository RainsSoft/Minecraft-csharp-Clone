using System.Collections.Generic;

namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiParticle : Gui
	{
		private List<Particle> Particles;
		private Minecraft Mc;

		public GuiParticle(Minecraft par1Minecraft)
		{
			Particles = new List<Particle>();
			Mc = par1Minecraft;
		}

		public virtual void Update()
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				Particle particle = (Particle)Particles[i];
				particle.PreUpdate();
				particle.Update(this);

				if (particle.IsDead)
				{
					Particles.RemoveAt(i--);
				}
			}
		}

		public virtual void Draw(float par1)
		{
			Mc.RenderEngineOld.BindTexture(Mc.RenderEngineOld.GetTexture("/gui/particles.png"));

			for (int i = 0; i < Particles.Count; i++)
			{
				Particle particle = (Particle)Particles[i];
				int j = (int)((particle.PrevPosX + (particle.PosX - particle.PrevPosX) * (double)par1) - 4D);
				int k = (int)((particle.PrevPosY + (particle.PosY - particle.PrevPosY) * (double)par1) - 4D);
				float f = (float)(particle.PrevTintAlpha + (particle.TintAlpha - particle.PrevTintAlpha) * (double)par1);
				float f1 = (float)(particle.PrevTintRed + (particle.TintRed - particle.PrevTintRed) * (double)par1);
				float f2 = (float)(particle.PrevTintGreen + (particle.TintGreen - particle.PrevTintGreen) * (double)par1);
				float f3 = (float)(particle.PrevTintBlue + (particle.TintBlue - particle.PrevTintBlue) * (double)par1);
				//GL.Color4(f1, f2, f3, f);
				DrawTexturedModalRect(j, k, 40, 0, 8, 8);
			}
		}
	}
}