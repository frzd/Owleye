using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Owleye.Controllers
{
    public abstract class BaseModel
    {
            
        public static void Validate<T>(ValidationContext context) where T : BaseModel
        {
            var model = typeof(T);
            foreach (var property in model.GetProperties())
            {

                List<ValidationResult> validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(model, context, validationResults);

                if (!isValid)
                {
                    //List of errors 
                    //validationResults.Select(r => r.ErrorMessage)
                    //return or do something
                }

                var attributes = property.GetCustomAttributes(false);
                Attribute[] attrs = Attribute.GetCustomAttributes(property);
                foreach (var item in attrs)
                {
                    var a = item.GetType();
                    if (a == typeof(RequiredAttribute))
                    {
                    }
                }
            }
        }

    }
}