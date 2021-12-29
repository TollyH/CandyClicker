# Candy Clicker

A simple candy clicking game written in C# and WPF, created initially to serve as a small idle game to play during class. Requires the .NET 6 Desktop Runtime to play and Visual Studio 2022 with .NET Desktop Development Tools installed to build.

## Instructions

*These instructions also appear in-game with added images*  
The aim of the game is simple: get as much candy as you possibly can! Clicking the large candy in middle will increment your total candies, shown at the very top. Initially, each click will be worth one candy, but this number will soon rise as you buy items from the shop. The number of candies a single click will give you is shown to the left with the abbreviation */c* and the number of times you are clicking per second is shown in the centre with the abbreviation *CPS*. After buying items from the shop, you will also begin to gain candies every second without the need to click. The number of candies you are receiving per second is show to the right with the abbreviation */s*.

### The Shop

The shop is located at the bottom of the window, listing of all the items that can be bought. The number to the right of each item is its price in candies, which will be red if you can afford the item, or grey if you cannot. To buy an item, simply click on it in the shop, and the cost in candies will immediately be removed from your total balance in return for an increase in your candies per second and candies per click. To see how much a particular item will increase your stats by, hover over it with the mouse pointer; the increase in stats, along with the number of times you have bought the item previously, will be displayed. Every time you buy an item from the shop, its price will increase by a quarter of its original price, so make sure to buy a wide range of items!

### Rare Candies

At random intervals, a golden candy may fall across the screen. If you manage to click it before it's gone, you will be rewarded with a single burst of 500x your current candies per click value. As you click the large candy, the grey bar underneath your stats will gradually begin to fill up. After 1,000 clicks, the bar will be completely full. Once this happens, you will have 5 seconds to click the (now golden) candy as much as possible. During this period, each click will be worth 10x what a click would normally be worth, so be sure to get as many clicks in as you can.  
*Note that it will always take 1,000 clicks to fill the meter, regardless of your candies per click value. During the 5 second period, the candy rain in the background will turn golden as well as the large candy; keep in mind that these golden candies cannot be clicked as if they were a rare candy.*

### Reincarnation

Reincarnation allows you to start the game anew with all your stats and the shop reset. In return, you will be granted a multiplier on your candies per second in your new game. You can reincarnate more than once, although each reincarnation will be harder to attain than the last, and you cannot reincarnate in a way that would result in you having a smaller multiplier than you currently have. For more information, click the reincarnation button in the top-left.

### Overflow Stars

After a long time of playing, multiple reincarnations, and reaping the item shop for all it's worth, your candies will likely be growing at an immense pace. You may feel like you've achieved everything possible, but there is still one last trophy for you to add to your collection. Overflow stars. If you manage to make your total candies exceed what the game can handle (a number which is 20 digits long, or over 10 quintillion), then you will be awarded a golden star (★) just above the item shop. Your candies will wrap around back to a lower number, and you can feel a sense of immense pride in knowing that you've gained so much candy that not even a computer can keep count anymore. A new star will be given every single time you manage to reach the limit. While they're not worth anything in the shop, if you've managed to reach this point, buying items probably isn't your highest priority anyway.

### Save Data

Your progress will automatically be saved every 10 seconds, as well as whenever you close the game. If you wish to completely restart the game at any point, or if you wish to transfer your save data to another system, first close the game, then delete/move the `save_data.dat` file. If you are moving it, keep in mind that it must be located in the same folder as `CandyClicker.exe` to be detected.  
*Further information about the structure of the save file can be found in `SAVEFILE.md`*.

## Credits and Attributions

Programming and Design: Tolly Hill  
Feature Testing: Fin Griffiths; Tom Reynolds; Riley Beer  
Initial Inspiration and Candy Picture: Oscar O'Brien  
Font: mikachan

**Copyright © 2021  Ptolemy Hill**
