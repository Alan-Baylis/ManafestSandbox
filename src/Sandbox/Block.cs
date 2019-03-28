/*
    Blocks are the base unit of terrain, and exist in such numbers that an object for each
    would be insanely expensive.
*/
using System;

public class Block {
    public enum Blocks{
        Air,
        Dirt,
        Grass,
        Stone,
        Wood,
        WoodPlanks,
        IronOre,
        CoalOre,
        CraftingTable,
        Furnace
    };

    public static IFancyBlock LoadFancyBlock(string json){
        return null;
    }

    public static string BlockModel(Blocks block){
        switch(block){
            case Blocks.Air: return ""; break;
        }

        return "Assets/Models/Blocks/" + block + ".obj";
    }

    public static string BlockTexture(Blocks block){
        return "Assets/Textures/" + block + ".png";
    }
}