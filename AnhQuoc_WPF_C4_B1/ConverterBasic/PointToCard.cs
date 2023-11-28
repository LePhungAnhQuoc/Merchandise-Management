using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    class PointToCard
    {
        #region Constants
        private const int maxGold = 10000;
        private const int minGold = 5000;
        private const int maxPlatinum = 10000;
        #endregion

        public static CardType ToCard(double point)
        {
            CardType result = CardType.None;

            if (point > minGold && point < maxGold)
                result = CardType.Gold;
            else if (point >= maxPlatinum)
                result = CardType.Platinum;

            return result;
        }
    }
}
