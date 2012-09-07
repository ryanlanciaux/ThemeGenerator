// Type: ColoRAMA.Extensions.ColorMath
// Assembly: ColoRAMA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Projects\.NET\ThemeGenerator\ThemeGenerator\bin\ColoRAMA.dll

using System;
using System.Drawing;

namespace ColoRAMA.Extensions
{
    public static class ColorMath
    {
        public static Color GetComplement(this Color c)
        {
            return ColorMath.ChangeHue(c, 180);
        }

        public static Color GetLeftTriad(this Color c)
        {
            return ColorMath.ChangeHue(c, 120);
        }

        public static Color GetRightTriad(this Color c)
        {
            return ColorMath.ChangeHue(c, -120);
        }

        public static Color GetLeftSplitComplement(this Color c)
        {
            return ColorMath.ChangeHue(c, 150);
        }

        public static Color GetRightSplitComplement(this Color c)
        {
            return ColorMath.ChangeHue(c, -150);
        }

        public static Color GetLeftAnalogous(this Color c)
        {
            return ColorMath.ChangeHue(c, 30);
        }

        public static Color GetRightAnalogous(this Color c)
        {
            return ColorMath.ChangeHue(c, -30);
        }

        public static Color ChangeHue(this Color c, int degrees)
        {
            float h = (float)degrees + c.GetHue();
            if ((double)h > 360.0)
                h -= 360f;
            if ((double)h < 0.0)
                h += 360f;
            return ColorMath.ColorFromAhsb((int)c.A, h, c.GetSaturation(), c.GetBrightness());
        }

        public static Color ChangeSaturation(this Color c, int amount)
        {
            float s = ColorMath.SetToMaxOrMin((float)amount * 0.01f + c.GetSaturation());
            return ColorMath.ColorFromAhsb((int)c.A, c.GetHue(), s, c.GetBrightness());
        }

        private static float SetToMaxOrMin(float s)
        {
            if ((double)s > 1.0)
                s = 1f;
            if ((double)s < 0.0)
                s = 0.0f;
            return s;
        }

        public static Color ChangeBrightness(this Color c, int amount)
        {
            float b = ColorMath.SetToMaxOrMin((float)amount * 0.01f + c.GetBrightness());
            return ColorMath.ColorFromAhsb((int)c.A, c.GetHue(), c.GetSaturation(), b);
        }

        public static string ToHexString(this Color c)
        {
            string str = "00000000" + ColorTranslator.ToWin32(c).ToString("X");
            return str.Substring(str.Length - 8, 8);
        }

        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {
            if (0 > a || (int)byte.MaxValue < a)
                throw new ArgumentOutOfRangeException("a", (object)a, "Invalid Alpha");
            if (0.0 > (double)h || 360.0 < (double)h)
                throw new ArgumentOutOfRangeException("h", (object)h, "Invalid Hue");
            if (0.0 > (double)s || 1.0 < (double)s)
                throw new ArgumentOutOfRangeException("s", (object)s, "Invalid Saturation");
            if (0.0 > (double)b || 1.0 < (double)b)
                throw new ArgumentOutOfRangeException("b", (object)b, "Invalid Brightness");
            if (0.0 == (double)s)
                return Color.FromArgb(a, Convert.ToInt32(b * (float)byte.MaxValue), Convert.ToInt32(b * (float)byte.MaxValue), Convert.ToInt32(b * (float)byte.MaxValue));
            float num1;
            float num2;
            if (0.5 < (double)b)
            {
                num1 = b - b * s + s;
                num2 = b + b * s - s;
            }
            else
            {
                num1 = b + b * s;
                num2 = b - b * s;
            }
            int num3 = (int)Math.Floor((double)h / 60.0);
            if (300.0 <= (double)h)
                h -= 360f;
            h /= 60f;
            h -= 2f * (float)Math.Floor(((double)num3 + 1.0) % 6.0 / 2.0);
            float num4 = 0 != num3 % 2 ? num2 - h * (num1 - num2) : h * (num1 - num2) + num2;
            int num5 = Convert.ToInt32(num1 * (float)byte.MaxValue);
            int num6 = Convert.ToInt32(num4 * (float)byte.MaxValue);
            int num7 = Convert.ToInt32(num2 * (float)byte.MaxValue);
            switch (num3)
            {
                case 1:
                    return Color.FromArgb(a, num6, num5, num7);
                case 2:
                    return Color.FromArgb(a, num7, num5, num6);
                case 3:
                    return Color.FromArgb(a, num7, num6, num5);
                case 4:
                    return Color.FromArgb(a, num6, num7, num5);
                case 5:
                    return Color.FromArgb(a, num5, num7, num6);
                default:
                    return Color.FromArgb(a, num5, num6, num7);
            }
        }
    }
}
