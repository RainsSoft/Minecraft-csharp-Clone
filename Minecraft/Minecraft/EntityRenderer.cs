using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using net.minecraft.src;

namespace net.minecraft.src
{
    public class EntityRenderer
    {
        public static bool AnaglyphEnable = false;

        /// <summary>
        /// Anaglyph field (0=R, 1=GB) </summary>
        public static int AnaglyphField;

        /// <summary>
        /// A reference to the Minecraft object. </summary>
        private Minecraft mc;
        private float farPlaneDistance;
        public ItemRenderer ItemRenderer;

        /// <summary>
        /// Entity renderer update count </summary>
        private int rendererUpdateCount;

        /// <summary>
        /// Pointed entity </summary>
        private Entity pointedEntity;
        private MouseFilter mouseFilterXAxis;
        private MouseFilter mouseFilterYAxis;

        /// <summary>
        /// Mouse filter dummy 1 </summary>
        private MouseFilter mouseFilterDummy1;

        /// <summary>
        /// Mouse filter dummy 2 </summary>
        private MouseFilter mouseFilterDummy2;

        /// <summary>
        /// Mouse filter dummy 3 </summary>
        private MouseFilter mouseFilterDummy3;

        /// <summary>
        /// Mouse filter dummy 4 </summary>
        private MouseFilter mouseFilterDummy4;
        private float thirdPersonDistance;

        /// <summary>
        /// Third person distance temp </summary>
        private float thirdPersonDistanceTemp;
        private float debugCamYaw;
        private float prevDebugCamYaw;
        private float debugCamPitch;
        private float prevDebugCamPitch;

        /// <summary>
        /// Smooth cam yaw </summary>
        private float SmoothCamYaw;

        /// <summary>
        /// Smooth cam pitch </summary>
        private float SmoothCamPitch;

        /// <summary>
        /// Smooth cam filter X </summary>
        private float SmoothCamFilterX;

        /// <summary>
        /// Smooth cam filter Y </summary>
        private float SmoothCamFilterY;

        /// <summary>
        /// Smooth cam partial ticks </summary>
        private float SmoothCamPartialTicks;
        private float DebugCamFOV;
        private float PrevDebugCamFOV;
        private float CamRoll;
        private float PrevCamRoll;

        /// <summary>
        /// The texture id of the blocklight/skylight texture used for lighting effects
        /// </summary>
        public int LightmapTexture;
        private int[] LightmapColors;

        /// <summary>
        /// FOV modifier hand </summary>
        private float FovModifierHand;

        /// <summary>
        /// FOV modifier hand prev </summary>
        private float FovModifierHandPrev;

        /// <summary>
        /// FOV multiplier temp </summary>
        private float FovMultiplierTemp;

        /// <summary>
        /// Cloud fog mode </summary>
        private bool CloudFog;
        private double CameraZoom;
        private double CameraYaw;
        private double CameraPitch;

        /// <summary>
        /// Previous frame time in milliseconds </summary>
        private long PrevFrameTime;

        /// <summary>
        /// End time of last render (ns) </summary>
        private long RenderEndNanoTime;

        /// <summary>
        /// Is set, updateCameraAndRender() calls updateLightmap(); set by updateTorchFlicker()
        /// </summary>
        private bool LightmapUpdateNeeded;

        /// <summary>
        /// Torch flicker X </summary>
        float TorchFlickerX;

        /// <summary>
        /// Torch flicker DX </summary>
        float TorchFlickerDX;

        /// <summary>
        /// Torch flicker Y </summary>
        float TorchFlickerY;

        /// <summary>
        /// Torch flicker DY </summary>
        float TorchFlickerDY;
        private Random Random;

        /// <summary>
        /// Rain sound counter </summary>
        private int RainSoundCounter;
        float[] RainXCoords;
        float[] RainYCoords;
        volatile int Field_1394_b;
        volatile int Field_1393_c;

        /// <summary>
        /// Fog color buffer </summary>
        //FloatBuffer FogColorBuffer;

        /// <summary>
        /// red component of the fog color </summary>
        float FogColorRed;

        /// <summary>
        /// green component of the fog color </summary>
        float FogColorGreen;

        /// <summary>
        /// blue component of the fog color </summary>
        float FogColorBlue;

        /// <summary>
        /// Fog color 2 </summary>
        private float FogColor2;

        /// <summary>
        /// Fog color 1 </summary>
        private float FogColor1;

        /// <summary>
        /// Debug view direction (0=OFF, 1=Front, 2=Right, 3=Back, 4=Left, 5=TiltLeft, 6=TiltRight)
        /// </summary>
        public int DebugViewDirection;

        public EntityRenderer(Minecraft par1Minecraft)
        {
            farPlaneDistance = 0.0F;
            pointedEntity = null;
            mouseFilterXAxis = new MouseFilter();
            mouseFilterYAxis = new MouseFilter();
            mouseFilterDummy1 = new MouseFilter();
            mouseFilterDummy2 = new MouseFilter();
            mouseFilterDummy3 = new MouseFilter();
            mouseFilterDummy4 = new MouseFilter();
            thirdPersonDistance = 4F;
            thirdPersonDistanceTemp = 4F;
            debugCamYaw = 0.0F;
            prevDebugCamYaw = 0.0F;
            debugCamPitch = 0.0F;
            prevDebugCamPitch = 0.0F;
            DebugCamFOV = 0.0F;
            PrevDebugCamFOV = 0.0F;
            CamRoll = 0.0F;
            PrevCamRoll = 0.0F;
            CloudFog = false;
            CameraZoom = 1.0D;
            CameraYaw = 0.0F;
            CameraPitch = 0.0F;
            PrevFrameTime = JavaHelper.CurrentTimeMillis();
            RenderEndNanoTime = 0L;
            LightmapUpdateNeeded = false;
            TorchFlickerX = 0.0F;
            TorchFlickerDX = 0.0F;
            TorchFlickerY = 0.0F;
            TorchFlickerDY = 0.0F;
            Random = new Random();
            RainSoundCounter = 0;
            Field_1394_b = 0;
            Field_1393_c = 0;
            //FogColorBuffer = GLAllocation.CreateDirectFloatBuffer(16);
            mc = par1Minecraft;
            ItemRenderer = new ItemRenderer(par1Minecraft);
            //LightmapTexture = par1Minecraft.RenderEngineOld.AllocateAndSetupTexture(new Bitmap(16, 16));
            LightmapColors = new int[256];
        }

        /// <summary>
        /// Updates the entity renderer
        /// </summary>
        public virtual void UpdateRenderer()
        {
            UpdateFovModifierHand();
            UpdateTorchFlicker();
            FogColor2 = FogColor1;
            thirdPersonDistanceTemp = thirdPersonDistance;
            prevDebugCamYaw = debugCamYaw;
            prevDebugCamPitch = debugCamPitch;
            PrevDebugCamFOV = DebugCamFOV;
            PrevCamRoll = CamRoll;

            if (mc.GameSettings.SmoothCamera)
            {
                float f = mc.GameSettings.MouseSensitivity * 0.6F + 0.2F;
                float f2 = f * f * f * 8F;
                SmoothCamFilterX = mouseFilterXAxis.Func_22386_a(SmoothCamYaw, 0.05F * f2);
                SmoothCamFilterY = mouseFilterYAxis.Func_22386_a(SmoothCamPitch, 0.05F * f2);
                SmoothCamPartialTicks = 0.0F;
                SmoothCamYaw = 0.0F;
                SmoothCamPitch = 0.0F;
            }

            if (mc.RenderViewEntity == null)
            {
                mc.RenderViewEntity = mc.ThePlayer;
            }

            float f1 = mc.TheWorld.GetLightBrightness(MathHelper2.Floor_double(mc.RenderViewEntity.PosX), MathHelper2.Floor_double(mc.RenderViewEntity.PosY), MathHelper2.Floor_double(mc.RenderViewEntity.PosZ));
            float f3 = (float)(3 - mc.GameSettings.RenderDistance) / 3F;
            float f4 = f1 * (1.0F - f3) + f3;
            FogColor1 += (f4 - FogColor1) * 0.1F;
            rendererUpdateCount++;
            ItemRenderer.UpdateEquippedItem();
            AddRainParticles();
        }

