using System;
using System.Collections.Generic;
//Edit_13-Mar-26
// edit 13mar2026 MS
// 1. [S]INGLE RESPONSIBILITY: This class only handles logging.
public class Logger
{
    public void Log(string message) => Console.WriteLine($"[LOG]: {message}");
}

// 2. [I]NTERFACE SEGREGATION: Smaller, specific interfaces are better than one "fat" interface.
public interface IPaymentProcessor
{
    void ProcessPayment(double amount);
}

// 3. ABSTRACTION & ENCAPSULATION: 
// We use an abstract class to hide internal details and provide a base.
public abstract class PaymentBase : IPaymentProcessor
{
    // ENCAPSULATION: Private field, protected logic.
    private readonly Logger _logger = new Logger();

    public abstract void ProcessPayment(double amount);

    protected void LogTransaction(string type, double amount)
    {
        _logger.Log($"Processing {type} for ${amount}");
    }
}

// 4. INHERITANCE: CreditCardPayment "is-a" PaymentBase.
// 5. [O]PEN/CLOSED: We can add new payment types (like Crypto) without changing existing code.
public class CreditCardPayment : PaymentBase
{
    public override void ProcessPayment(double amount)
    {
        LogTransaction("Credit Card", amount);
        // Logic for Credit Card...
    }
}

public class PayPalPayment : PaymentBase
{
    public override void ProcessPayment(double amount)
    {
        LogTransaction("PayPal", amount);
        // Logic for PayPal...
    }
}

// 6. [D]EPENDENCY INVERSION: The Manager depends on the Interface (IPaymentProcessor), 
// not the concrete classes (CreditCardPayment).
public class PaymentManager
{
    private readonly IPaymentProcessor _processor;

    public PaymentManager(IPaymentProcessor processor)
    {
        _processor = processor;
    }

    public void Execute(double amount)
    {
        _processor.ProcessPayment(amount);
    }
}

// 7. [L]ISKOV SUBSTITUTION & POLYMORPHISM: 
// We can swap any IPaymentProcessor without breaking the program.
class Program
{
    static void Main()
    {
        List<IPaymentProcessor> payments = new List<IPaymentProcessor> {
            new CreditCardPayment(),
            new PayPalPayment()
        };

        foreach (var payment in payments)
        {
            PaymentManager manager = new PaymentManager(payment);
            manager.Execute(100.00);
        }
    }
}