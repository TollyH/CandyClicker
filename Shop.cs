/*
 * Candy Clicker ver. 1.1.1
 * Copyright © 2021-2022  Ptolemy Hill
 */
namespace CandyClicker
{
    public static class Shop
    {
        public class ShopItem
        {
            public string Name { get; private set; }
            public ulong AdditionalPerClick { get; private set; }
            public ulong AdditionalPerSecond { get; private set; }
            public ulong Price { get; private set; }

            public ShopItem(string name, ulong additionalPerClick, ulong additionalPerSecond, ulong price)
            {
                Name = name;
                AdditionalPerClick = additionalPerClick;
                AdditionalPerSecond = additionalPerSecond;
                Price = price;
            }
        }

        public static readonly ShopItem[] CandyShop =
        {
            new ShopItem("Unobtainium Candy", 2500, 25000, 1),
        };
    }
}