        /// <summary>
        /// Finds what block or object the mouse is over at the specified partial tick time. Args: partialTickTime
        /// </summary>
        public virtual void GetMouseOver(float par1)
        {
            if (mc.RenderViewEntity == null)
            {
                return;
            }

            if (mc.TheWorld == null)
            {
                return;
            }

            float d = mc.PlayerController.GetBlockReachDistance();
            mc.ObjectMouseOver = mc.RenderViewEntity.RayTrace(d, par1);
            float d1 = d;
            Vec3D vec3d = mc.RenderViewEntity.GetPosition(par1);

            if (mc.PlayerController.ExtendedReach())
            {
                d1 = d = 6;
            }
            else
            {
                if (d1 > 3)
                {
                    d1 = 3;
                }

                d = d1;
            }

            if (mc.ObjectMouseOver != null)
            {
                d1 = (float)mc.ObjectMouseOver.HitVec.DistanceTo(vec3d);
            }

            Vec3D vec3d1 = mc.RenderViewEntity.GetLook(par1);
            Vec3D vec3d2 = vec3d.AddVector(vec3d1.XCoord * d, vec3d1.YCoord * d, vec3d1.ZCoord * d);
            pointedEntity = null;
            float f = 1.0F;
            List<Entity> list = mc.TheWorld.GetEntitiesWithinAABBExcludingEntity(mc.RenderViewEntity, mc.RenderViewEntity.BoundingBox.AddCoord((float)vec3d1.XCoord * d, (float)vec3d1.YCoord * d, (float)vec3d1.ZCoord * d).Expand(f, f, f));
            double d2 = d1;

            for (int i = 0; i < list.Count; i++)
            {
                Entity entity = (Entity)list[i];

                if (!entity.CanBeCollidedWith())
                {
                    continue;
                }

                float f1 = entity.GetCollisionBorderSize();
                AxisAlignedBB axisalignedbb = entity.BoundingBox.Expand(f1, f1, f1);
                MovingObjectPosition movingobjectposition = axisalignedbb.CalculateIntercept(vec3d, vec3d2);

                if (axisalignedbb.IsVecInside(vec3d))
                {
                    if (0.0F < d2 || d2 == 0.0F)
                    {
                        pointedEntity = entity;
                        d2 = 0.0F;
                    }

                    continue;
                }

                if (movingobjectposition == null)
                {
                    continue;
                }

                double d3 = vec3d.DistanceTo(movingobjectposition.HitVec);

                if (d3 < d2 || d2 == 0.0F)
                {
                    pointedEntity = entity;
                    d2 = d3;
                }
            }

            if (pointedEntity != null && (d2 < d1 || mc.ObjectMouseOver == null))
            {
                mc.ObjectMouseOver = new MovingObjectPosition(pointedEntity);
            }
        }

        /// <summary>
        /// Update FOV modifier hand
        /// </summary>
        private void UpdateFovModifierHand()
        {
            EntityPlayerSP entityplayersp = (EntityPlayerSP)mc.RenderViewEntity;
            FovMultiplierTemp = entityplayersp.GetFOVMultiplier();
            FovModifierHandPrev = FovModifierHand;
            FovModifierHand += (FovMultiplierTemp - FovModifierHand) * 0.5F;
        }

        /// <summary>
        /// Changes the field of view of the player depending on if they are underwater or not
        /// </summary>
        private float GetFOVModifier(float par1, bool par2)
        {
            if (DebugViewDirection > 0)
            {
                return 90F;
            }

            EntityPlayer entityplayer = (EntityPlayer)mc.RenderViewEntity;
            float f = 70F;

            if (par2)
            {
                f += mc.GameSettings.FovSetting * 40F;
                f *= FovModifierHandPrev + (FovModifierHand - FovModifierHandPrev) * par1;
            }

            if (entityplayer.GetHealth() <= 0)
            {
                float f1 = (float)entityplayer.DeathTime + par1;
                f /= (1.0F - 500F / (f1 + 500F)) * 2.0F + 1.0F;
            }

            int i = ActiveRenderInfo.GetBlockIdAtEntityViewpoint(mc.TheWorld, entityplayer, par1);

            if (i != 0 && Block.BlocksList[i].BlockMaterial == Material.Water)
            {
                f = (f * 60F) / 70F;
            }

            return f + PrevDebugCamFOV + (DebugCamFOV - PrevDebugCamFOV) * par1;
        }

        private void HurtCameraEffect(float par1)
        {
            EntityLiving entityliving = mc.RenderViewEntity;
            float f = (float)entityliving.HurtTime - par1;

            if (entityliving.GetHealth() <= 0)
            {
                float f1 = (float)entityliving.DeathTime + par1;
                //GL.Rotate(40F - 8000F / (f1 + 200F), 0.0F, 0.0F, 1.0F);
            }

            if (f < 0.0F)
            {
                return;
            }
            else
            {
                f /= entityliving.MaxHurtTime;
                f = MathHelper2.Sin(f * f * f * f * (float)Math.PI);
                float f2 = entityliving.AttackedAtYaw;
                //GL.Rotate(-f2, 0.0F, 1.0F, 0.0F);
                //GL.Rotate(-f * 14F, 0.0F, 0.0F, 1.0F);
                //GL.Rotate(f2, 0.0F, 1.0F, 0.0F);
                return;
            }
        }

        /// <summary>
        /// Setups all the GL settings for view bobbing. Args: partialTickTime
        /// </summary>
        private void SetupViewBobbing(float par1)
        {
            if (!(mc.RenderViewEntity is EntityPlayer))
            {
                return;
            }
            else
            {
                EntityPlayer entityplayer = (EntityPlayer)mc.RenderViewEntity;
                float f = entityplayer.DistanceWalkedModified - entityplayer.PrevDistanceWalkedModified;
                float f1 = -(entityplayer.DistanceWalkedModified + f * par1);
                float f2 = entityplayer.PrevCameraYaw + (entityplayer.CameraYaw - entityplayer.PrevCameraYaw) * par1;
                float f3 = entityplayer.PrevCameraPitch + (entityplayer.CameraPitch - entityplayer.PrevCameraPitch) * par1;
                //GL.Translate(MathHelper.Sin(f1 * (float)Math.PI) * f2 * 0.5F, -Math.Abs(MathHelper.Cos(f1 * (float)Math.PI) * f2), 0.0F);
                //GL.Rotate(MathHelper.Sin(f1 * (float)Math.PI) * f2 * 3F, 0.0F, 0.0F, 1.0F);
                //GL.Rotate(Math.Abs(MathHelper.Cos(f1 * (float)Math.PI - 0.2F) * f2) * 5F, 1.0F, 0.0F, 0.0F);
                //GL.Rotate(f3, 1.0F, 0.0F, 0.0F);
                return;
            }
        }

