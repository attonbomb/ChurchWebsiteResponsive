using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChurchWebsite.Binders
{
    public class TimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Ensure there's incomming data
            var key = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(key);

            if (valueProviderResult == null || string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
            {
                return null;
            }

            // Preserve it in case we need to redisplay the form
            bindingContext.ModelState.SetModelValue(key, valueProviderResult);

            // Parse
            var hours = ((string[])valueProviderResult.RawValue)[0];
            var minutes = ((string[])valueProviderResult.RawValue)[1];

            // A TimeSpan represents the time elapsed since midnight
            var time = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), 0);

            return time;
        }
    }
}