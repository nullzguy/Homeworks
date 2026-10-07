namespace Bank;

class CashSlot {
	public int Nominal;
	public int Count;
	public CashSlot(int nominal, int count) {
		Nominal = nominal;
		Count = count;
	}
}

class ATM {
	public const int MAX_NOTES = 30; // більше ніж 30 купюр за раз не видає
	private CashSlot[] slots;
	private BankCardManager manager;
	private Random random = new Random();

	public ATM(BankCardManager manager) {
		this.manager = manager;
		int[] nominals = { 1000, 500, 200, 100 };
		slots = new CashSlot[nominals.Length];
		for (int i = 0; i < nominals.Length; i++)
			slots[i] = new CashSlot(nominals[i], random.Next(0, 101)); // 0..100 купюр
	}

	public BankCard InsertCard(string cardNumber) => manager.GetBankCard(cardNumber);

	public void PrintStorage() {
		Console.WriteLine(" ATM has:");
		foreach (CashSlot slot in slots)
			Console.WriteLine($"  {slot.Nominal} uah x {slot.Count}");
	}

	public bool TryDispense(decimal amount, out int[] take) {
		take = new int[slots.Length];
		if (amount <= 0 || amount % 100 != 0) return false;
		if (!Find((int)amount, 0, MAX_NOTES, take)) {
			take = new int[slots.Length];
			return false;
		}
		for (int i = 0; i < slots.Length; i++) slots[i].Count -= take[i];
		return true;
	}

	private bool Find(int left, int index, int notesLeft, int[] take) {
		if (left == 0) return true;
		if (index >= slots.Length || notesLeft == 0) return false;
		int max = Math.Min(slots[index].Count, left / slots[index].Nominal);
		max = Math.Min(max, notesLeft);
		for (int n = max; n >= 0; n--) {
			take[index] = n;
			if (Find(left - n * slots[index].Nominal, index + 1, notesLeft - n, take)) return true;
		}
		take[index] = 0;
		return false;
	}

	public void PrintDispensed(int[] take) {
		for (int i = 0; i < slots.Length; i++)
			if (take[i] > 0) Console.WriteLine($"  {slots[i].Nominal} uah x {take[i]}");
	}
}