        /// <summary>
        /// sets up player's eye (or camera in third person mode)
        /// </summary>
        private void OrientCamera(float par1)
        {
            EntityLiving entityliving = mc.RenderViewEntity;
            float f = entityliving.YOffset - 1.62F;
            double d = entityliving.PrevPosX + (entityliving.PosX - entityliving.PrevPosX) * (double)par1;
            double d1 = (entityliving.PrevPosY + (entityliving.PosY - entityliving.PrevPosY) * (double)par1) - (double)f;
            double d2 = entityliving.PrevPosZ + (entityliving.PosZ - entityliving.PrevPosZ) * (double)par1;
            //GL.Rotate(PrevCamRoll + (CamRoll - PrevCamRoll) * par1, 0.0F, 0.0F, 1.0F);

            if (entityliving.IsPlayerSleeping())
            {
                f = (float)((double)f + 1.0D);
                //GL.Translate(0.0F, 0.3F, 0.0F);

                if (!mc.GameSettings.DebugCamEnable)
                {
                    int i = mc.TheWorld.GetBlockId(MathHelper2.Floor_double(entityliving.PosX), MathHelper2.Floor_double(entityliving.PosY), MathHelper2.Floor_double(entityliving.PosZ));

                    if (i == Block.Bed.BlockID)
                    {
                        int j = mc.TheWorld.GetBlockMetadata(MathHelper2.Floor_double(entityliving.PosX), MathHelper2.Floor_double(entityliving.PosY), MathHelper2.Floor_double(entityliving.PosZ));
                        int k = j & 3;
                        //GL.Rotate(k * 90, 0.0F, 1.0F, 0.0F);
                    }

                    //GL.Rotate(entityliving.PrevRotationYaw + (entityliving.RotationYaw - entityliving.PrevRotationYaw) * par1 + 180F, 0.0F, -1F, 0.0F);
                    //GL.Rotate(entityliving.PrevRotationPitch + (entityliving.RotationPitch - entityliving.PrevRotationPitch) * par1, -1F, 0.0F, 0.0F);
                }
            }
            else if (mc.GameSettings.ThirdPersonView > 0)
            {
                double d3 = thirdPersonDistanceTemp + (thirdPersonDistance - thirdPersonDistanceTemp) * par1;

                if (mc.GameSettings.DebugCamEnable)
                {
                    float f1 = prevDebugCamYaw + (debugCamYaw - prevDebugCamYaw) * par1;
                    float f3 = prevDebugCamPitch + (debugCamPitch - prevDebugCamPitch) * par1;
                    //GL.Translate(0.0F, 0.0F, (float)(-d3));
                    //GL.Rotate(f3, 1.0F, 0.0F, 0.0F);
                    //GL.Rotate(f1, 0.0F, 1.0F, 0.0F);
                }
                else
                {
                    float f2 = entityliving.RotationYaw;
                    float f4 = entityliving.RotationPitch;

                    if (mc.GameSettings.ThirdPersonView == 2)
                    {
                        f4 += 180F;
                    }

                    double d4 = (double)(-MathHelper2.Sin((f2 / 180F) * (float)Math.PI) * MathHelper2.Cos((f4 / 180F) * (float)Math.PI)) * d3;
                    double d5 = (double)(MathHelper2.Cos((f2 / 180F) * (float)Math.PI) * MathHelper2.Cos((f4 / 180F) * (float)Math.PI)) * d3;
                    double d6 = (double)(-MathHelper2.Sin((f4 / 180F) * (float)Math.PI)) * d3;

                    for (int l = 0; l < 8; l++)
                    {
                        float f5 = (l & 1) * 2 - 1;
                        float f6 = (l >> 1 & 1) * 2 - 1;
                        float f7 = (l >> 2 & 1) * 2 - 1;
                        f5 *= 0.1F;
                        f6 *= 0.1F;
                        f7 *= 0.1F;
                        MovingObjectPosition movingobjectposition = mc.TheWorld.RayTraceBlocks(Vec3D.CreateVector(d + (double)f5, d1 + (double)f6, d2 + (double)f7), Vec3D.CreateVector((d - d4) + (double)f5 + (double)f7, (d1 - d6) + (double)f6, (d2 - d5) + (double)f7));

                        if (movingobjectposition == null)
                        {
                            continue;
                        }

                        double d7 = movingobjectposition.HitVec.DistanceTo(Vec3D.CreateVector(d, d1, d2));

                        if (d7 < d3)
                        {
                            d3 = d7;
                        }
                    }

                    if (mc.GameSettings.ThirdPersonView == 2)
                    {
                        //GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
                    }

                    //GL.Rotate(entityliving.RotationPitch - f4, 1.0F, 0.0F, 0.0F);
                    //GL.Rotate(entityliving.RotationYaw - f2, 0.0F, 1.0F, 0.0F);
                    //GL.Translate(0.0F, 0.0F, (float)(-d3));
                    //GL.Rotate(f2 - entityliving.RotationYaw, 0.0F, 1.0F, 0.0F);
                    //GL.Rotate(f4 - entityliving.RotationPitch, 1.0F, 0.0F, 0.0F);
                }
            }
            else
            {
                //GL.Translate(0.0F, 0.0F, -0.1F);
            }

            if (!mc.GameSettings.DebugCamEnable)
            {
                //GL.Rotate(entityliving.PrevRotationPitch + (entityliving.RotationPitch - entityliving.PrevRotationPitch) * par1, 1.0F, 0.0F, 0.0F);
                //GL.Rotate(entityliving.PrevRotationYaw + (entityliving.RotationYaw - entityliving.PrevRotationYaw) * par1 + 180F, 0.0F, 1.0F, 0.0F);
            }

            //GL.Translate(0.0F, f, 0.0F);
            d = entityliving.PrevPosX + (entityliving.PosX - entityliving.PrevPosX) * (double)par1;
            d1 = (entityliving.PrevPosY + (entityliving.PosY - entityliving.PrevPosY) * (double)par1) - (double)f;
            d2 = entityliving.PrevPosZ + (entityliving.PosZ - entityliving.PrevPosZ) * (double)par1;
            CloudFog = mc.RenderGlobal.Func_27307_a(d, d1, d2, par1);
        }

        /// <summary>
        /// sets up projection, view effects, camera position/rotation
        /// </summary>
        private void SetupCameraTransform(float par1, int par2)
        {
            farPlaneDistance = 256 >> mc.GameSettings.RenderDistance;
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();

       //     if (mc.GameSettings.Anaglyph)
       //     {
                //GL.Translate((float)(-(par2 * 2 - 1)) * f, 0.0F, 0.0F);
        //    }

        //    if (CameraZoom != 1.0D)
        //    {
                //GL.Translate((float)CameraYaw, (float)(-CameraPitch), 0.0F);
                //GL.Scale(CameraZoom, CameraZoom, 1.0D);
        //    }

            Matrix.CreatePerspectiveFieldOfView(GetFOVModifier(par1, true), (float)mc.DisplayWidth / (float)mc.DisplayHeight, 0.05F, farPlaneDistance * 2.0F);

       //     if (mc.PlayerController.Func_35643_e())
       //     {
             //   float f1 = 0.6666667F;
                //GL.Scale(1.0F, f1, 1.0F);
      //      }

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

       //     if (mc.GameSettings.Anaglyph)
        //    {
                //GL.Translate((float)(par2 * 2 - 1) * 0.1F, 0.0F, 0.0F);
        //    }

            HurtCameraEffect(par1);

            if (mc.GameSettings.ViewBobbing)
            {
                SetupViewBobbing(par1);
            }

            float f2 = mc.ThePlayer.PrevTimeInPortal + (mc.ThePlayer.TimeInPortal - mc.ThePlayer.PrevTimeInPortal) * par1;

            if (f2 > 0.0F)
            {
                int i = 20;

                if (mc.ThePlayer.IsPotionActive(Potion.Confusion))
                {
                    i = 7;
                }

                float f3 = 5F / (f2 * f2 + 5F) - f2 * 0.04F;
                f3 *= f3;
                //GL.Rotate(((float)rendererUpdateCount + par1) * (float)i, 0.0F, 1.0F, 1.0F);
                //GL.Scale(1.0F / f3, 1.0F, 1.0F);
                //GL.Rotate(-((float)rendererUpdateCount + par1) * (float)i, 0.0F, 1.0F, 1.0F);
            }

            OrientCamera(par1);

            if (DebugViewDirection > 0)
            {
                int j = DebugViewDirection - 1;

                if (j == 1)
                {
                    //GL.Rotate(90F, 0.0F, 1.0F, 0.0F);
                }

                if (j == 2)
                {
                    //GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
                }

                if (j == 3)
                {
                    //GL.Rotate(-90F, 0.0F, 1.0F, 0.0F);
                }

                if (j == 4)
                {
                    //GL.Rotate(90F, 1.0F, 0.0F, 0.0F);
                }

                if (j == 5)
                {
                    //GL.Rotate(-90F, 1.0F, 0.0F, 0.0F);
                }
            }
        }

        /// <summary>
        /// Render player hand
        /// </summary>
        private void RenderHand(float par1, int par2)
        {
            if (DebugViewDirection > 0)
            {
                return;
            }

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
        //    float f = 0.07F;

        //    if (mc.GameSettings.Anaglyph)
        //    {
                //GL.Translate((float)(-(par2 * 2 - 1)) * f, 0.0F, 0.0F);
        //    }

        //    if (CameraZoom != 1.0D)
        //    {
                //GL.Translate((float)CameraYaw, (float)(-CameraPitch), 0.0F);
                //GL.Scale(CameraZoom, CameraZoom, 1.0D);
         //   }

            Matrix.CreatePerspectiveFieldOfView(GetFOVModifier(par1, false), (float)mc.DisplayWidth / (float)mc.DisplayHeight, 0.05F, farPlaneDistance * 2.0F);

        //    if (mc.PlayerController.Func_35643_e())
        //    {
             //   float f1 = 0.6666667F;
                //GL.Scale(1.0F, f1, 1.0F);
        //    }

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

        //    if (mc.GameSettings.Anaglyph)
        //    {
                //GL.Translate((float)(par2 * 2 - 1) * 0.1F, 0.0F, 0.0F);
        //    }

            //GL.PushMatrix();
            HurtCameraEffect(par1);

            if (mc.GameSettings.ViewBobbing)
            {
                SetupViewBobbing(par1);
            }

            if (mc.GameSettings.ThirdPersonView == 0 && !mc.RenderViewEntity.IsPlayerSleeping() && !mc.GameSettings.HideGUI && !mc.PlayerController.Func_35643_e())
            {
                EnableLightmap(par1);
                ItemRenderer.RenderItemInFirstPerson(par1);
                DisableLightmap(par1);
            }

            //GL.PopMatrix();

            if (mc.GameSettings.ThirdPersonView == 0 && !mc.RenderViewEntity.IsPlayerSleeping())
            {
                ItemRenderer.RenderOverlays(par1);
                HurtCameraEffect(par1);
            }

            if (mc.GameSettings.ViewBobbing)
            {
                SetupViewBobbing(par1);
            }
        }

