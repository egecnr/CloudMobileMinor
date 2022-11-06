
using Microsoft.EntityFrameworkCore;
using System;

namespace ShowerShow.Models
{
    [Flags]
    public enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
}
