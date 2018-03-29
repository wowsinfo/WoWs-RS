# WRInfo Console Edition
~~Logo coming soon~~

中文介绍 日本語 (soon)

Open source real time player info for [World of Warships](http://worldofwarships.com). This program is written in C# and works for Windows 7+ (probably)

## How does it work
This program get information from python.log and detects update every 20 seconds. If there is a new battle, it will check for all players id and get their ship statistics. [Personal Rating](https://wows-numbers.com/personal/rating) is calculated to show how skilled a player is. **Ability Point** is then calculated to show how experienced a player is

 ***Why is this important?***

 Imagine this scenario. 2000 battles AVERAGE Yamato vs 20 battles GREAT Yamato. Who will win? Well, it is hard to tell. Every battles means something (AFK does not count) and a player could learn from battles. Therefore, WRInfo uses Ability Point and Persona Rating together to show how good a player could be

*This program is possiable thanks to wargaming api and wows number* 

## Features
- Realtime statistic
- Support all game modes
- Lightweight (size < 2m, ram < 40m)
- Command line
- Auto update

## Run this project
- Replace you application id inside Value.cs
If you do not have one, request one [here](https://developers.wargaming.net/)

- Click run inside Visual Studio

## Support me
Simply share with your friends so that this program could help more players or [Donate](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=LTVLXBS7K85XA&lc=AU&item_name=Donations&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)

Thank you for your support >_<
