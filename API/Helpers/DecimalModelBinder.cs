using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Helpers
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None && !string.IsNullOrEmpty(valueProviderResult.FirstValue))
            {
                if (decimal.TryParse(valueProviderResult.FirstValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
                {
                    bindingContext.Result = ModelBindingResult.Success(decimalValue);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid decimal value.");
                }
            }

            return Task.CompletedTask;
        }
    }
}