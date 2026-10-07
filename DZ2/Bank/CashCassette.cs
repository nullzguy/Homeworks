namespace Bank;

class CashCassette
{
    public int Denomination { get; }
    public int Count { get; set; }

    public CashCassette(int denomination, int count)
    {
        Denomination = denomination;
        Count = count;
    }
}
