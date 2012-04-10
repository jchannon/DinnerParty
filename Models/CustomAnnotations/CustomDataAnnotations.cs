using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Nancy.Validation;
using Nancy.Validation.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;

namespace DinnerParty.Models.CustomAnnotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MatchAttribute : ValidationAttribute
    {
        public String SourceProperty { get; set; }
        public String MatchProperty { get; set; }

        public MatchAttribute(string source, string match)
        {
            SourceProperty = source; 
            MatchProperty = match;
        }

        //public string FormatErorrMessage { get { return this.ErrorMessage; } }
        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessage;
        }
        

        public override Boolean IsValid(Object value)
        {
            if (value == null)
                return false;

            Type objectType = value.GetType();

            PropertyInfo[] properties = objectType.GetProperties();

            object sourceValue = new object();
            object matchValue = new object();

            Type sourceType = null;
            Type matchType = null;

            int counter = 0;

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.Name == SourceProperty || propertyInfo.Name == MatchProperty)
                {
                    if (counter == 0)
                    {
                        sourceValue = propertyInfo.GetValue(value, null);
                        sourceType = propertyInfo.GetValue(value, null).GetType();
                    }
                    if (counter == 1)
                    {
                        matchValue = propertyInfo.GetValue(value, null);
                        matchType = propertyInfo.GetValue(value, null).GetType();
                    }
                    counter++;
                    if (counter == 2)
                    {
                        break;
                    }
                }
            }

            if (sourceType != null && matchType != null)
            {
                //if (Convert.ChangeType(sourceValue, sourceType) == Convert.ChangeType(matchValue, matchType))
                return String.Equals(sourceValue.ToString(), matchValue.ToString());
            }
            return false;
        }
    }

   


}