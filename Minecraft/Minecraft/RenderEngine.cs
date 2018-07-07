using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
    public enum TextureMode { Clamp, Blur, BlurClamp }

    public class RenderEngine
    {
        public static RenderEngine Instance;

        Minecraft mc;

        public GraphicsDevice GraphicsDevice;

        public SpriteBatch SpriteBatch;

        BasicEffect basicEffect;

        Dictionary<string, Texture2D> textureMap;

        /// <summary>
        /// A mapping from image URLs to ThreadDownloadImageData instances </summary>
        Dictionary<string, ThreadedImageDownloader> urlToImageDataMap;

        List<string> allocatedTextureNames;

        bool isDrawing;

        Texture2D boundTexture;

        Texture2D missingTexture;

        Matrix ViewMatrix;
        Matrix ProjectionMatrix;

        public RenderEngine(Minecraft mc)
        {
            this.mc = mc;
            GraphicsDevice = mc.GraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.TextureEnabled = true;
            basicEffect.LightingEnabled = false;

            textureMap = new Dictionary<string, Texture2D>();
            allocatedTextureNames = new List<string>();

            missingTexture = new Texture2D(GraphicsDevice, 64, 64);
        }

        

        
        public bool LoadResourceTexture(string name)
        {
            try
            {
            	//Image test = Image.FromStream(Minecraft.GetResourceStream(name));
            	//Texture2D texture = new Texture2D(GraphicsDevice, Minecraft.GetResourceStream(name));
            	var texture = Texture2D.FromStream(GraphicsDevice, Minecraft.GetResourceStream(name));
                textureMap.Add(name, texture);
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to load texture from resource: /'" + name + "/'");
                Console.WriteLine();

                Utilities.LogException(e);

                return false;
            }

            return true;
        }

        public Texture2D GetTexture(string name)
        {
            if (textureMap.ContainsKey(name))
                return textureMap[name];
            else
            {
                if (LoadResourceTexture(name))
                    return textureMap[name];
            }

            return missingTexture;
        }

        public Texture2D GenerateNewTexture(int width, int height)
        {
            return new Texture2D(GraphicsDevice, width, height);
        }

        public string AllocateTexture(Texture2D texture)
        {
            if (allocatedTextureNames.Count == 0)
            {
                var name = "texture_0";
                allocatedTextureNames.Add(name);
                textureMap.Add(name, texture);
                return name;
            }
            else
            {
                var number = int.Parse(allocatedTextureNames[allocatedTextureNames.Count - 1].Split('_')[1]);
                var name = "texture_" + ++number;
                allocatedTextureNames.Add(name);
                textureMap.Add(name, texture);
                return name;
            }
        }

        public void DeleteTexture(string textureName)
        {
            textureMap.Remove(textureName);

            if (allocatedTextureNames.Contains(textureName))
                allocatedTextureNames.Remove(textureName);
        }

        public void StartDrawing()
        {
            StartDrawing(Microsoft.Xna.Framework.Color.Black);
        }

        public void StartDrawing(Microsoft.Xna.Framework.Color clearColor)
        {
            isDrawing = true;

            GraphicsDevice.Clear(clearColor);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
        }

        public void StopDrawing()
        {
            SpriteBatch.End();

            isDrawing = false;
        }

        public void BindTexture(string textureName)
        {
            if (!textureMap.ContainsKey(textureName))
            {
                if (LoadResourceTexture(textureName))
                {
                    boundTexture = textureMap[textureName];
                }
                else boundTexture = missingTexture;
            }
            else
            {
                boundTexture = textureMap[textureName];
            }
        }

        public void BindTexture(string texture, TextureMode mode)
        {
            BindTexture(texture);
        }

        public void RenderSprite(string textureName, Microsoft.Xna.Framework.Rectangle destination)
        {
            if (!textureMap.ContainsKey(textureName))
            {
                if (!LoadResourceTexture(textureName))
                {
                    return;
                }
            }

            if (!isDrawing)
            {
                Console.WriteLine("Warning. Not currently drawing. Call StartDrawing() to render sprites!");
                Console.WriteLine();
                return;
            }

            SpriteBatch.Draw(textureMap[textureName], destination, Microsoft.Xna.Framework.Color.White);
        }

        public void RenderSprite(Microsoft.Xna.Framework.Rectangle destination, RectangleF source)
        {
            RenderSprite(destination, source, 0);
        }

        public void RenderSprite(Microsoft.Xna.Framework.Rectangle destination, RectangleF source, float depth)
        {
            RenderSprite(destination, GetDefiniteRectangle(boundTexture, source), depth);
        }

        public void RenderSprite(Microsoft.Xna.Framework.Rectangle destination, Microsoft.Xna.Framework.Rectangle? source)
        {
            RenderSprite(destination, source, 0);
        }

        public void RenderSprite(Microsoft.Xna.Framework.Rectangle destination, Microsoft.Xna.Framework.Rectangle? source, float depth)
        {
            if (!isDrawing)
            {
                Console.WriteLine("Warning. Not currently drawing. Call StartDrawing() to render sprites!");
                Console.WriteLine();
                return;
            }

            if (boundTexture == null)
            {
                Console.WriteLine("No texture specified. Bind a texture or specify a texture name.");
                Console.WriteLine();
                return;
            }

            SpriteBatch.Draw(boundTexture, GetResizedRectangle(destination), source, Microsoft.Xna.Framework.Color.White);
        }

        public Vector2 GetDisplayScaler()
        {
            var res = GetMCRes();
            float x = res.GetScaledWidth();
            float y = res.GetScaledHeight();
            var diffX = mc.DisplayWidth / x;
            var diffY = mc.DisplayHeight / y;

            return new Vector2(diffX, diffY);
        }

        public Microsoft.Xna.Framework.Rectangle GetResizedRectangle(Microsoft.Xna.Framework.Rectangle r)
        {
            var scaler = GetDisplayScaler();
            return new RectangleF(r.X * scaler.X, r.Y * scaler.Y, r.Width * scaler.X, r.Height * scaler.Y).ToRectangle();
        }

        public ScaledResolution GetMCRes()
        {
            return new ScaledResolution(mc.GameSettings, mc.DisplayWidth, mc.DisplayHeight);
        }

        public Microsoft.Xna.Framework.Rectangle GetDefiniteRectangle(Texture2D texture, RectangleF rectangleF)
        {
            return new Microsoft.Xna.Framework.Rectangle((int)(texture.Width * rectangleF.X), (int)(texture.Height * rectangleF.Y), (int)(texture.Width * rectangleF.Width), (int)(texture.Height * rectangleF.Height));
        }

        public void SetView(Matrix matrix)
        {
            ViewMatrix = matrix;
        }

        public void MulView(Matrix matrix)
        {
            ViewMatrix *= matrix;
        }

        public void SetProjection(Matrix matrix)
        {
            ProjectionMatrix = matrix;
        }

        public void MulProjection(Matrix matrix)
        {
            ProjectionMatrix *= matrix;
        }

        public void DrawIndexed(VertexPositionTexture[] verts, int[] indices)
        {
            basicEffect.World = Matrix.Identity;
            basicEffect.View = ViewMatrix;
            basicEffect.Projection = ProjectionMatrix;
            basicEffect.Texture = boundTexture;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, verts.Length, indices, 0, indices.Length / 3);
            }
        }

        /// <summary>
        /// Takes a URL of a downloadable image and the name of the local image to be used as a fallback.  If the image has
        /// been downloaded, returns the GL texture of the downloaded image, otherwise returns the GL texture of the fallback
        /// image.
        /// </summary>
        public string GetTextureForDownloadableImage(string par1Str, string par2Str)
        {
            ThreadedImageDownloader threaddownloadimagedata = urlToImageDataMap[par1Str];
            
			if (threaddownloadimagedata != null && threaddownloadimagedata.Image != null && !threaddownloadimagedata.TextureSetupComplete)
			{
				if (threaddownloadimagedata.TextureName == "")
				{
					threaddownloadimagedata.TextureName = AllocateTexture(threaddownloadimagedata.Image);
				}
				else
				{
					//SetupTexture(threaddownloadimagedata.Image, threaddownloadimagedata.TextureName);
				}

				threaddownloadimagedata.TextureSetupComplete = true;
			}

			if (threaddownloadimagedata == null || threaddownloadimagedata.TextureName == "")
			{
				if (par2Str == null)
				{
					return "";
				}
				else
				{
					return par2Str;
				}
			}
			else
			{
				return threaddownloadimagedata.TextureName;
			}
        }

        /// <summary>
        /// Return a ThreadDownloadImageData instance for the given URL. If it does not already exist, it is created and
        /// uses the passed ImageBuffer. If it does, its reference count is incremented.
        /// </summary>
        public ThreadedImageDownloader ObtainImageData(string par1Str, ImageBuffer par2ImageBuffer)
        {
            ThreadedImageDownloader threaddownloadimagedata = null;

            if (urlToImageDataMap.ContainsKey(par1Str))
                threaddownloadimagedata = urlToImageDataMap[par1Str];

            if (threaddownloadimagedata == null)
            {
                urlToImageDataMap.Add(par1Str, new ThreadedImageDownloader(par1Str, par2ImageBuffer));
            }
            else
            {
                threaddownloadimagedata.ReferenceCount++;
            }

            return threaddownloadimagedata;
        }

        /// <summary>
        /// Decrements the reference count for a given URL, deleting the image data if the reference count hits 0
        /// </summary>
        public void ReleaseImageData(string par1Str)
        {
            ThreadedImageDownloader threaddownloadimagedata = null;

            if (urlToImageDataMap.ContainsKey(par1Str))
                threaddownloadimagedata = urlToImageDataMap[par1Str];

            if (threaddownloadimagedata != null)
            {
                threaddownloadimagedata.ReferenceCount--;

                if (threaddownloadimagedata.ReferenceCount == 0)
                {
                    if (threaddownloadimagedata.TextureName == "")
                    {
                        DeleteTexture(threaddownloadimagedata.TextureName);
                    }

                    urlToImageDataMap.Remove(par1Str);
                }
            }
        }
    }
}
