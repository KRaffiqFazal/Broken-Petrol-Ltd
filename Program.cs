using System;
using System.Timers;


namespace Broken_Petrol_Ltd
{     
    class Car
    {
        public string type;
        public int maxFuelCapacity;
        public int fuelInTank;
        public int fuellingTime;
        public bool isFuelling;
        public System.Timers.Timer waitingTime;
        public bool hasWaited;
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
            waitingTime = new(rnd.Next(1000, 2000)); //waiting time is now a timer that is unique to each object and so will tick down until it triggers the elapsed event function
            waitingTime.Elapsed += WaitingTime_Elapsed;
            waitingTime.AutoReset = false;
            waitingTime.Enabled = true;
            isFuelling = false;
            hasWaited = false;
        }
        public static Car[] existingCars = { }; //cars that exist in the petrol station, can be waiting or fuelling.
        public static Car newCar = new Car(); //variable for new car to be appended into above array
        public static Car[] leavingCars = { }; //array that will contain cars who have finished waiting
        public static int carCounter = 0;

        public static Pump pump1 = new Pump(1);
        public static Pump pump2 = new Pump(2);
        public static Pump pump3 = new Pump(3);
        public static Pump pump4 = new Pump(4);
        public static Pump pump5 = new Pump(5);
        public static Pump pump6 = new Pump(6);
        public static Pump pump7 = new Pump(7);
        public static Pump pump8 = new Pump(8);
        public static Pump pump9 = new Pump(9);


        public static Random intervalTime = new Random();
        public static System.Timers.Timer carCreator = new(intervalTime.Next(1500, 2201));

        public static void Main(string[] args)
        {
            carCreator.Elapsed += CarCreator_Elapsed;
            carCreator.Enabled = true;
            carCreator.AutoReset = true;
            carCreator.Start();
            Console.ReadKey();
            carCreator.Stop();
            Console.WriteLine("All the vehicles that have left are: ");
            foreach (Car car in leavingCars)
            {
                Console.WriteLine(car.type);
            }

        }
        private static void CarCreator_Elapsed(object sender, ElapsedEventArgs e) //creates vehicles
        {
            int i = 0;
            foreach (Car car in existingCars)
            {
                if (car.hasWaited == true)
                {
                    i++;
                }
            }
            if (existingCars.Length - i <= 7)
            {
                existingCars = existingCars.Append(newCar).ToArray();
                Console.WriteLine(existingCars[carCounter].type + " " + existingCars[carCounter].fuelInTank + " / " + existingCars[carCounter].maxFuelCapacity);
                carCounter++;
                newCar = new Car();
                newCar.waitingTime.Start();
            }

        }
        private static void WaitingTime_Elapsed(object? sender, ElapsedEventArgs e) //changes the condition on the cars to know that they have finished waiting
        {
            int i = 0;
            foreach (Car car in existingCars)
            {
                if (car.waitingTime.Enabled == false && !car.hasWaited)
                {
                    leavingCars = leavingCars.Append(car).ToArray();
                    car.hasWaited = true;
                    Console.WriteLine("A vehicle has left " + car.type);
                    i++;
                }
            }
        }

    }
}