        /// <summary>
        /// Disable secondary texture unit used by lightmap
        /// </summary>
        public virtual void DisableLightmap(double par1)
        {
            OpenGlHelper.SetActiveTexture(OpenGlHelper.LightmapTexUnit);
            //GL.Disable(EnableCap.Texture2D);
            OpenGlHelper.SetActiveTexture(OpenGlHelper.DefaultTexUnit);
        }

        /// <summary>
        /// Enable lightmap in secondary texture unit
        /// </summary>
        public virtual void EnableLightmap(double par1)
        {
            OpenGlHelper.SetActiveTexture(OpenGlHelper.LightmapTexUnit);
            //GL.MatrixMode(MatrixMode.Texture);
            //GL.LoadIdentity();
        //    float f = 0.00390625F;
            //GL.Scale(f, f, f);
            //GL.Translate(8F, 8F, 8F);
            //GL.MatrixMode(MatrixMode.Modelview);
            mc.RenderEngineOld.BindTexture(LightmapTexture);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            //GL.Enable(EnableCap.Texture2D);
            OpenGlHelper.SetActiveTexture(OpenGlHelper.DefaultTexUnit);
        }

        /// <summary>
        /// Recompute a random value that is applied to block color in updateLightmap()
        /// </summary>
        private void UpdateTorchFlicker()
        {
            TorchFlickerDX += (float)(((new Random(1)).NextDouble() - new Random(2).NextDouble()) * new Random(3).NextDouble() * new Random(4).NextDouble());
            TorchFlickerDY += (float)(((new Random(5)).NextDouble() - new Random(6).NextDouble()) * new Random(7).NextDouble() * new Random(8).NextDouble());
            TorchFlickerDX *= 0.90000000000000002F;
            TorchFlickerDY *= 0.90000000000000002F;
            TorchFlickerX += (TorchFlickerDX - TorchFlickerX) * 1.0F;
            TorchFlickerY += (TorchFlickerDY - TorchFlickerY) * 1.0F;
            LightmapUpdateNeeded = true;
        }

        private void UpdateLightmap()
        {
            World world = mc.TheWorld;

            if (world == null)
            {
                return;
            }

            for (int i = 0; i < 256; i++)
            {
                float f = world.Func_35464_b(1.0F) * 0.95F + 0.05F;
                float f1 = world.WorldProvider.LightBrightnessTable[i / 16] * f;
                float f2 = world.WorldProvider.LightBrightnessTable[i % 16] * (TorchFlickerX * 0.1F + 1.5F);

                if (world.LightningFlash > 0)
                {
                    f1 = world.WorldProvider.LightBrightnessTable[i / 16];
                }

                float f3 = f1 * (world.Func_35464_b(1.0F) * 0.65F + 0.35F);
                float f4 = f1 * (world.Func_35464_b(1.0F) * 0.65F + 0.35F);
                float f5 = f1;
                float f6 = f2;
                float f7 = f2 * ((f2 * 0.6F + 0.4F) * 0.6F + 0.4F);
                float f8 = f2 * (f2 * f2 * 0.6F + 0.4F);
                float f9 = f3 + f6;
                float f10 = f4 + f7;
                float f11 = f5 + f8;
                f9 = f9 * 0.96F + 0.03F;
                f10 = f10 * 0.96F + 0.03F;
                f11 = f11 * 0.96F + 0.03F;

                if (world.WorldProvider.TheWorldType == 1)
                {
                    f9 = 0.22F + f6 * 0.75F;
                    f10 = 0.28F + f7 * 0.75F;
                    f11 = 0.25F + f8 * 0.75F;
                }

                float f12 = mc.GameSettings.GammaSetting;

                if (f9 > 1.0F)
                {
                    f9 = 1.0F;
                }

                if (f10 > 1.0F)
                {
                    f10 = 1.0F;
                }

                if (f11 > 1.0F)
                {
                    f11 = 1.0F;
                }

                float f13 = 1.0F - f9;
                float f14 = 1.0F - f10;
                float f15 = 1.0F - f11;
                f13 = 1.0F - f13 * f13 * f13 * f13;
                f14 = 1.0F - f14 * f14 * f14 * f14;
                f15 = 1.0F - f15 * f15 * f15 * f15;
                f9 = f9 * (1.0F - f12) + f13 * f12;
                f10 = f10 * (1.0F - f12) + f14 * f12;
                f11 = f11 * (1.0F - f12) + f15 * f12;
                f9 = f9 * 0.96F + 0.03F;
                f10 = f10 * 0.96F + 0.03F;
                f11 = f11 * 0.96F + 0.03F;

                if (f9 > 1.0F)
                {
                    f9 = 1.0F;
                }

                if (f10 > 1.0F)
                {
                    f10 = 1.0F;
                }

                if (f11 > 1.0F)
                {
                    f11 = 1.0F;
                }

                if (f9 < 0.0F)
                {
                    f9 = 0.0F;
                }

                if (f10 < 0.0F)
                {
                    f10 = 0.0F;
                }

                if (f11 < 0.0F)
                {
                    f11 = 0.0F;
                }

                int c = 377;
                int j = (int)(f9 * 255F);
                int k = (int)(f10 * 255F);
                int l = (int)(f11 * 255F);
                LightmapColors[i] = c << 24 | j << 16 | k << 8 | l;
            }

            mc.RenderEngineOld.CreateTextureFromBytes(LightmapColors, 16, 16, LightmapTexture);
        }

        ///<summary>
        /// Will update any inputs that effect the camera angle (mouse) and then render the world and GUI
        ///</summary>
        public void UpdateCameraAndRender(float par1)
        {
            Profiler.StartSection("lightTex");

            if (LightmapUpdateNeeded)
            {
                UpdateLightmap();
            }

            Profiler.EndSection();

            /*if (!Display.isActive())
            {
                if (JavaHelper.CurrentTimeMillis() - PrevFrameTime > 500L)
                {
                    mc.DisplayInGameMenu();
                }
            }
            else*/
            {
                PrevFrameTime = JavaHelper.CurrentTimeMillis();
            }

            Profiler.StartSection("mouse");

            if (mc.InGameHasFocus)
            {
                //mc.Input.Mouse.MouseXYChange();
                float f = mc.GameSettings.MouseSensitivity * 0.6F + 0.2F;
                float f1 = f * f * f * 8F;
                float f2 = mc.Input.Mouse.Delta.X * f1;
                float f3 = mc.Input.Mouse.Delta.Y * f1;
                int l = 1;

                if (mc.GameSettings.InvertMouse)
                {
                    l = -1;
                }

                if (mc.GameSettings.SmoothCamera)
                {
                    SmoothCamYaw += f2;
                    SmoothCamPitch += f3;
                    float f4 = par1 - SmoothCamPartialTicks;
                    SmoothCamPartialTicks = par1;
                    f2 = SmoothCamFilterX * f4;
                    f3 = SmoothCamFilterY * f4;
                    mc.ThePlayer.SetAngles(f2, f3 * (float)l);
                }
                else
                {
                    mc.ThePlayer.SetAngles(f2, f3 * (float)l);
                }
            }

            Profiler.EndSection();

            if (mc.SkipRenderWorld)
            {
                return;
            }

            AnaglyphEnable = mc.GameSettings.Anaglyph;
            ScaledResolution scaledresolution = new ScaledResolution(mc.GameSettings, mc.DisplayWidth, mc.DisplayHeight);
            float i = scaledresolution.GetScaledWidth();
            float j = scaledresolution.GetScaledHeight();
            int mouseX = (int)(mc.Input.Mouse.X * (i / mc.DisplayWidth));
            int mouseY = (int)(mc.Input.Mouse.Y * (j / mc.DisplayHeight));
            char c = (char)0xc8;

            if (mc.GameSettings.LimitFramerate == 1)
            {
                c = 'x';
            }

            if (mc.GameSettings.LimitFramerate == 2)
            {
                c = '(';
            }

            if (mc.TheWorld != null)
            {
                Profiler.StartSection("level");

                if (mc.GameSettings.LimitFramerate == 0)
                {
                    RenderWorld(par1, 0L);
                }
                else
                {
                    RenderWorld(par1, RenderEndNanoTime + (long)(0x3b9aca00 / c));
                }

                Profiler.EndStartSection("sleep");

                if (mc.GameSettings.LimitFramerate == 2)
                {
                    int l1 = (int)(((RenderEndNanoTime + (long)(0x3b9aca00 / c)) - JavaHelper.NanoTime()) / 0xf4240L);

                    if (l1 > 0L && l1 < 500L)
                    {
                        try
                        {
                            Thread.Sleep(l1);
                        }
                        catch (ThreadInterruptedException interruptedexception)
                        {
                            Console.WriteLine(interruptedexception.StackTrace);
                        }
                    }
                }

                RenderEndNanoTime = JavaHelper.NanoTime();
                Profiler.EndStartSection("gui");
                
                if (!mc.GameSettings.HideGUI || mc.CurrentScreen != null)
                {
                    mc.IngameGUI.RenderGameOverlay(par1, mc.CurrentScreen != null, mouseX, mouseY);
                }
                
                Profiler.EndSection();
            }
            else
            {
                //GL.Viewport(0, 0, mc.DisplayWidth, mc.DisplayHeight);
                //GL.MatrixMode(MatrixMode.Projection);
                //GL.LoadIdentity();
                //GL.MatrixMode(MatrixMode.Modelview);
                //GL.LoadIdentity();
                SetupOverlayRendering();
                int l2 = (int)(((RenderEndNanoTime + (long)(0x3b9aca00 / c)) - JavaHelper.NanoTime()) / 0xf4240L);

                if (l2 < 0L)
                {
                    l2 += 10;
                }

                if (l2 > 0L && l2 < 500L)
                {
                    try
                    {
                        Thread.Sleep(l2);
                    }
                    catch (ThreadInterruptedException interruptedexception1)
                    {
                        Console.WriteLine(interruptedexception1.StackTrace);
                    }
                }

                RenderEndNanoTime = JavaHelper.NanoTime();
            }

            if (mc.CurrentScreen != null)
            {
                //GL.Clear(ClearBufferMask.ColorBufferBit);
                mc.CurrentScreen.DrawScreen(mouseX, mouseY, par1);

                if (mc.CurrentScreen != null && mc.CurrentScreen.GuiParticles != null)
                {
                    mc.CurrentScreen.GuiParticles.Draw(par1);
                }
            }
        }

