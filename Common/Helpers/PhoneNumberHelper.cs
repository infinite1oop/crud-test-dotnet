using PhoneNumbers;

namespace Common.Helpers
{
    public static class PhoneNumberHelper
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            bool result = false;
            phoneNumber = phoneNumber.Trim();
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            if (phoneNumber.StartsWith("00"))
            {
                phoneNumber = "+" + phoneNumber.Remove(0, 2);
            }
            try
            {
                PhoneNumber number = phoneUtil.Parse(phoneNumber, null);
                var type = phoneUtil.GetNumberType(number);
                if (type == PhoneNumberType.MOBILE)
                {
                    result = phoneUtil.IsValidNumber(number);
                }
                return result;
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
        public static ulong? ConvertPhoneNumberToULong(string phoneNumberStr)
        {
            if (phoneNumberStr is null)
                return null;
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumber number = phoneUtil.Parse(phoneNumberStr, null);
                return ulong.Parse(number.CountryCode.ToString() + number.NationalNumber.ToString());
            }
            catch (NumberParseException)
            {
                throw new ArgumentException("Invalid phone number format.");
            }
        }
    }
}
