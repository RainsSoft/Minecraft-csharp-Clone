using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderList
	{
		private int Field_1242_a;
		private int Field_1241_b;
		private int Field_1240_c;
		private double Field_1239_d;
		private double Field_1238_e;
		private double Field_1237_f;
		private Buffer<int> Field_1236_g;
		private bool Field_1235_h;
		private bool Field_1234_i;

		public RenderList()
		{
            Field_1236_g = new Buffer<int>(0x10000);// GLAllocation.CreateDirectIntBuffer(0x10000);
			Field_1235_h = false;
			Field_1234_i = false;
		}

		public virtual void Func_861_a(int par1, int par2, int par3, double par4, double par6, double par8)
		{
			Field_1235_h = true;
			Field_1236_g.Clear();
			Field_1242_a = par1;
			Field_1241_b = par2;
			Field_1240_c = par3;
			Field_1239_d = par4;
			Field_1238_e = par6;
			Field_1237_f = par8;
		}

		public virtual bool Func_862_a(int par1, int par2, int par3)
		{
			if (!Field_1235_h)
			{
				return false;
			}
			else
			{
				return par1 == Field_1242_a && par2 == Field_1241_b && par3 == Field_1240_c;
			}
		}

		public virtual void Func_858_a(int par1)
		{
			Field_1236_g.Put(par1);

			if (Field_1236_g.Remaining == 0)
			{
				Func_860_a();
			}
		}

		public virtual void Func_860_a()
		{
			if (!Field_1235_h)
			{
				return;
			}

			if (!Field_1234_i)
			{
				Field_1236_g.Flip();
				Field_1234_i = true;
			}

			if (Field_1236_g.Remaining > 0)
			{
				//GL.PushMatrix();
				//GL.Translate((float)((double)Field_1242_a - Field_1239_d), (float)((double)Field_1241_b - Field_1238_e), (float)((double)Field_1240_c - Field_1237_f));
				//GL.CallLists(0, ListNameType.Int, ref Field_1236_g);
				//GL.PopMatrix();
			}
		}

		public virtual void Func_859_b()
		{
			Field_1235_h = false;
			Field_1234_i = false;
		}
	}
}