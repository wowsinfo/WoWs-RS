# WRInfo Console Edition
<img src="https://raw.githubusercontent.com/HenryQuan/WRInfo-Console-Edition/master/Logo/logos/WRInfo-Logo%400.5x.png" width="200px" height="200px" />

# This is not working anymore (ask wargaming for the reason)
**I might remake this but proabbaly not**

中文介绍 日本語 (soon)

Open source real time player info for [World of Warships](http://worldofwarships.com). This program is written in C# and works for Windows 7+ (probably) and does not work for Mac because of colorful console framework

**Please respect all players and do not judge others base on their stats. If you could not agree this, this program is not for you. Please DO NOT download it**

## How does it work
This program get information from python.log and detects update every 20 seconds. If there is a new battle, it will check for all players id and get their ship statistics. [Personal Rating](https://wows-numbers.com/personal/rating) is calculated to show how skilled a player is. **Ability Point** is then calculated to show how experienced a player is

 ***Why is this important?***

 Imagine this scenario. 2000 battles AVERAGE Yamato vs 20 battles GREAT Yamato. Who will win? Well, it is hard to tell. Every battles means something (AFK does not count) and a player could learn from battles. Therefore, WRInfo uses Ability Point and Persona Rating together to show how good a player could be

*This program is possiable thanks to wargaming api and wows number* 

## Features
- Realtime statistic
- Support all game modes and servers
- Lightweight (size < 2m, ram < 40m)
- Command line
- No installation
- Auto update

This program should work for all future versions unless there are changes to the game structure or wargaming api. Updates are only for bug fixing and algorithmn updates

## Run this project
- Replace you application id inside Value.cs
If you do not have one, request one [here](https://developers.wargaming.net/)

- Click run inside Visual Studio

## Support me
Simply share with your friends so that this program could help more players

BTC: 1QC2jwyuEFCxXBH68e8vGeTjp6ce3T4r1q

Thank you for your support >_<
