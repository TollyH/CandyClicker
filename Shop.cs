/*
 * Candy Clicker ver. 1.0.0
 * Copyright © 2021  Ptolemy Hill
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
            new ShopItem("Auto Clicker", 0, 1, 100),
            new ShopItem("Candy Machine", 0, 5, 250),
            new ShopItem("Larger Candies", 1, 0, 500),
            new ShopItem("Grandma", 2, 10, 1000),
            new ShopItem("Candy Shop", 3, 15, 2500),
            new ShopItem("Candy Factory", 5, 20, 5000),
            new ShopItem("Candy Army", 10, 50, 50000),
            new ShopItem("King Candy", 15, 60, 100000),
            new ShopItem("Candy Nuke", 25, 100, 250000),
            new ShopItem("Planet Candy", 50, 500, 500000),
            new ShopItem("Candy Cosmos", 100, 750, 1000000),
            new ShopItem("CandyTime Continuum", 250, 1000, 2000000),
            new ShopItem("Candyverse Portal", 500, 5000, 5000000),
            new ShopItem("Infinite Candy Theory", 1000, 10000, 10000000),
            new ShopItem("Unobtainium Candy", 2500, 25000, 15000000),
        };
    }
}
