using System;

namespace StorageMangler.Domain.Utilities
{
    public interface IDateTimeFactory
    {
        DateTimeOffset ConstructNowWithOffset();
    }

    public class DateTimeFactory : IDateTimeFactory
    {
 
        public DateTimeOffset ConstructNowWithOffset()
        {
            return DateTimeOffset.Now;
        }
    
    }    
}

