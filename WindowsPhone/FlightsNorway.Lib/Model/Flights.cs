﻿using System;
namespace FlightsNorway.Lib.Model
{
    public class Flight
    {
        public string FlightId { get; set; }
        public Airline Airline { get; set; }
        public Airport Airport { get; set; }
        public FlightStatus FlightStatus { get; set; }
        public DateTime ScheduledTime { get; set; }
        public Direction Direction { get; set; }
        public string Gate { get; set; }
        public string Belt { get; set; }

        public Flight()
            : this(new Airline(), new Airport())
        {
        }

        public Flight(Airline airline, Airport airport)
        {
            Airline = airline;
            Airport = airport;
            FlightStatus = new FlightStatus();
        }

        public override string ToString()
        {
            if (Direction == Direction.Arrival)
            {
                return
                    string.Format("{0} from {1} arriving at {2}", FlightId, Airport.Name,
                                        ScheduledTime.ToShortTimeString());
            }
            else
            {
                return
                    string.Format("{0} to {1} departing at {2}", FlightId, Airport.Name,
                                        ScheduledTime.ToShortTimeString());
            }
        }

    }

    public enum Direction
    {
        Arrival,
        Depature
    }
}
