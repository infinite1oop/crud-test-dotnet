﻿using Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomizedValidationAttribute : ValidationAttribute
    {
        public enum ValidationType
        {
            PhoneNumber,
            BankAccountNumber,
            Date
        }
        private readonly ValidationType _validationType;

        #region Constants
        const string PHONENUMBER_ERRORMESSAGE = "Please Enter a Valid Phone Number";
        const string BANKACCOUNTNUMBER_ERRORMESSAGE = "Please Enter a Valid Bank Account Number";
        const string DATE_ERRORMESSAGE = "Please Enter a Valid Date";
        const string BANKACCOUNTNUMBER_PATTERN = @"^\d{6,20}$";
        #endregion

        public CustomizedValidationAttribute(ValidationType validationType)
        {
            _validationType = validationType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }
            string pattern = "";
            string errorMessage = "";
            switch (_validationType)
            {
                case ValidationType.PhoneNumber:
                    errorMessage = PHONENUMBER_ERRORMESSAGE;
                    var phoneNumber = value.ToString();
                    var isValid = PhoneNumberHelper.IsValidPhoneNumber(phoneNumber);
                    return isValid ? ValidationResult.Success : new ValidationResult(errorMessage);
                case ValidationType.BankAccountNumber:
                    pattern = BANKACCOUNTNUMBER_PATTERN;
                    errorMessage = BANKACCOUNTNUMBER_ERRORMESSAGE;
                    break;
                case ValidationType.Date:
                    errorMessage = DATE_ERRORMESSAGE;
                    if (DateHelper.IsValidIso8601Date(value.ToString()))
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(errorMessage);
                default:
                    throw new InvalidOperationException("Invalid validation type.");
            }
            if (!string.IsNullOrEmpty(pattern) && !System.Text.RegularExpressions.Regex.IsMatch(value.ToString(), pattern))
            {
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
