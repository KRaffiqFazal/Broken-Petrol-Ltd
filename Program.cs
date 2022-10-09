using System;
using System.Timers;


namespace Broken_Petrol_Ltd
{     
    class Car
    {
        public String type;
        public String fuelType;
        public int maxFuelCapacity;
        public int fuelInTank;
        public System.Timers.Timer fuellingTime;
        public int fuellingTimeINT; // the same value as fuellingTime but in an int so maths can be done on the value
        public bool isFuelling;
        public System.Timers.Timer waitingTime;
        public bool hasWaited;
        public bool hasFuelled;
        public int atPump;
        public Car()
        {
            String[] model = { "Car", "Van", "HGV" }; //array of car types available
            int[] fuelSizes = { 50, 80, 150 };        //array of fuel capacities available
            String[] fuelTypes = { "Unleaded", "LPG", "Diesel" };
            Random rnd = new Random();
            int newType = rnd.Next(3); //creates a random value between 0 and 3 (not including 3), will be used to create a car type and ensure it can have correct fuel capacity
            type = model[newType];
            maxFuelCapacity = fuelSizes[newType];
            if (type.Equals("HGV"))
            {
                fuelType = fuelTypes[2];
            }
            else if (type.Equals("Van"))
            {
                fuelType = fuelTypes[rnd.Next(1, 3)];

            }
            else
            {
                fuelType = fuelTypes[rnd.Next(3)];
            }
            fuelInTank = rnd.Next(5, maxFuelCapacity / 2); //the fuel in their tank which has a maximum of less than half of the max fuel capacity
            int temp = Convert.ToInt32(Math.Floor((maxFuelCapacity - fuelInTank) / 1.5)); //ensures that does not round above the max fuel capacity and makes it an integer from double
            fuellingTimeINT = rnd.Next(1, temp); //the length of time it takes to fuel is found by working out the time it would take to fully fuel and using that as the upper parameter in a Random.Next() method
            fuellingTime = new(fuellingTimeINT * 1000);
            fuellingTime.Elapsed += FuellingTime_Elapsed;
            fuellingTime.AutoReset = false;
            fuellingTime.Enabled = true;
            waitingTime = new(rnd.Next(8000, 9000)); //waiting time is now a timer that is unique to each object and so will tick down until it triggers the elapsed event function
            waitingTime.Elapsed += WaitingTime_Elapsed;
            waitingTime.AutoReset = false;
            waitingTime.Enabled = true;
            isFuelling = false;
            hasWaited = false;
            hasFuelled = false;
            atPump = 0;
        }

        public static Car[] existingCars = { }; //cars that exist in the petrol station, can be waiting or fuelling.
        public static Car newCar = new Car(); //variable for new vehicle to be appended into above array
        public static Car[] leavingCars = { }; //contains vehicles that have left BEFORE fuelling
        public static Car[] finishedFuellingCars = { }; //contains vehicle that have left AFTER fuelling
        public static int carCounter = 0;

        public static Pump[] lane1 = { new Pump(1), new Pump(2), new Pump(3) }; //each array of lane contains 3 pumps, this will help to signify if a vehicle is unable to fuel if a pump is in the way
        public static Pump[] lane2 = { new Pump(4), new Pump(5), new Pump(6) };
        public static Pump[] lane3 = { new Pump(7), new Pump(8), new Pump(9) };
        public static Pump[][] allPumps = { lane1, lane2, lane3 };

        public static Random intervalTime = new Random();
        public static System.Timers.Timer carCreator = new(intervalTime.Next(1500, 2201)); //creates a car every 1500-2201ms, properties of timer in Main method
        public static System.Timers.Timer assigner = new(100); //a timer that will check every 100ms to see if it can allocate a car a pump, properties in Main method
     

