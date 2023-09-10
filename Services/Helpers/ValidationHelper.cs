﻿using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        public static void ModelValidation(object obj) {
            // model validation
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
