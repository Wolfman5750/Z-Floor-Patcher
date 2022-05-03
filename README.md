Z-Floor Patcher

This patcher is based on the xEdit scripts located found on the Nexus here:
https://www.nexusmods.com/skyrimspecialedition/mods/66652

RobertGK and Kojack found that there are potential issues when objects are located outside of the bounds of the worldspace. Moving objects horizontally will place objects in neighboring cells, but items or navmeshes with a z-coordinate < -30,000 or > 30,000 can cause issues with the game engine and cause stuttering. This patcher cycles through all placed objects and all navmeshes. If it encounters anything with a z-coordinate less than -30,000, then it is placed at exactly -30,000. If it encounters anything with a z-coordinate > 30,000, then it is placed at exactly 30,000.
