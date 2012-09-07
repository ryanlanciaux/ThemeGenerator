using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Models;
using ColoRAMA.Extensions;
using ThemeGenerator;

namespace ColoRAMA_Web.Models.Factories
{
    public static class ColorFactory
    {

        
        public static ThemeData GetHtml(ThemeData theme)
        {
            theme.Main = ToHtml(theme.Main);
            theme.Foreground = ToHtml(theme.Foreground);
            theme.Background = ToHtml(theme.Background);
            theme.Identifier = ToHtml(theme.Identifier);
            theme.Keyword = ToHtml(theme.Keyword);
            theme.StringColor = ToHtml(theme.StringColor);
            theme.User = ToHtml(theme.User);
            theme.User2 = ToHtml(theme.User2);
            theme.Comment = ToHtml(theme.Comment);
            theme.Error = ToHtml(theme.Error);
            theme.Operator = ToHtml(theme.Operator);
            theme.MarginColor = ToHtml(theme.MarginColor);
            theme.Preprocessor = ToHtml(theme.Preprocessor);
            theme.NumberColor = ToHtml(theme.NumberColor);
            return theme;
        }

        private static string ToHtml(string themeData)
        {
            return "#" + themeData.Substring(6, 2) + themeData.Substring(4, 2) + themeData.Substring(2, 2);
        }

        public static ThemeData GetThemeDataBrightHtml(string forecolor, string backcolor, string maincolor, string contrast)
        {
            return GetHtml(GetThemeDataBright(forecolor, backcolor, maincolor, contrast));
        }
        public static ThemeData GetThemeDataBright(string forecolor, string backcolor, string maincolor, string contrastString)
        {

            Regex rex = new Regex("^#?([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$");
            if (!rex.IsMatch(forecolor) || !rex.IsMatch(backcolor) || !rex.IsMatch(maincolor))
            {
                throw new InvalidColorException();
            }

            double dblContrast;
            if ((!double.TryParse(contrastString, out dblContrast)) || (dblContrast > 180) || (dblContrast < 0))
            {
                throw new InvalidContrastException();
            }

            var theme = new ThemeData();
            int contrast = (int)double.Parse(contrastString);

            Color main = ColorTranslator.FromHtml(maincolor);
            Color fore = ColorTranslator.FromHtml(forecolor);
            Color back = ColorTranslator.FromHtml(backcolor);

            int shadeMultiplyer = back.R + back.G + back.B > 382.5 ? -1 : 1;
            
            if (contrast == 0) { contrast = 1; }           

            theme.Main = main.ToHexString();
            theme.Foreground = fore.ToHexString();
            theme.Background = back.ToHexString();
            theme.Identifier = fore.ToHexString();
            theme.Keyword = theme.Main;

            theme.StringColor = main.ChangeHue((int)(contrast / double.Parse(ConfigurationManager.AppSettings["string"])))       
                .ToHexString();

            theme.NumberColor = main.ChangeHue((int) (contrast/double.Parse(ConfigurationManager.AppSettings["number"])))
                .ToHexString();

            theme.User = main.ChangeHue((int)(contrast))

                         .ToHexString();

            theme.User2 = main.ChangeHue((int)(contrast))
                          .ChangeSaturation(-1 * shadeMultiplyer * 30)
                          .ToHexString();

            theme.Comment = fore.ChangeBrightness(30 * shadeMultiplyer * -1 )
                            .ToHexString();

            theme.Error = ColorTranslator.FromHtml("#F00000")
                .ToHexString();

            theme.Operator = fore                
                .ToHexString();

            theme.MarginColor = back.ToHexString();

            theme.Preprocessor = main
                .ChangeHue((int)(contrast / double.Parse(ConfigurationManager.AppSettings["preprocessor"])))
                .ToHexString();
            return theme;
        }
    }
}