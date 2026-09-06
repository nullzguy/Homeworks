// Я буду використовувати це як темлату для наступних дз, мені вона сподобалося.

// using System;
using System.Text;

namespace DZ1 { class Program {
// Не хочеться табити постійно, так гарніше, та менше місця займа.
// До речі, стиль брекетів {} Allman мені не подомається, тож буде K&R

// - - - = = = &   З Н А Ч Е Н Н Я ,   Ф У Н К Ц І Ї ,   Т А   І Н Ш Е   & = = = - - -

public struct Report {
	public readonly string Text;
	public readonly int Severity;

	public Report(string text, int severity) {
	    this.Text = text;
	    this.Severity = severity;
	}
	// SEVERITY:
	// 0 - Добре, 1 - Нейтрально, 2 - Попередження, 3 - Критично
	// Лінь enum створювати
	// Ну хоча можна було б define створити.. ладно
};
public static Report[] Reports = {
	new Report("Input cannot be empty!", 3),
	new Report("Failed to parse input!", 3),
	new Report("Unknown homework id!", 3),
	new Report("Number cannot be negative!", 3),
	new Report("Keep going!", 1),
	new Report("Just a bit more!", 1),
	new Report("Almost there!", 1),
	new Report("You did it, good job!", 0),
	new Report("You overwalked!", 0),
	new Report("You a robot by any chance?", 0),
	new Report("You shouldn't really see this, something's not ok", 2),
};
public static void ShowReport(int id) {
	// Хмм, в С# ПаскальКейс для назв функцій? Якщо чесно, виглядає гарно))
	int sev = Reports[id].Severity;
	string message = Reports[id].Text;
	string sevWarning = 
	sev==0 ? "\e[38;2;50;255;70m" :
	sev==2 ? "\e[38;2;255;175;0m WARNING: " : 
	sev==3 ? "\e[38;2;255;20;20m   -- ERROR --\n" : "";
 	Console.WriteLine($"{sevWarning} {message}\e[0m");
	// Коротко, '\e' або '\x1B' або '\u001B' (в С та C++ - це просто '\033') - це юнікод ESCAPE.
	// Штука це НЕЙМОВІРНО корисна, адже дозволює управляти курсором, кольорами, та іншим.
	// Приклади: "\e[38;2;<r>;<g>;<b>m" міняє колір тексту, а "\e[48;2;<r>;<g>;<b>m" - колір фону тексту
	// "\e[H" - повертає курсор наверх, "\e[0m" - RESET (ресет кольору), та дуже дуже багато іншого.
	if (sev==3) Environment.Exit(1);
}

public static void SafeReadLineToInt(out int num) {
	// Безпечність залізобетонна))
	num = -1;
	string? input = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(input)) ShowReport(0);
	else if (!int.TryParse(input, out num)) ShowReport(1);
}
public static void SafeReadLineToFloat(out float num) {
	// Безпечність залізобетонна))
	num = -1;
	string? input = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(input)) ShowReport(0);
	else if (!float.TryParse(input, out num)) ShowReport(1);
}
public static void SafeReadLineToBool(out bool state) {
	// Тут теж
	state = false;
	string? input = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(input)) { ShowReport(0); return; }
	switch (input[0]) {
	   case 'y': case 'Y': state = true; break;
	   case 'n': case 'N': state = false; break;
	   default: ShowReport(1); break;
	}
}

public static Action[] Homeworks = {
	Hw1, Hw2, Hw3
};

// - - - = = = &   Д О М А Ш К И   & = = = - - -

public static void Hw1() {
	// Напишіть невеликий скрипт для фітнес-трекера:
	
	Console.WriteLine(" What's your steps target?");
	SafeReadLineToInt(out int targetSteps);
	if (targetSteps <= 0) ShowReport(3);
	Console.WriteLine(" How many steps have you walked?");
	SafeReadLineToInt(out int walkedSteps);
	if (walkedSteps <= 0) ShowReport(3);
	
	float completeness = walkedSteps/(float)targetSteps;
	ShowReport(
		(completeness<0.7f) ? 4 :
		(completeness>=0.7f && completeness<0.9f) ? 5 :
		(completeness>=0.9f && completeness<1.0f) ? 6 :
		(completeness>=1.0f && completeness<1.1f) ? 7 :
		(completeness>=1.1f && completeness<2.0f) ? 8 :
		(completeness>=2.0f) ? 9 : 10
	);
}

public static void Hw2() {
	// Напишіть програму яка буде імітувати касу в магазині електроніки:
	
	Console.WriteLine(" What's your products cost?");
	SafeReadLineToFloat(out float cost);
	if (cost <= 0) ShowReport(3);
	Console.WriteLine(" Do you have a bonus card for a discount? (yY/nN)");
	SafeReadLineToBool(out bool bonus);
	float cashBackPercent = (cost >= 2e3f && cost < 1e4f) ? 0.01f :
	(cost >= 1e4f) ? 0.05f : 0.0f;
	float bonusPercent = bonus ? (cost >= 2e4f) ? 0.05f : 0.03f : 0.0f;
	float totalCost = cost*(1.0f-cashBackPercent-bonusPercent);
	Console.WriteLine(
		$" Total cost: { cost } uah - { cashBackPercent*cost } uah cashBack - { bonusPercent*cost } uah bonus =\n" +
		$" { totalCost } uah"
	);
}

public static void Hw3() {
	Console.WriteLine(" What's your total energy usage in kwH?");
	SafeReadLineToFloat(out float kwH);
	if (kwH <= 0) ShowReport(3);
	float totalCost = (kwH < 100) ? kwH * 1.44f :
	(kwH>=100 && kwH < 600) ? kwH * 1.68f :
	(kwH>=600) ? kwH * 1.92f : -1;
	if ((int)totalCost==-1) ShowReport(10);
	Console.WriteLine($" Your total electricity bill cost is {totalCost} uah");
}

// - - - = = = &   П Р О Г Р А М А   & = = = - - -

static void Main() {
	// На С# дуже не привично, але є корисні функції))
	// Але треба все заново вчити та заучувати
	// Я буду по англ писати все, бо не зручно перемикатися дуже
	
	Console.InputEncoding = Encoding.UTF8;
	Console.OutputEncoding = Encoding.UTF8;
	
	Console.WriteLine($"Choose homework id (1-{Homeworks.Length}):");
	SafeReadLineToInt(out int hwIdx);
	if (hwIdx < 1 || hwIdx > Homeworks.Length) ShowReport(2);
	else { Console.Clear(); Homeworks[hwIdx - 1](); }
	
	// До речі, якщо щось не так - я закінчую програму)))
	// Лінь постійно в стек закидувати адрес інструкції для продовження заново
	// Та й взагалі, так безпечніше.
}

}}