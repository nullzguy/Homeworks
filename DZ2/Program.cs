// using System;
using System.Text;
using Bank;

namespace DZ2 { class Program {

// - - - = = = &   З Н А Ч Е Н Н Я ,   Ф У Н К Ц І Ї   & = = = - - -

static BankCardManager Manager = new BankCardManager();
static ATM Atm = new ATM(Manager);
static string pin = "";

static void Report(string text, int severity) {
	string prefix =
	severity == 0 ? "\e[38;2;50;255;70m" :
	severity == 2 ? "\e[38;2;255;175;0m WARNING: " :
	severity == 3 ? "\e[38;2;255;20;20m   -- ERROR --\n" : "";
	Console.WriteLine($"{prefix} {text}\e[0m");
}

static string ReadLineSafe() => Console.ReadLine() ?? "";

static void SafeReadLineToInt(out int num) {
	num = -1;
	string input = ReadLineSafe();
	if (string.IsNullOrWhiteSpace(input)) Report("Input cannot be empty!", 3);
	else if (!int.TryParse(input, out num)) Report("Failed to parse input!", 3);
}

// - - - = = = &   Б А Н К О М А Т   & = = = - - -

static BankCard? InsertCard() {
	while (true) {
		Console.Write(" Insert card (card number): ");
		string number = ReadLineSafe();
		if (number.Length == 0) return null;
		try { return Atm.InsertCard(number); }
		catch (Exception e) { Report(e.Message, 3); }
	}
}

static bool AskPin(BankCard card) {
	while (true) {
		Console.Write(" PIN: ");
		string input = ReadLineSafe();
		try {
			if (card.CheckPin(input)) { pin = input; return true; }
			Report("Wrong PIN, try again.", 2);
		} catch (Exception e) {
			Report(e.Message, 3);
			return false;
		}
	}
}

static void ShowBalance(BankCard card) {
	try { Console.WriteLine($" Balance: {card.GetBalance(pin)} uah"); }
	catch (Exception e) { Report(e.Message, 3); }
}

static void Withdraw(BankCard card) {
	Console.Write(" Amount to withdraw: ");
	SafeReadLineToInt(out int amount);
	if (amount <= 0) { Report("Amount must be positive!", 3); return; }
	if (amount % 100 != 0) { Report("Amount must be a multiple of 100!", 3); return; }
	try { if (amount > card.GetBalance(pin)) { Report("Not enough money on the card!", 3); return; }
	} catch (Exception e) { Report(e.Message, 3); return; }
	if (!Atm.TryDispense(amount, out int[] take)) { Report("ATM can't give this sum!", 3); return; }
	card.Withdraw(pin, amount);
	Report("Take your cash:", 0);
	Atm.PrintDispensed(take);
}

static void Session(BankCard card) {
	if (!AskPin(card)) return;
	while (true) {
		Console.WriteLine("\n Choose operation:");
		Console.WriteLine("  1) Balance");
		Console.WriteLine("  2) Withdraw cash");
		Console.WriteLine("  3) Eject card");
		SafeReadLineToInt(out int choice);
		if (choice == 3) { Report("Card returned, have a Totally Perfect Day!", 0); return; }
		else if (choice == 1) ShowBalance(card);
		else if (choice == 2) Withdraw(card);
		else { Report("Unknown operation!", 2); continue; }
		if (!AskPin(card)) return;
	}
}

// - - - = = = &   П Р О Г Р А М А   & = = = - - -

static void Main() {
	Console.InputEncoding = Encoding.UTF8;
	Console.OutputEncoding = Encoding.UTF8;

	while (true) {
		Console.WriteLine("\n=== ATM ===");
		Atm.PrintStorage();
		BankCard? card = InsertCard();
		if (card == null) break;
		if (card.IsBlocked()) { Report("This card is blocked!", 3); continue; }
		Session(card);
	}
}

}}