        public static void Main(string[] args)
        {
            carCreator.Elapsed += CarCreator_Elapsed;
            carCreator.Enabled = true;
            carCreator.AutoReset = true;
            carCreator.Start();
            assigner.Elapsed += Assigner_Elapsed;
            assigner.Enabled = true;
            assigner.AutoReset = true;
            assigner.Start();
            Console.ReadKey();
            carCreator.Stop();
            Console.WriteLine("All the vehicles that have left are: ");
            foreach (Car car in leavingCars)
            {
                Console.WriteLine(car.type);
            }
            foreach (Car car in finishedFuellingCars)
            {
                Console.WriteLine($"A {car.type} fuelled at pump {car.atPump} for {car.fuellingTimeINT} seconds and then left: {car.hasFuelled}");
            }
            foreach (Pump[] lane in allPumps)
            {
                foreach (Pump pump in lane)
                {
                    Console.WriteLine($"Pump number: {pump.pumpNumber} provided {pump.lpgDispensed} litres of LPG, {pump.dieselDispensed} litres of Diesel and {pump.unleadedDispensed} litres of Unleaded.");
                }
            }

        }

        private static void Assigner_Elapsed(object? sender, ElapsedEventArgs e)
        {
            foreach (Car car in existingCars)
            {
                if (car.hasWaited == false && car.isFuelling == false) //checks to find cars that are not fuelling and are still waiting
                {
                    foreach (Pump[] lane in allPumps) //loops through the jagged array of each lane
                    {
                        if (lane[2].inUse == false && lane[1].inUse == false && lane[0].inUse == false) //checks to see if the farthest pump is free and the route there is clear too
                        {
                            car.isFuelling = true; //fuel in farthest pump on the lane
                            lane[2].inUse = true;
                            car.atPump = lane[2].pumpNumber;
                            car.fuellingTime.Start();
                            
                        }
                        else if (lane[2].inUse == true && lane[1].inUse == false && lane[0].inUse == false) //checks to see if the middle pump is free if the farthest is not free
                        {
                            lane[1].inUse = true;
                            car.isFuelling = true; //fuel in middle pump on this lane
                            car.atPump = lane[1].pumpNumber;
                            car.fuellingTime.Start();
                        }
                        else if (lane[1].inUse == true && lane[0].inUse == false) //checks to see if the first pump is free if the middle pump is not free
                        {
                            car.isFuelling = true; //fuel in first pump on this lane
                            car.atPump = lane[0].pumpNumber;
                            car.fuellingTime.Start();
                            lane[0].inUse = true;
                        }
                    }
                }
            }
        }

        private static void CarCreator_Elapsed(object sender, ElapsedEventArgs e) //creates vehicles
        {
            int i = 0;
            foreach (Car car in existingCars) //given that the existingCars array will include the cars that have left, I need to work out which cars have "left" via the boolean and take them away
            {
                if (car.hasWaited == true)
                {
                    i++;
                }
            }
            if (existingCars.Length - i <= 7) //cars cannot be created if they surpass the queue
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
            foreach (Car car in existingCars)
            {
                if (car.waitingTime.Enabled == false && !car.hasWaited)
                {
                    leavingCars = leavingCars.Append(car).ToArray();
                    car.hasWaited = true;
                    Console.WriteLine("A vehicle has left " + car.type);
                }
            }
        }
        private void FuellingTime_Elapsed(object? sender, ElapsedEventArgs e)
        {
            foreach (Car car in existingCars)
            {
                if (car.fuellingTime.Enabled == false && car.isFuelling && car.hasFuelled == false) //if a car has finished fuelling, this function is called but to check if the car is also currently fuelling and not waiting to be fuelled, the bool must be checked too
                {
                    finishedFuellingCars = finishedFuellingCars.Append(car).ToArray();
                    car.hasFuelled = true;
                    foreach (Pump[] lane in allPumps)
                    {
                        foreach (Pump pump in lane)
                        {
                            if (pump.pumpNumber == car.atPump)
                            {
                                pump.inUse = false;
                                
                                if (car.fuelType.Equals("LPG"))
                                {
                                    pump.lpgDispensed += car.fuellingTimeINT * 1.5;
                                }
                                else if (car.fuelType.Equals("Diesel"))
                                {
                                    pump.dieselDispensed += car.fuellingTimeINT * 1.5;
                                }
                                else
                                {
                                    pump.unleadedDispensed += car.fuellingTimeINT * 1.5;
                                }
                                Console.WriteLine($"{car.type} has finished fuelling. {pump.unleadedDispensed} of Unleaded, {pump.dieselDispensed} of Diesel, {pump.lpgDispensed} of LPG");
                            }
                        }
                    }
                }
            }
        }

    }
}