        public void RenderWorld(float par1, long par2)
        {
            Profiler.StartSection("lightTex");

            if (LightmapUpdateNeeded)
            {
                UpdateLightmap();
            }

            //GL.Enable(EnableCap.CullFace);
            //GL.Enable(EnableCap.DepthTest);

            if (mc.RenderViewEntity == null)
            {
                mc.RenderViewEntity = mc.ThePlayer;
            }

            Profiler.EndStartSection("pick");
            GetMouseOver(par1);
            EntityLiving entityliving = mc.RenderViewEntity;
            RenderGlobal renderglobal = mc.RenderGlobal;
            EffectRenderer effectrenderer = mc.EffectRenderer;
            double d = entityliving.LastTickPosX + (entityliving.PosX - entityliving.LastTickPosX) * (double)par1;
            double d1 = entityliving.LastTickPosY + (entityliving.PosY - entityliving.LastTickPosY) * (double)par1;
            double d2 = entityliving.LastTickPosZ + (entityliving.PosZ - entityliving.LastTickPosZ) * (double)par1;
            Profiler.EndStartSection("center");
            IChunkProvider ichunkprovider = mc.TheWorld.GetChunkProvider();

            if (ichunkprovider is ChunkProviderLoadOrGenerate)
            {
                ChunkProviderLoadOrGenerate chunkproviderloadorgenerate = (ChunkProviderLoadOrGenerate)ichunkprovider;
                int j = MathHelper2.Floor_float((int)d) >> 4;
                int k = MathHelper2.Floor_float((int)d2) >> 4;
                chunkproviderloadorgenerate.SetCurrentChunkOver(j, k);
            }

            for (int i = 0; i < 2; i++)
            {
                if (mc.GameSettings.Anaglyph)
                {
                    AnaglyphField = i;

                    if (AnaglyphField == 0)
                    {
                        //GL.ColorMask(false, true, true, false);
                    }
                    else
                    {
                        //GL.ColorMask(true, false, false, false);
                    }
                }

                Profiler.EndStartSection("clear");
                //GL.Viewport(0, 0, mc.DisplayWidth, mc.DisplayHeight);
                UpdateFogColor(par1);
                //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                //GL.Enable(EnableCap.CullFace);
                Profiler.EndStartSection("camera");
                SetupCameraTransform(par1, i);
                ActiveRenderInfo.UpdateRenderInfo(mc.ThePlayer, mc.GameSettings.ThirdPersonView == 2);
                Profiler.EndStartSection("frustrum");
                ClippingHelperImpl.GetInstance();

                if (mc.GameSettings.RenderDistance < 2)
                {
                    SetupFog(-1, par1);
                    Profiler.EndStartSection("sky");
                    renderglobal.RenderSky(par1);
                }

                //GL.Enable(EnableCap.Fog);
                SetupFog(1, par1);

                if (mc.GameSettings.AmbientOcclusion)
                {
                    //GL.ShadeModel(ShadingModel.Smooth);
                }

                Profiler.EndStartSection("culling");
                Frustrum frustrum = new Frustrum();
                frustrum.SetPosition(d, d1, d2);
                mc.RenderGlobal.ClipRenderersByFrustum(frustrum, par1);

                if (i == 0)
                {
                    Profiler.EndStartSection("updatechunks");
                    long l;

                    do
                    {
                        if (mc.RenderGlobal.UpdateRenderers(entityliving, false) || par2 == 0L)
                        {
                            break;
                        }

                        l = par2 - JavaHelper.NanoTime();
                    }
                    while (l >= 0L && l <= 0x3b9aca00L);
                }

                SetupFog(0, par1);
                //GL.Enable(EnableCap.Fog);
                //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/terrain.png"));
                RenderHelper.DisableStandardItemLighting();
                Profiler.EndStartSection("terrain");
                renderglobal.SortAndRender(entityliving, 0, par1);
                //GL.ShadeModel(ShadingModel.Flat);

                if (DebugViewDirection == 0)
                {
                    RenderHelper.EnableStandardItemLighting();
                    Profiler.EndStartSection("entities");
                    renderglobal.RenderEntities(entityliving.GetPosition(par1), frustrum, par1);
                    EnableLightmap(par1);
                    Profiler.EndStartSection("litParticles");
                    effectrenderer.Func_1187_b(entityliving, par1);
                    RenderHelper.DisableStandardItemLighting();
                    SetupFog(0, par1);
                    Profiler.EndStartSection("particles");
                    effectrenderer.RenderParticles(entityliving, par1);
                    DisableLightmap(par1);

                    if (mc.ObjectMouseOver != null && entityliving.IsInsideOfMaterial(Material.Water) && (entityliving is EntityPlayer) && !mc.GameSettings.HideGUI)
                    {
                        EntityPlayer entityplayer = (EntityPlayer)entityliving;
                        //GL.Disable(EnableCap.AlphaTest);
                        Profiler.EndStartSection("outline");
                        renderglobal.DrawBlockBreaking(entityplayer, mc.ObjectMouseOver, 0, entityplayer.Inventory.GetCurrentItem(), par1);
                        renderglobal.DrawSelectionBox(entityplayer, mc.ObjectMouseOver, 0, entityplayer.Inventory.GetCurrentItem(), par1);
                        //GL.Enable(EnableCap.AlphaTest);
                    }
                }

                //GL.Disable(EnableCap.Blend);
                //GL.Enable(EnableCap.CullFace);
                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                //GL.DepthMask(true);
                SetupFog(0, par1);
                //GL.Enable(EnableCap.Blend);
                //GL.Disable(EnableCap.CullFace);
                //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/terrain.png"));

                if (mc.GameSettings.FancyGraphics)
                {
                    Profiler.EndStartSection("water");

                    if (mc.GameSettings.AmbientOcclusion)
                    {
                        //GL.ShadeModel(ShadingModel.Smooth);
                    }

                    //GL.ColorMask(false, false, false, false);
                    int i1 = renderglobal.SortAndRender(entityliving, 1, par1);

                    if (mc.GameSettings.Anaglyph)
                    {
                        if (AnaglyphField == 0)
                        {
                            //GL.ColorMask(false, true, true, true);
                        }
                        else
                        {
                            //GL.ColorMask(true, false, false, true);
                        }
                    }
                    else
                    {
                        //GL.ColorMask(true, true, true, true);
                    }

                    if (i1 > 0)
                    {
                        renderglobal.RenderAllRenderLists(1, par1);
                    }

                    //GL.ShadeModel(ShadingModel.Flat);
                }
                else
                {
                    Profiler.EndStartSection("water");
                    renderglobal.SortAndRender(entityliving, 1, par1);
                }

                //GL.DepthMask(true);
                //GL.Enable(EnableCap.CullFace);
                //GL.Disable(EnableCap.Blend);

                if (CameraZoom == 1.0D && (entityliving is EntityPlayer) && !mc.GameSettings.HideGUI && mc.ObjectMouseOver != null && !entityliving.IsInsideOfMaterial(Material.Water))
                {
                    EntityPlayer entityplayer1 = (EntityPlayer)entityliving;
                    //GL.Disable(EnableCap.AlphaTest);
                    Profiler.EndStartSection("outline");
                    renderglobal.DrawBlockBreaking(entityplayer1, mc.ObjectMouseOver, 0, entityplayer1.Inventory.GetCurrentItem(), par1);
                    renderglobal.DrawSelectionBox(entityplayer1, mc.ObjectMouseOver, 0, entityplayer1.Inventory.GetCurrentItem(), par1);
                    //GL.Enable(EnableCap.AlphaTest);
                }

                Profiler.EndStartSection("weather");
                RenderRainSnow(par1);
                //GL.Disable(EnableCap.Fog);

                if (pointedEntity == null);

                if (mc.GameSettings.ShouldRenderClouds())
                {
                    Profiler.EndStartSection("clouds");
                    //GL.PushMatrix();
                    SetupFog(0, par1);
                    //GL.Enable(EnableCap.Fog);
                    renderglobal.RenderClouds(par1);
                    //GL.Disable(EnableCap.Fog);
                    SetupFog(1, par1);
                    //GL.PopMatrix();
                }

                Profiler.EndStartSection("hand");

                if (CameraZoom == 1.0D)
                {
                    //GL.Clear(ClearBufferMask.ColorBufferBit);
                    RenderHand(par1, i);
                }

                if (!mc.GameSettings.Anaglyph)
                {
                    Profiler.EndSection();
                    return;
                }
            }

            //GL.ColorMask(true, true, true, false);
            Profiler.EndSection();
        }

