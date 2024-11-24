public interface IPickable {
    Bonus CurrentBonus { get; }
    void Apply(Bonus bonus);
}
