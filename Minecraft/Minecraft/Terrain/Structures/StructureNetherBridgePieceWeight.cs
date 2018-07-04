using System;

namespace net.minecraft.src
{
	public class StructureNetherBridgePieceWeight
	{
		public Type Field_40699_a;
		public readonly int Field_40697_b;
		public int Field_40698_c;
		public int Field_40695_d;
		public bool Field_40696_e;

		public StructureNetherBridgePieceWeight(Type par1Class, int par2, int par3, bool par4)
		{
			Field_40699_a = par1Class;
			Field_40697_b = par2;
			Field_40695_d = par3;
			Field_40696_e = par4;
		}

		public StructureNetherBridgePieceWeight(Type par1Class, int par2, int par3) : this(par1Class, par2, par3, false)
		{
		}

		public virtual bool Func_40693_a(int par1)
		{
			return Field_40695_d == 0 || Field_40698_c < Field_40695_d;
		}

		public virtual bool Func_40694_a()
		{
			return Field_40695_d == 0 || Field_40698_c < Field_40695_d;
		}
	}
}