        private void AddRainParticles()
        {
            float f = mc.TheWorld.GetRainStrength(1.0F);

            if (!mc.GameSettings.FancyGraphics)
            {
                f /= 2.0F;
            }

            if (f == 0.0F)
            {
                return;
            }

            Random.SetSeed(rendererUpdateCount * 0x12a7ce5f);
            EntityLiving entityliving = mc.RenderViewEntity;
            World world = mc.TheWorld;
            int i = MathHelper2.Floor_double(entityliving.PosX);
            int j = MathHelper2.Floor_double(entityliving.PosY);
            int k = MathHelper2.Floor_double(entityliving.PosZ);
            byte byte0 = 10;
            double d = 0.0F;
            double d1 = 0.0F;
            double d2 = 0.0F;
            int l = 0;
            int i1 = (int)(100F * f * f);

            if (mc.GameSettings.ParticleSetting == 1)
            {
                i1 >>= 1;
            }
            else if (mc.GameSettings.ParticleSetting == 2)
            {
                i1 = 0;
            }

            for (int j1 = 0; j1 < i1; j1++)
            {
                int k1 = (i + Random.Next(byte0)) - Random.Next(byte0);
                int l1 = (k + Random.Next(byte0)) - Random.Next(byte0);
                int i2 = world.GetPrecipitationHeight(k1, l1);
                int j2 = world.GetBlockId(k1, i2 - 1, l1);
                BiomeGenBase biomegenbase = world.GetBiomeGenForCoords(k1, l1);

                if (i2 > j + byte0 || i2 < j - byte0 || !biomegenbase.CanSpawnLightningBolt() || biomegenbase.GetFloatTemperature() <= 0.2F)
                {
                    continue;
                }

                float f1 = (float)Random.NextDouble();
                float f2 = (float)Random.NextDouble();

                if (j2 <= 0)
                {
                    continue;
                }

                if (Block.BlocksList[j2].BlockMaterial == Material.Lava)
                {
                    mc.EffectRenderer.AddEffect(new EntitySmokeFX(world, k1 + f1, (i2 + 0.1F) - Block.BlocksList[j2].MinY, l1 + f2, 0.0F, 0.0F, 0.0F));
                    continue;
                }

                if (Random.Next(++l) == 0)
                {
                    d = (float)k1 + f1;
                    d1 = (double)((float)i2 + 0.1F) - Block.BlocksList[j2].MinY;
                    d2 = (float)l1 + f2;
                }

                mc.EffectRenderer.AddEffect(new EntityRainFX(world, k1 + f1, (i2 + 0.1F) - Block.BlocksList[j2].MinY, l1 + f2));
            }

            if (l > 0 && Random.Next(3) < RainSoundCounter++)
            {
                RainSoundCounter = 0;

                if (d1 > entityliving.PosY + 1.0D && world.GetPrecipitationHeight(MathHelper2.Floor_double(entityliving.PosX), MathHelper2.Floor_double(entityliving.PosZ)) > MathHelper2.Floor_double(entityliving.PosY))
                {
                    mc.TheWorld.PlaySoundEffect(d, d1, d2, "ambient.weather.rain", 0.1F, 0.5F);
                }
                else
                {
                    mc.TheWorld.PlaySoundEffect(d, d1, d2, "ambient.weather.rain", 0.2F, 1.0F);
                }
            }
        }

        ///<summary>
        /// Render rain and snow
        ///</summary>
        protected void RenderRainSnow(float par1)
        {
            float f = mc.TheWorld.GetRainStrength(par1);

            if (f <= 0.0F)
            {
                return;
            }

            EnableLightmap(par1);

            if (RainXCoords == null)
            {
                RainXCoords = new float[1024];
                RainYCoords = new float[1024];

                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        float f1 = j - 16;
                        float f2 = i - 16;
                        float f3 = MathHelper2.Sqrt_float(f1 * f1 + f2 * f2);
                        RainXCoords[i << 5 | j] = -f2 / f3;
                        RainYCoords[i << 5 | j] = f1 / f3;
                    }
                }
            }

            EntityLiving entityliving = mc.RenderViewEntity;
            World world = mc.TheWorld;
            int k = MathHelper2.Floor_double(entityliving.PosX);
            int l = MathHelper2.Floor_double(entityliving.PosY);
            int i1 = MathHelper2.Floor_double(entityliving.PosZ);
            Tessellator tessellator = Tessellator.Instance;
            //GL.Disable(EnableCap.CullFace);
            //GL.Normal3(0.0F, 1.0F, 0.0F);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.01F);
            //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/environment/snow.png"));
            double d = entityliving.LastTickPosX + (entityliving.PosX - entityliving.LastTickPosX) * (double)par1;
            double d1 = entityliving.LastTickPosY + (entityliving.PosY - entityliving.LastTickPosY) * (double)par1;
            double d2 = entityliving.LastTickPosZ + (entityliving.PosZ - entityliving.LastTickPosZ) * (double)par1;
            int j1 = MathHelper2.Floor_double(d1);
            int k1 = 5;

            if (mc.GameSettings.FancyGraphics)
            {
                k1 = 10;
            }

            bool flag = false;
            sbyte byte0 = -1;
            float f4 = (float)rendererUpdateCount + par1;

            if (mc.GameSettings.FancyGraphics)
            {
                k1 = 10;
            }

            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            flag = false;

