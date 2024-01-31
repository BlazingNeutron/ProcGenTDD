# ProcGenTDD

[![.NET](https://github.com/BlazingNeutron/ProcGenTDD/actions/workflows/dotnet.yml/badge.svg)](https://github.com/BlazingNeutron/ProcGenTDD/actions/workflows/dotnet.yml)

A simple C# library that generates random 2D string arrays representing a 2D side scrolling platformer level.

Restrictions:

- start and end floor sections are permenant and count as floor sections
- player has the ability to walk left and right across adjacent floor sections
- player has the ability to jump
  - 1 unit up
  - 2 unit horizontally
  - 1 unit diagonally (up & left or up & right)
- player can survive falls from any height
- player can move left or right while falling
