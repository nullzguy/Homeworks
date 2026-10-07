namespace Bank;
class BankCard {
	private const int MAX_ATTEMPTS = 3; // скільки разів можна промахнутися піном
	private decimal balance = 0;
	private string cardNumber = "";
	private string ownerName = "";
	private string pinCode = "";
	private int failedPinAttempts = 0;
	private bool isBlocked = false;

	public BankCard(string cardNumber, string pinCode, decimal balance, string ownerName) {
		this.cardNumber = cardNumber;
		this.pinCode = pinCode;
		this.balance = balance;
		this.ownerName = ownerName;
	}

	public string GetCardNumber() => cardNumber;
	public string GetOwnerName() => ownerName;
	public bool IsBlocked() => isBlocked;

	public bool CheckPin(string pin) {
		if (isBlocked) throw new Exception("Card is blocked.");
		if (pin == pinCode) return true;
		failedPinAttempts++;
		if (failedPinAttempts >= MAX_ATTEMPTS) {
			isBlocked = true;
			throw new Exception("Too many failed attempts, card is blocked!");
		}
		return false;
	}

	public decimal GetBalance(string pin) {
		if (!CheckPin(pin)) throw new Exception("Invalid PIN.");
		return balance;
	}

	public void Withdraw(string pin, decimal amount) {
		if (!CheckPin(pin)) throw new Exception("Invalid PIN.");
		if (amount > balance) throw new Exception("Not enough money on the card.");
		balance -= amount;
	}
}
