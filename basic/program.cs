using System;
using System.Collections.Generic;

public delegate decimal BillingStrategy(decimal baseCost);

public class PatientEventArgs : EventArgs
{
    public string PatientName { get; set; }
    public decimal BillAmount { get; set; }
}

public abstract class Patient
{
    public string Name { get; set; }
    public string PatientId { get; set; }
    protected decimal baseCost;
    
    public Patient(string name) 
    { 
        Name = name; 
        PatientId = Guid.NewGuid().ToString().Substring(0, 6);
    }
    
    public abstract decimal GetBaseCost();
}

public class GeneralPatient : Patient
{
    public GeneralPatient(string name) : base(name) { baseCost = 1000m; }
    public override decimal GetBaseCost() => baseCost;
}

public class EmergencyPatient : Patient
{
    public EmergencyPatient(string name) : base(name) { baseCost = 5000m; }
    public override decimal GetBaseCost() => baseCost;
}

public class ICUPatient : Patient
{
    public int Days { get; set; }
    public ICUPatient(string name, int days) : base(name) { baseCost = 10000m; Days = days; }
    public override decimal GetBaseCost() => baseCost * Days;
}

public static class BillingStrategies
{
    public static decimal Standard(decimal cost) => cost;
    public static decimal Insurance(decimal cost) => cost * 0.7m;
    public static decimal Senior(decimal cost) => cost * 0.8m;
}

public class BillingSystem
{
    public event EventHandler<PatientEventArgs> OnBillGenerated;
    public event EventHandler<PatientEventArgs> OnPaymentReceived;
    
    public decimal CalculateBill(Patient patient, BillingStrategy strategy)
    {
        decimal bill = strategy(patient.GetBaseCost());
        OnBillGenerated?.Invoke(this, new PatientEventArgs { PatientName = patient.Name, BillAmount = bill });
        return bill;
    }
    
    public void ProcessPayment(Patient patient, decimal amount)
    {
        OnPaymentReceived?.Invoke(this, new PatientEventArgs { PatientName = patient.Name, BillAmount = amount });
    }
}

public class NotificationService
{
    public void Subscribe(BillingSystem billing)
    {
        billing.OnBillGenerated += (s, e) => Console.WriteLine($"[ACCOUNTING] Bill: ${e.BillAmount:F2} for {e.PatientName}");
        billing.OnBillGenerated += (s, e) => Console.WriteLine($"[PHARMACY] Prepare meds for {e.PatientName}");
        billing.OnPaymentReceived += (s, e) => Console.WriteLine($"[FINANCE] Payment: ${e.BillAmount:F2} from {e.PatientName}");
    }
}

public class Hospital
{
    private List<Patient> patients = new List<Patient>();
    private BillingSystem billing = new BillingSystem();
    
    public Hospital()
    {
        new NotificationService().Subscribe(billing);
    }
    
    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n1.Admit 2.View 3.Bill 4.Exit");
            Console.Write("Choice: ");
            
            switch (Console.ReadLine())
            {
                case "1": Admit(); break;
                case "2": View(); break;
                case "3": Bill(); break;
                case "4": return;
            }
        }
    }
    
    void Admit()
    {
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Type (1.General 2.Emergency 3.ICU): ");
        
        Patient p = Console.ReadLine() switch
        {
            "2" => new EmergencyPatient(name),
            "3" => new ICUPatient(name, GetInt("Days: ")),
            _ => new GeneralPatient(name)
        };
        
        patients.Add(p);
        Console.WriteLine($"Admitted! ID: {p.PatientId}");
    }
    
    void View()
    {
        for (int i = 0; i < patients.Count; i++)
            Console.WriteLine($"{i + 1}. {patients[i].Name} (ID: {patients[i].PatientId})");
    }
    
    void Bill()
    {
        if (patients.Count == 0) return;
        
        View();
        int idx = GetInt("Select: ") - 1;
        if (idx < 0 || idx >= patients.Count) return;
        
        Patient p = patients[idx];
        Console.Write("Billing (1.Standard 2.Insurance 3.Senior): ");
        
        BillingStrategy strategy = Console.ReadLine() switch
        {
            "2" => BillingStrategies.Insurance,
            "3" => BillingStrategies.Senior,
            _ => BillingStrategies.Standard
        };
        
        decimal bill = billing.CalculateBill(p, strategy);
        Console.WriteLine($"\nPatient: {p.Name}\nBase: ${p.GetBaseCost():F2}\nFinal: ${bill:F2}");
        
        Console.Write("Pay now? (y/n): ");
        if (Console.ReadLine() == "y") billing.ProcessPayment(p, bill);
    }
    
    int GetInt(string prompt)
    {
        Console.Write(prompt);
        return int.Parse(Console.ReadLine());
    }
}

class Program
{
    static void Main() => new Hospital().Run();
}