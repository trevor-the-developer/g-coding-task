using GuestiaCodingTask.Data;

namespace GuestiaCodingTask.Helpers
{
    public static class NameDisplayFormatTypeHelper
    {
        public static string FormatGuestName(Guest guest)
        {
            // [Display(Name = "LASTNAME FirstName")]
            // [Display(Name = "LastName, F(irstName)")]
            switch (guest.GuestGroup.NameDisplayFormat)
            {
                case NameDisplayFormatType.UpperCaseLastNameSpaceFirstName:
                    return $"{guest.LastName.ToUpper()} {guest.FirstName}";
                case NameDisplayFormatType.LastNameCommaFirstNameInitial:
                    return $"{guest.LastName}, {guest.FirstName.Substring(0, 1).ToUpper()}";
                default:
                    return $"{guest.LastName.ToUpper()} {guest.FirstName}";
            }
        }        
    }
}