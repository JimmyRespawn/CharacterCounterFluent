﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterCounterWinUI3.Helpers
{
    public class GeneralHelper
    {
        public static Windows.UI.Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Windows.UI.Color.FromArgb(a, r, g, b);
        }

        public static async Task<bool> Rate(string StoreProduectID)
        {
            try
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=" + StoreProduectID));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}