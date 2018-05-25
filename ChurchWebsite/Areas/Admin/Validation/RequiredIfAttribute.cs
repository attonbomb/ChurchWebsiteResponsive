using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChurchWebsite.Areas.Admin.Validation
{
    public sealed class RequiredIfAttribute : ValidationAttribute
    {
        private RequiredAttribute innerAttribute = new RequiredAttribute();
        public string DependentUpon { get; set; }
        public object Value { get; set; }
        private bool ValidationOk = true;

        public bool AllowEmptyStrings
        {
            get
            {
                return innerAttribute.AllowEmptyStrings;
            }
            set
            {
                innerAttribute.AllowEmptyStrings = value;
            }
        }

        public RequiredIfAttribute(string dependentUpon, object value)
        {
            innerAttribute.AllowEmptyStrings = true;
            this.DependentUpon = dependentUpon;
            this.Value = value;
        }

        public RequiredIfAttribute(string dependentUpon)
        {
            innerAttribute.AllowEmptyStrings = true;
            this.DependentUpon = dependentUpon;
            this.Value = null;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(DependentUpon);

            if (field != null)
            {
                // get the value of the dependent property
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                // trim spaces of dependent value
                if (dependentValue != null && dependentValue is string)
                {
                    dependentValue = (dependentValue as string).Trim();

                    if (!AllowEmptyStrings && (dependentValue as string).Length == 0)
                    {
                        dependentValue = null;
                    }
                }

                // compare the value against the target value
                if ((dependentValue == null && Value == null) ||
                    (dependentValue != null && (Value == "*" || dependentValue.Equals(Value))))
                {
                    // match => means we should try validating this field
                    if (!innerAttribute.IsValid(value))
                        // validation failed - return an error
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }

    public class RequiredIfValidator : DataAnnotationsModelValidator<RequiredIfAttribute>
    {
        string errorMsg = string.Empty;
        string dependantUpon = string.Empty;
        object value = new object();

        public RequiredIfValidator(ModelMetadata metadata, ControllerContext context, RequiredIfAttribute attribute)
            : base(metadata, context, attribute)
        {
            errorMsg = attribute.ErrorMessage;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule ValidationRule = new ModelClientValidationRule();
            ValidationRule.ErrorMessage = errorMsg;
            ValidationRule.ValidationType = "RequiredIfAttribute";
            ValidationRule.ValidationParameters.Add("DependantUpon", dependantUpon);
            ValidationRule.ValidationParameters.Add("Value", value);
            return new[] { ValidationRule };
        }
    }
}