            for (int l1 = i1 - k1; l1 <= i1 + k1; l1++)
            {
                for (int i2 = k - k1; i2 <= k + k1; i2++)
                {
                    int j2 = ((l1 - i1) + 16) * 32 + ((i2 - k) + 16);
                    float f5 = RainXCoords[j2] * 0.5F;
                    float f6 = RainYCoords[j2] * 0.5F;
                    BiomeGenBase biomegenbase = world.GetBiomeGenForCoords(i2, l1);

                    if (!biomegenbase.CanSpawnLightningBolt() && !biomegenbase.GetEnableSnow())
                    {
                        continue;
                    }

                    int k2 = world.GetPrecipitationHeight(i2, l1);
                    int l2 = l - k1;
                    int i3 = l + k1;

                    if (l2 < k2)
                    {
                        l2 = k2;
                    }

                    if (i3 < k2)
                    {
                        i3 = k2;
                    }

                    float f7 = 1.0F;
                    int j3 = k2;

                    if (j3 < j1)
                    {
                        j3 = j1;
                    }

                    if (l2 == i3)
                    {
                        continue;
                    }

                    Random.SetSeed(i2 * i2 * 3121 + i2 * 0x2b24abb ^ l1 * l1 * 0x66397 + l1 * 13761);
                    float f8 = biomegenbase.GetFloatTemperature();

                    if (world.GetWorldChunkManager().GetTemperatureAtHeight(f8, k2) >= 0.15F)
                    {
                        if (byte0 != 0)
                        {
                            if (byte0 >= 0)
                            {
                                tessellator.Draw();
                            }

                            byte0 = 0;
                            //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/environment/rain.png"));
                            tessellator.StartDrawingQuads();
                        }

                        float f9 = (((float)(rendererUpdateCount + i2 * i2 * 3121 + i2 * 0x2b24abb + l1 * l1 * 0x66397 + l1 * 13761 & 0x1f) + par1) / 32F) * (3F + (float)Random.NextDouble());
                        double d3 = (double)((float)i2 + 0.5F) - entityliving.PosX;
                        double d4 = (double)((float)l1 + 0.5F) - entityliving.PosZ;
                        float f13 = MathHelper2.Sqrt_double(d3 * d3 + d4 * d4) / (float)k1;
                        float f14 = 1.0F;
                        tessellator.SetBrightness(world.GetLightBrightnessForSkyBlocks(i2, j3, l1, 0));
                        tessellator.SetColorRGBA_F(f14, f14, f14, ((1.0F - f13 * f13) * 0.5F + 0.5F) * f);
                        tessellator.SetTranslation(-d * 1.0D, -d1 * 1.0D, -d2 * 1.0D);
                        tessellator.AddVertexWithUV((double)((float)i2 - f5) + 0.5D, l2, (double)((float)l1 - f6) + 0.5D, 0.0F * f7, ((float)l2 * f7) / 4F + f9 * f7);
                        tessellator.AddVertexWithUV((double)((float)i2 + f5) + 0.5D, l2, (double)((float)l1 + f6) + 0.5D, 1.0F * f7, ((float)l2 * f7) / 4F + f9 * f7);
                        tessellator.AddVertexWithUV((double)((float)i2 + f5) + 0.5D, i3, (double)((float)l1 + f6) + 0.5D, 1.0F * f7, ((float)i3 * f7) / 4F + f9 * f7);
                        tessellator.AddVertexWithUV((double)((float)i2 - f5) + 0.5D, i3, (double)((float)l1 - f6) + 0.5D, 0.0F * f7, ((float)i3 * f7) / 4F + f9 * f7);
                        tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
                        continue;
                    }

                    if (byte0 != 1)
                    {
                        if (byte0 >= 0)
                        {
                            tessellator.Draw();
                        }

                        byte0 = 1;
                        //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/environment/snow.png"));
                        tessellator.StartDrawingQuads();
                    }

                    float f10 = ((float)(rendererUpdateCount & 0x1ff) + par1) / 512F;
                    float f11 = (float)Random.NextDouble() + f4 * 0.01F * (float)Random.NextGaussian();
                    float f12 = (float)Random.NextDouble() + f4 * (float)Random.NextGaussian() * 0.001F;
                    double d5 = (double)((float)i2 + 0.5F) - entityliving.PosX;
                    double d6 = (double)((float)l1 + 0.5F) - entityliving.PosZ;
                    float f15 = MathHelper2.Sqrt_double(d5 * d5 + d6 * d6) / (float)k1;
                    float f16 = 1.0F;
                    tessellator.SetBrightness((world.GetLightBrightnessForSkyBlocks(i2, j3, l1, 0) * 3 + 0xf000f0) / 4);
                    tessellator.SetColorRGBA_F(f16, f16, f16, ((1.0F - f15 * f15) * 0.3F + 0.5F) * f);
                    tessellator.SetTranslation(-d * 1.0D, -d1 * 1.0D, -d2 * 1.0D);
                    tessellator.AddVertexWithUV((double)((float)i2 - f5) + 0.5D, l2, (double)((float)l1 - f6) + 0.5D, 0.0F * f7 + f11, ((float)l2 * f7) / 4F + f10 * f7 + f12);
                    tessellator.AddVertexWithUV((double)((float)i2 + f5) + 0.5D, l2, (double)((float)l1 + f6) + 0.5D, 1.0F * f7 + f11, ((float)l2 * f7) / 4F + f10 * f7 + f12);
                    tessellator.AddVertexWithUV((double)((float)i2 + f5) + 0.5D, i3, (double)((float)l1 + f6) + 0.5D, 1.0F * f7 + f11, ((float)i3 * f7) / 4F + f10 * f7 + f12);
                    tessellator.AddVertexWithUV((double)((float)i2 - f5) + 0.5D, i3, (double)((float)l1 - f6) + 0.5D, 0.0F * f7 + f11, ((float)i3 * f7) / 4F + f10 * f7 + f12);
                    tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
                }
            }

            if (byte0 >= 0)
            {
                tessellator.Draw();
            }

