using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Owleye.Controllers
{
    public abstract class BaseModel
    {
        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get
            {
                return _errors;
            }

        }

        public virtual List<string> Validate<T>(T model) where T : BaseModel
        {
            var tModel = typeof(T);
            foreach (var property in tModel.GetProperties())
            {
                var attributes = property.GetCustomAttributes(false);
                Attribute[] attrs = Attribute.GetCustomAttributes(property);
                foreach (var item in attrs)
                {
                    if (item.GetType() == typeof(RequiredAttribute))
                    {
                        ValidateRequiredError(property, model);
                    }

                    if (item.GetType() == typeof(StringLengthAttribute))
                    {
                        var attrib = item as StringLengthAttribute;
                        ValidateLengthError(property, model, attrib.MinimumLength, attrib.MaximumLength);
                    }

                    if (item.GetType() == typeof(EmailAddressAttribute))
                    {
                        ValidateEmailAddress(property, model);
                    }

                }
            }

            return _errors;
        }

        private void ValidateRequiredError<T>(PropertyInfo property, T model) where T : BaseModel
        {
            if (string.IsNullOrEmpty(property.GetValue(model)?.ToString()))
            {
                _errors.Add($"{property.Name} is required");
            }
        }

        private void ValidateEmailAddress<T>(PropertyInfo property, T model) where T : BaseModel
        {
            string value = property.GetValue(model)?.ToString().Trim();

            if (string.IsNullOrEmpty(value))
                _errors.Add($"{property.Name} is not valid email address.");

            var attrib = new EmailAddressAttribute();
            if (attrib.IsValid(value) == false)
            {
                _errors.Add($"{property.Name} is not valid email address.");
            }

        }

        private void ValidateLengthError<T>(PropertyInfo property, T model, int minlength, int maxLength) where T : BaseModel
        {
            string value = property.GetValue(model)?.ToString().Trim();

            if (value?.Length < minlength)
            {
                _errors.Add($"{property.Name} MinLength is {minlength} , current length is {value.Length} ");
            }

            if (value?.Length > maxLength)
            {
                _errors.Add($"{property.Name} MaxLength is {minlength} , current length is {value.Length} ");
            }
        }

        private void MobileNumber<T>(PropertyInfo property, T model) where T : BaseModel
        {
            string value = property.GetValue(model)?.ToString().Trim();

            if (string.IsNullOrEmpty(value))
                _errors.Add($"{property.Name} is not valid email address.");


            const string pattern = @"^09[0|1|2|3][0-9]{8}$";
            Regex reg = new Regex(pattern);


            Console.WriteLine(reg.IsMatch("0919111662"));

            var attrib = new EmailAddressAttribute();
            if (attrib.IsValid(value) == false)
            {
                _errors.Add($"{property.Name} is not valid email address.");
            }

        }
    }
}