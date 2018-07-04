namespace net.minecraft.src
{
	sealed class StepSoundStone : StepSound
	{
		public StepSoundStone(string par1Str, float par2, float par3) : base(par1Str, par2, par3)
		{
		}

		/// <summary>
		/// Used when a block breaks, EXA: Player break, Shep eating grass, etc..
		/// </summary>
		public override string GetBreakSound()
		{
			return "random.glass";
		}
	}
}