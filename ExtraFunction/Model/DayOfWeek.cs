
using Microsoft.EntityFrameworkCore;
using System;

namespace ExtraFunction.Model
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
