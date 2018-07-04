using System;
using System.Linq;

namespace net.minecraft.src
{
    public class Options
    {
        public static Options MUSIC = new Options("options.music", true, false);

        public static Options SOUND = new Options("options.sound", true, false);

        public static Options INVERT_MOUSE = new Options("options.invertMouse", false, true);

        public static Options SENSITIVITY = new Options("options.sensitivity", true, false);

        public static Options FOV = new Options("options.fov", true, false);

        public static Options GAMMA = new Options("options.gamma", true, false);

        public static Options RENDER_DISTANCE = new Options("options.renderDistance", false, false);

        public static Options VIEW_BOBBING = new Options("options.viewBobbing", false, true);

        public static Options ANAGLYPH = new Options("options.anaglyph", false, true);

        public static Options ADVANCED_OPENGL = new Options("options.advancedOpengl", false, true);

        public static Options FRAMERATE_LIMIT = new Options("options.framerateLimit", false, false);

        public static Options DIFFICULTY = new Options("options.difficulty", false, false);

        public static Options GRAPHICS = new Options("options.graphics", false, false);

        public static Options AMBIENT_OCCLUSION = new Options("options.ao", false, true);

        public static Options GUI_SCALE = new Options("options.guiScale", false, false);

        public static Options RENDER_CLOUDS = new Options("options.renderClouds", false, true);

        public static Options PARTICLES = new Options("options.particles", false, false);

        public bool Float;

        public bool Bool;

        public string String;

        static Options[] optionsArray = new Options[] { 
            Options.MUSIC, Options.SOUND, Options.INVERT_MOUSE, 
            Options.SENSITIVITY, Options.FOV, Options.GAMMA, 
            Options.RENDER_DISTANCE, Options.VIEW_BOBBING, 
            Options.ANAGLYPH, Options.ADVANCED_OPENGL, Options.FRAMERATE_LIMIT, 
            Options.DIFFICULTY, Options.GRAPHICS, Options.AMBIENT_OCCLUSION, 
            Options.GUI_SCALE, Options.RENDER_CLOUDS, Options.PARTICLES };

        private Options(string par3Str, bool par4, bool par5)
        {
            String = par3Str;
            Float = par4;
            Bool = par5;
        }

        public int Ordinal()
        {
            return Array.IndexOf(optionsArray, this);
        }

        public static Options GetOptions(int par0)
        {
            return optionsArray[par0];
        }
    }
}