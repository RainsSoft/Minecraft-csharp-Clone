namespace net.minecraft.src
{
	public class Timer
	{
		/// <summary>
		/// The number of timer ticks per second of real time </summary>
		float TicksPerSecond;

		/// <summary>
		/// The time reported by the high-resolution clock at the last call of updateTimer(), in seconds
		/// </summary>
		private double LastHRTime;

		/// <summary>
		/// How many full ticks have turned over since the last call to updateTimer(), capped at 10.
		/// </summary>
		public int ElapsedTicks;

		/// <summary>
		/// How much time has elapsed since the last tick, in ticks, for use by display rendering routines (range: 0.0 -
		/// 1.0).  This field is frozen if the display is paused to eliminate jitter.
		/// </summary>
		public float RenderPartialTicks;

		/// <summary>
		/// A multiplier to make the timer (and therefore the game) go faster or slower.  0.5 makes the game run at half-
		/// speed.
		/// </summary>
		public float TimerSpeed;

		/// <summary>
		/// How much time has elapsed since the last tick, in ticks (range: 0.0 - 1.0).
		/// </summary>
		public float ElapsedPartialTicks;

		/// <summary>
		/// The time reported by the system clock at the last sync, in milliseconds
		/// </summary>
		private long LastSyncSysClock;

		/// <summary>
		/// The time reported by the high-resolution clock at the last sync, in milliseconds
		/// </summary>
		private long LastSyncHRClock;
		private long Field_28132_i;

		/// <summary>
		/// A ratio used to sync the high-resolution clock to the system clock, updated once per second
		/// </summary>
		private double TimeSyncAdjustment;

		public Timer(float par1)
		{
			TimerSpeed = 1.0F;
			ElapsedPartialTicks = 0.0F;
			TimeSyncAdjustment = 1.0D;
			TicksPerSecond = par1;
			LastSyncSysClock = JavaHelper.CurrentTimeMillis();
			LastSyncHRClock = JavaHelper.NanoTime() / 0xf4240L;
		}

		/// <summary>
		/// Updates all fields of the Timer using the current time
		/// </summary>
		public virtual void UpdateTimer()
		{
			long l = JavaHelper.CurrentTimeMillis();
			long l1 = l - LastSyncSysClock;
			long l2 = JavaHelper.NanoTime() / 0xf4240L;
			double d = (double)l2 / 1000D;

			if (l1 > 1000L)
			{
				LastHRTime = d;
			}
			else if (l1 < 0L)
			{
				LastHRTime = d;
			}
			else
			{
				Field_28132_i += l1;

				if (Field_28132_i > 1000L)
				{
					long l3 = l2 - LastSyncHRClock;
					double d2 = (double)Field_28132_i / (double)l3;
					TimeSyncAdjustment += (d2 - TimeSyncAdjustment) * 0.20000000298023224D;
					LastSyncHRClock = l2;
					Field_28132_i = 0L;
				}

				if (Field_28132_i < 0L)
				{
					LastSyncHRClock = l2;
				}
			}

			LastSyncSysClock = l;
			double d1 = (d - LastHRTime) * TimeSyncAdjustment;
			LastHRTime = d;

			if (d1 < 0.0F)
			{
				d1 = 0.0F;
			}

			if (d1 > 1.0D)
			{
				d1 = 1.0D;
			}

			ElapsedPartialTicks += (float)d1 * TimerSpeed * TicksPerSecond;
			ElapsedTicks = (int)ElapsedPartialTicks;
			ElapsedPartialTicks -= ElapsedTicks;

			if (ElapsedTicks > 10)
			{
				ElapsedTicks = 10;
			}

			RenderPartialTicks = ElapsedPartialTicks;
		}
	}
}