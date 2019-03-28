/*
    A manager of one discrete set of blocks to be loaded
*/
using System;

public class Chunk {
    public const int Depth = 64;
    public const int Width = 64;
    public const int Height = 256;
    public int[] blockTypes;
    public int[] fancyBlockReferences;
}