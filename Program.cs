using System;
using System.Timers;

class Car
{

    public string type;
    public int maxFuelCapacity;
    public int fuelInTank;
    public int fuellingTime;
    public int waitingTime;
    public bool isFuelling;
    public Car()
    {
        String[] model = { "Car", "Van", "HGV" }; //array of car types available
        int[] fuelSizes = { 50, 80, 150 };        //array of fuel capacities available

        Random rnd = new Random();
        int newType = rnd.Next(3); //creates a random value between 0 and 3 (not including 3), will be used to create a car type and ensure it can have correct fuel capacity
        type = model[newType];
        maxFuelCapacity = fuelSizes[newType];
        fuelInTank = rnd.Next(5, maxFuelCapacity / 2); //the fuel in their tank which has a maximum of less than half of the max fuel capacity
        int temp = Convert.ToInt32(Math.Floor((maxFuelCapacity - fuelInTank) / 1.5)); //ensures that does not round above the max fuel capacity and makes it an integer from double
        fuellingTime = rnd.Next(0, temp); //the length of time it takes to fuel is found by working out the time it would take to fully fuel and using that as the upper parameter in a Random.Next() method
        waitingTime = rnd.Next(20, 80);
        isFuelling = false;
    }
    public static System.Timers.Timer carCreator = new(1500);
    public static void Main(string[] args)
    {
        //carCreator.Elapsed += XXXXXXXX;
        carCreator.Enabled = true;
        carCreator.AutoReset = true;
        carCreator.Start();
        Console.ReadKey();

    }
}