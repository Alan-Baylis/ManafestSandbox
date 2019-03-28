public interface IFancyBlock {
    int GetId();
    string ToJson();
    void Update(int tick);
}