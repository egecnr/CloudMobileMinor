
using Microsoft.EntityFrameworkCore;
using System;

namespace ShowerShow.Models
{
    [Flags]
    internal enum DayOfWeek
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
