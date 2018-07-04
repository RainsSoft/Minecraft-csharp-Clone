using System.Text;

namespace net.minecraft.src
{

	public class StepSound
	{
		public readonly string StepSoundName;
		public readonly float StepSoundVolume;
		public readonly float StepSoundPitch;

		public StepSound(string par1Str, float par2, float par3)
		{
			StepSoundName = par1Str;
			StepSoundVolume = par2;
			StepSoundPitch = par3;
		}

		public virtual float GetVolume()
		{
			return StepSoundVolume;
		}

		public virtual float GetPitch()
		{
			return StepSoundPitch;
		}

		/// <summary>
		/// Used when a block breaks, EXA: Player break, Shep eating grass, etc..
		/// </summary>
		public virtual string GetBreakSound()
		{
			return (new StringBuilder()).Append("step.").Append(StepSoundName).ToString();
		}

		/// <summary>
		/// Used when a entity walks over, or otherwise interacts with the block.
		/// </summary>
		public virtual string GetStepSound()
		{
			return (new StringBuilder()).Append("step.").Append(StepSoundName).ToString();
		}
	}

}