            //GL.Enable(EnableCap.CullFace);
            //GL.Disable(EnableCap.Blend);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
            DisableLightmap(par1);
        }

        ///<summary>
        /// Setup orthogonal projection for rendering GUI screen overlays
        ///</summary>
        public void SetupOverlayRendering()
        {
            ScaledResolution scaledresolution = new ScaledResolution(mc.GameSettings, mc.DisplayWidth, mc.DisplayHeight);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0.0F, scaledresolution.ScaledWidthD, scaledresolution.ScaledHeightD, 0.0F, 1000D, 3000D);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Translate(0.0F, 0.0F, -2000F);
        }

        ///<summary>
        /// calculates fog and calls ClearColor
        ///</summary>
        private void UpdateFogColor(float par1)
        {
            World world = mc.TheWorld;
            EntityLiving entityliving = mc.RenderViewEntity;
            float f = 1.0F / (float)(4 - mc.GameSettings.RenderDistance);
            f = 1.0F - (float)Math.Pow(f, 0.25D);
            Vec3D vec3d = world.GetSkyColor(mc.RenderViewEntity, par1);
            float f1 = (float)vec3d.XCoord;
            float f2 = (float)vec3d.YCoord;
            float f3 = (float)vec3d.ZCoord;
            Vec3D vec3d1 = world.GetFogColor(par1);
            FogColorRed = (float)vec3d1.XCoord;
            FogColorGreen = (float)vec3d1.YCoord;
            FogColorBlue = (float)vec3d1.ZCoord;

            if (mc.GameSettings.RenderDistance < 2)
            {
                Vec3D vec3d2 = MathHelper2.Sin(world.GetCelestialAngleRadians(par1)) <= 0.0F ? Vec3D.CreateVector(1.0D, 0.0F, 0.0F) : Vec3D.CreateVector(-1D, 0.0F, 0.0F);
                float f5 = (float)entityliving.GetLook(par1).DotProduct(vec3d2);

                if (f5 < 0.0F)
                {
                    f5 = 0.0F;
                }

                if (f5 > 0.0F)
                {
                    float[] af = world.WorldProvider.CalcSunriseSunsetColors(world.GetCelestialAngle(par1), par1);

                    if (af != null)
                    {
                        f5 *= af[3];
                        FogColorRed = FogColorRed * (1.0F - f5) + af[0] * f5;
                        FogColorGreen = FogColorGreen * (1.0F - f5) + af[1] * f5;
                        FogColorBlue = FogColorBlue * (1.0F - f5) + af[2] * f5;
                    }
                }
            }

            FogColorRed += (f1 - FogColorRed) * f;
            FogColorGreen += (f2 - FogColorGreen) * f;
            FogColorBlue += (f3 - FogColorBlue) * f;
            float f4 = world.GetRainStrength(par1);

            if (f4 > 0.0F)
            {
                float f6 = 1.0F - f4 * 0.5F;
                float f8 = 1.0F - f4 * 0.4F;
                FogColorRed *= f6;
                FogColorGreen *= f6;
                FogColorBlue *= f8;
            }

            float f7 = world.GetWeightedThunderStrength(par1);

            if (f7 > 0.0F)
            {
                float f9 = 1.0F - f7 * 0.5F;
                FogColorRed *= f9;
                FogColorGreen *= f9;
                FogColorBlue *= f9;
            }

            int i = ActiveRenderInfo.GetBlockIdAtEntityViewpoint(mc.TheWorld, entityliving, par1);

            if (CloudFog)
            {
                Vec3D vec3d3 = world.DrawClouds(par1);
                FogColorRed = (float)vec3d3.XCoord;
                FogColorGreen = (float)vec3d3.YCoord;
                FogColorBlue = (float)vec3d3.ZCoord;
            }
            else if (i != 0 && Block.BlocksList[i].BlockMaterial == Material.Water)
            {
                FogColorRed = 0.02F;
                FogColorGreen = 0.02F;
                FogColorBlue = 0.2F;
            }
            else if (i != 0 && Block.BlocksList[i].BlockMaterial == Material.Lava)
            {
                FogColorRed = 0.6F;
                FogColorGreen = 0.1F;
                FogColorBlue = 0.0F;
            }

            float f10 = FogColor2 + (FogColor1 - FogColor2) * par1;
            FogColorRed *= f10;
            FogColorGreen *= f10;
            FogColorBlue *= f10;
            float d = (float)((entityliving.LastTickPosY + (entityliving.PosY - entityliving.LastTickPosY) * par1) * world.WorldProvider.GetVoidFogYFactor());

            if (entityliving.IsPotionActive(Potion.Blindness))
            {
                int j = entityliving.GetActivePotionEffect(Potion.Blindness).GetDuration();

                if (j < 20)
                {
                    d *= 1.0F - (float)j / 20F;
                }
                else
                {
                    d = 0.0F;
                }
            }

            if (d < 1.0D)
            {
                if (d < 0.0F)
                {
                    d = 0.0F;
                }

                d *= d;
                FogColorRed *= d;
                FogColorGreen *= d;
                FogColorBlue *= d;
            }

            if (mc.GameSettings.Anaglyph)
            {
                float f11 = (FogColorRed * 30F + FogColorGreen * 59F + FogColorBlue * 11F) / 100F;
                float f12 = (FogColorRed * 30F + FogColorGreen * 70F) / 100F;
                float f13 = (FogColorRed * 30F + FogColorBlue * 70F) / 100F;
                FogColorRed = f11;
                FogColorGreen = f12;
                FogColorBlue = f13;
            }

            //GL.ClearColor(FogColorRed, FogColorGreen, FogColorBlue, 0.0F);
        }

        ///<summary>
        /// Sets up the Fog to be rendered. If the arg passed in is -1 the Fog starts at 0 and goes to 80% of far plane
        /// distance and is used for sky rendering.
        ///</summary>
        private void SetupFog(int par1, float par2)
        {
            EntityLiving entityliving = mc.RenderViewEntity;
            bool flag = false;

            if (entityliving is EntityPlayer)
            {
                flag = ((EntityPlayer)entityliving).Capabilities.IsCreativeMode;
            }

            if (par1 == 999)
            {
                //GL.Fog(FogParameter.FogColor, new float[] {0.0F, 0.0F, 0.0F, 1.0F});
                //GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
                //GL.Fog(FogParameter.FogStart, 0.0F);
                //GL.Fog(FogParameter.FogEnd, 8F);
                /*
                if (GLContext.getCapabilities().GL_NV_Fog_distance)
                {
                    //GL.Fog(34138, 34139);
                }
                */
                //GL.Fog(FogParameter.FogStart, 0.0F);
                return;
            }

            //GL.Fog(FogParameter.FogColor, new float[] {FogColorRed, FogColorGreen, FogColorBlue, 1.0F});
            //GL.Normal3(0.0F, -1F, 0.0F);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            int i = ActiveRenderInfo.GetBlockIdAtEntityViewpoint(mc.TheWorld, entityliving, par2);

            if (entityliving.IsPotionActive(Potion.Blindness))
            {
                float f = 5F;
                int j = entityliving.GetActivePotionEffect(Potion.Blindness).GetDuration();

                if (j < 20)
                {
                    f = 5F + (farPlaneDistance - 5F) * (1.0F - (float)j / 20F);
                }

                //GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);

                if (par1 < 0)
                {
                    //GL.Fog(FogParameter.FogStart, 0.0F);
                    //GL.Fog(FogParameter.FogEnd, f * 0.8F);
                }
                else
                {
                    //GL.Fog(FogParameter.FogStart, f * 0.25F);
                    //GL.Fog(FogParameter.FogEnd, f);
                }
                /*
                if (GLContext.getCapabilities().GL_NV_Fog_distance)
                {
                    //GL.Fog(34138, 34139);
                }*/
            }
            else if (CloudFog)
            {
                //GL.Fog(FogParameter.FogMode, (int)FogMode.Exp);
                //GL.Fog(FogParameter.FogDensity, 0.1F);
                float f1 = 1.0F;
                float f5 = 1.0F;
                float f8 = 1.0F;

                if (mc.GameSettings.Anaglyph)
                {
                    float f11 = (f1 * 30F + f5 * 59F + f8 * 11F) / 100F;
                    float f15 = (f1 * 30F + f5 * 70F) / 100F;
                    float f18 = (f1 * 30F + f8 * 70F) / 100F;
                    f1 = f11;
                    f5 = f15;
                    f8 = f18;
                }
            }
            else if (i > 0 && Block.BlocksList[i].BlockMaterial == Material.Water)
            {
                //GL.Fog(FogParameter.FogMode, (int)FogMode.Exp);

                if (!entityliving.IsPotionActive(Potion.WaterBreathing))
                {
                    //GL.Fog(FogParameter.FogDensity, 0.1F);
                }
                else
                {
                    //GL.Fog(FogParameter.FogDensity, 0.05F);
                }

                float f2 = 0.4F;
                float f6 = 0.4F;
                float f9 = 0.9F;

                if (mc.GameSettings.Anaglyph)
                {
                    float f12 = (f2 * 30F + f6 * 59F + f9 * 11F) / 100F;
                    float f16 = (f2 * 30F + f6 * 70F) / 100F;
                    float f19 = (f2 * 30F + f9 * 70F) / 100F;
                    f2 = f12;
                    f6 = f16;
                    f9 = f19;
                }
            }
            else if (i > 0 && Block.BlocksList[i].BlockMaterial == Material.Lava)
            {
                //GL.Fog(FogParameter.FogMode, (int)FogMode.Exp);
                //GL.Fog(FogParameter.FogDensity, 2.0F);
                float f3 = 0.4F;
                float f7 = 0.3F;
                float f10 = 0.3F;

                if (mc.GameSettings.Anaglyph)
                {
                    float f13 = (f3 * 30F + f7 * 59F + f10 * 11F) / 100F;
                    float f17 = (f3 * 30F + f7 * 70F) / 100F;
                    float f20 = (f3 * 30F + f10 * 70F) / 100F;
                    f3 = f13;
                    f7 = f17;
                    f10 = f20;
                }
            }
            else
            {
                float f4 = farPlaneDistance;

                if (mc.TheWorld.WorldProvider.GetWorldHasNoSky() && !flag)
                {
                    double d = (double)((entityliving.GetBrightnessForRender(par2) & 0xf00000) >> 20) / 16D + (entityliving.LastTickPosY + (entityliving.PosY - entityliving.LastTickPosY) * (double)par2 + 4D) / 32D;

                    if (d < 1.0D)
                    {
                        if (d < 0.0F)
                        {
                            d = 0.0F;
                        }

                        d *= d;
                        float f14 = 100F * (float)d;

                        if (f14 < 5F)
                        {
                            f14 = 5F;
                        }

                        if (f4 > f14)
                        {
                            f4 = f14;
                        }
                    }
                }

                //GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);

                if (par1 < 0)
                {
                    //GL.Fog(FogParameter.FogStart, 0.0F);
                    //GL.Fog(FogParameter.FogEnd, f4 * 0.8F);
                }
                else
                {
                    //GL.Fog(FogParameter.FogStart, f4 * 0.25F);
                    //GL.Fog(FogParameter.FogEnd, f4);
                }
                /*
                if (GLContext.getCapabilities().GL_NV_Fog_distance)
                {
                    //GL.Fog(34138, 34139);
                }
                */
                if (mc.TheWorld.WorldProvider.Func_48218_b((int)entityliving.PosX, (int)entityliving.PosZ))
                {
                    //GL.Fog(FogParameter.FogStart, f4 * 0.05F);
                    //GL.Fog(FogParameter.FogEnd, Math.Min(f4, 192F) * 0.5F);
                }
            }

            //GL.Enable(EnableCap.ColorMaterial);
            //GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.Ambient);
        }

        ///<summary>
        /// Update and return fogColorBuffer with the RGBA values passed as arguments
        ///</summary>
        /*private FloatBuffer SetFogColorBuffer(float par1, float par2, float par3, float par4)
        {
            FogColorBuffer.clear();
            FogColorBuffer.put(par1).put(par2).put(par3).put(par4);
            FogColorBuffer.flip();
            return FogColorBuffer;
        }*/
    }
}