using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public static class PlayerId
    {
        public static string GetPlayerLetter(sbyte pnum)
        {
            if (pnum < 10)
            {
                return pnum.ToString();
            }
            else if (pnum <= 36)
            {
                char letter = 'a';
                letter += (char)(pnum - 10);
                return letter.ToString();
            }
            else
            {
                return "<Out of range player id: " + pnum + ">";
            }
        }
    }
}
