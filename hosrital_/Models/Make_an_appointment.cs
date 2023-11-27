using System;

namespace hosrital_.Models;

public class Make_an_appointment
{
    public int Id { get; set; }
    public string Patient { get; set; }
    public string Doctor { get; set; }
    public DateTime Date_and_time_of_reception { get; set; }
}