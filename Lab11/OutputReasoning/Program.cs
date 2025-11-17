
// using System;
// delegate int Calc(int x, int y);
// class Program
// {
//     static int Add(int a, int b) { Console.Write("A"); return a + b; }
//     static int Mul(int a, int b) { Console.Write("M"); return a * b; }
//     static int Sub(int a, int b) { Console.Write("S"); return a - b; }
//     static void Main()
//     {
//         Calc c = Add;
//         c += Mul;
//         c += Sub;
//         c -= Add;
//         int res = c(2, 3);
//         Console.Write(":" + res);
//     }
// }

// using System;
// delegate void ActionHandler(ref int x);
// class Program
// {
// static void Inc(ref int a) { a += 2; Console.Write("I" + a + " "); }
// static void Dec(ref int a) { a--; Console.Write("D" + a + " "); }
// static void Main()
// {
// int val = 3;
// ActionHandler act = Inc;
// act += Dec;
// act(ref val);
// Console.Write("F" + val);
// }
// }

// using System;
// class LimitEventArgs : EventArgs
// {
// public int CurrentValue { get; }
// public LimitEventArgs(int val) => CurrentValue = val;
// }
// class Counter
// {
//     public event EventHandler<LimitEventArgs> LimitReached;
//     public event EventHandler<LimitEventArgs> MilestoneReached;
//     private int value = 0;
//     public void Increment()
//     {
//         value++;
//         Console.Write(">" + value);
//         // Fire Milestone event every 2nd increment
//         if (value % 2 == 0)
//             MilestoneReached?.Invoke(this, new LimitEventArgs(value));
//         // Fire Limit event every 3rd increment
//         if (value % 3 == 0)
//             LimitReached?.Invoke(this, new LimitEventArgs(value));
//     }
// }


// class Program
// {
// static void Main()
// {
// Counter c = new Counter();
// // Subscribers for LimitReached
// c.LimitReached += (s, e) => Console.Write("[L" + e.CurrentValue + "]");
// c.LimitReached += (s, e) => Console.Write("(Reset)");
// // Subscribers for MilestoneReached
// c.MilestoneReached += (s, e) =>
// {
// Console.Write("[M" + e.CurrentValue + "]");
// if (e.CurrentValue == 4)
// Console.Write("{Alert}");
// };
// for (int i = 0; i < 6; i++)
// c.Increment();
// }
// }

// using System;
// class TemperatureEventArgs : EventArgs
// {
// public int OldTemperature { get; }
// public int NewTemperature { get; }
// public TemperatureEventArgs(int oldTemp, int newTemp)
// {
// OldTemperature = oldTemp;
// NewTemperature = newTemp;
// }
// }
// class TemperatureSensor
// {
// public event EventHandler<TemperatureEventArgs> TemperatureChanged;
// private int temperature = 25;
// public void UpdateTemperature(int newTemp)
// {
// int oldTemp = temperature;
//         temperature = newTemp;
// if (Math.Abs(newTemp - oldTemp) > 5)
// {
// TemperatureChanged?.Invoke(this, new TemperatureEventArgs(oldTemp, newTemp));
// }
// }
// }
// class Program
// {
// static void Main()
// {
// TemperatureSensor sensor = new TemperatureSensor();
// sensor.TemperatureChanged += (s, e) =>
// Console.WriteLine($"Temperature changed from {e.OldTemperature}°C to {e.NewTemperature}°C");
// sensor.TemperatureChanged += (s, e) =>
// {
// if (Math.Abs(e.NewTemperature - e.OldTemperature) > 10)
// Console.WriteLine(" Warning: Sudden change detected!");
// };
// sensor.UpdateTemperature(28);
// sensor.UpdateTemperature(30);
// sensor.UpdateTemperature(46);
// sensor.UpdateTemperature(52);
// }
// }

// using System;
// class NotifyEventArgs : EventArgs
// {
// public string Message { get; }
// public NotifyEventArgs(string msg) => Message = msg;
// }
// class Notifier
// {
// public event EventHandler<NotifyEventArgs> OnNotify;
// public void Trigger(string msg)
// {
// Console.Write("[Start]");
// OnNotify?.Invoke(this, new NotifyEventArgs(msg));
// Console.Write("[End]");
// }
// }
// class Program
// {
// static void Main()
// {
// Notifier n = new Notifier();
// n.OnNotify += (s, e) =>
// {
// Console.Write("{" + e.Message + "}");
// };
// n.OnNotify += (s, e) =>
// {
// Console.Write("(Nested)");
// if (e.Message == "Ping")
// ((Notifier)s).Trigger("Pong");
// };
// n.Trigger("Ping");
// }
// }


using System;
class AlertEventArgs : EventArgs
{
public string Info { get; }
public AlertEventArgs(string info) => Info = info;
}
class Sensor
{
public event EventHandler<AlertEventArgs> ThresholdReached;
public void Check(int value)
{
Console.Write("[Check]");
if (value > 50)
ThresholdReached?.Invoke(this, new AlertEventArgs("High"));
Console.Write("[Done]");
}
}
class Program
{
static void Main()
{
Sensor s = new Sensor();
s.ThresholdReached += (sender, e) =>
{
Console.Write("{" + e.Info + "}");
if (e.Info == "High")
((Sensor)sender).Check(30);
};
s.ThresholdReached += (sender, e) =>
Console.Write("(Alert)");
s.Check(80);
}
}