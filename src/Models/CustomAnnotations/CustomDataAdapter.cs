using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using Nancy.Validation;

namespace DinnerParty.Models.CustomAnnotations
{
    public class CustomDataAdapter : IDataAnnotationsValidatorAdapter
    {
        protected readonly ValidationAttribute attribute;

        public CustomDataAdapter(MatchAttribute attribute)
        {
            this.attribute = attribute;
        }

        public IEnumerable<ModelValidationRule> GetRules()
        {
            yield return new ModelValidationRule("custom", attribute.FormatErrorMessage,
                new[] { ((MatchAttribute)attribute).SourceProperty });
        }

        public IEnumerable<ModelValidationError> Validate(object instance)
        {
            var context =
                new ValidationContext(instance, null, null)
                {
                    MemberName = ((MatchAttribute)attribute).SourceProperty
                };

            var result =
         attribute.GetValidationResult(instance, context);

            if (result != null)
            {
                yield return new ModelValidationError(result.MemberNames, attribute.FormatErrorMessage);
            }

            yield break;
        }

    }
}