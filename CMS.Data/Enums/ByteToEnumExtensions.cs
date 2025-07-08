using CMS.Data.Enums.Authen;

namespace CMS.Data.Enums
{
    public static class ByteToEnumExtensions
    {

        public static GenderEnum GetGenderEnum(this byte value)
        {
            Enum.TryParse(value.ToString(), out GenderEnum st);
            return st;
        }
        public static UserLogActionEnum GetUserLogActionEnum(this int value)
        {
            _ = Enum.TryParse(value.ToString(), out UserLogActionEnum st);
            return st;
        }
        public static UserLogStatusEnum GetUserLogStatusEnum(this byte value)
        {
            _ = Enum.TryParse(value.ToString(), out UserLogStatusEnum st);
            return st;
        }
    }
}
