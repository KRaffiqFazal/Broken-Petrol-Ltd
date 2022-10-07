using System;
using System.Timers;

class Car
{

    public string type;
    public int maxFuelCapacity;
    public int fuelInTank;
    public int fuellingTime;
    public bool isFuelling;
    public System.Timers.Timer waitingTime;
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
        waitingTime = new(rnd.Next(9000, 10000)); //waiting time is now a timer that is unique to each object and so will tick down until it triggers the elapsed event function
        waitingTime.Elapsed += WaitingTime_Elapsed;
        waitingTime.AutoReset = false;
        waitingTime.Enabled = true;
        isFuelling = false;
    }

    public static Car[] existingCars = { }; //cars that exist in the petrol station, can be waiting or fuelling.
    public static Car newCar = new Car(); //variable for new car to be appended into above array
    public static Car[] leavingCars = { }; //array that will contain cars who have finished waiting
    public static int carCounter = 0;

    public static Random intervalTime = new Random();
    public static System.Timers.Timer carCreator = new(intervalTime.Next(1500, 2201));

    public static bool pump1Free = true;

    public static bool pump2Free = true;
    
    public static bool pump3Free = true;
    
    public static bool pump4Free = true;
    
    public static bool pump5Free = true;
    
    public static bool pump6Free = true;
    
    public static bool pump7Free = true;
    
    public static bool pump8Free = true;
    
    public static bool pump9Free = true;

    public static void Main(string[] args)
    {
        carCreator.Elapsed += CarCreator_Elapsed;
        carCreator.Enabled = true;
        carCreator.AutoReset = true;
        carCreator.Start();
        Console.ReadKey();

    }
    private static void CarCreator_Elapsed(object sender, ElapsedEventArgs e) //creates vehicles
    {
        existingCars = existingCars.Append(newCar).ToArray();
        Console.WriteLine(existingCars[carCounter].type + " " + existingCars[carCounter].fuelInTank + " / " + existingCars[carCounter].maxFuelCapacity);
        carCounter++;
        newCar = new Car();
        newCar.waitingTime.Start();

    }
    private void WaitingTime_Elapsed(object? sender, ElapsedEventArgs e) //changes the condition on the cars to know that they have finished waiting
    {
        int i = 0;
        foreach (Car car in existingCars)
        {
            if (car.waitingTime.Enabled == false)
            {
                leavingCars = leavingCars.Append(car).ToArray();
                Console.WriteLine("A vehicle has left " + car.type);
                Console.WriteLine(leavingCars[i].type);
                i++;
            }
        }
    }

}