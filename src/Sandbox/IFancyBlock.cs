public interface IFancyBlock {
    int GetId();
    string ToJson();
    void Update(BlockWorld world, int tick);
}