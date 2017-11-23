using System;
using System.Globalization;
using System.Threading.Tasks;
using DemoWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace DemoWebApplication
{
    public class CoordinateBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;
        public CoordinateBinder(IModelBinder fallbackBinder)
        {
            _fallbackBinder = fallbackBinder;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                string bindingFieldName = bindingContext.FieldName;
                ValueProviderResult first = bindingContext.ValueProvider.GetValue(bindingFieldName);
                if (first == ValueProviderResult.None)
                {
                    return _fallbackBinder.BindModelAsync(bindingContext);
                }

                string[] tokens = first.FirstValue.Split(',');
                if (tokens.Length != 2)
                {
                    throw new ArgumentException();
                }

                string xStr = tokens[0];
                string yStr = tokens[1];
                var x = decimal.Parse(xStr, CultureInfo.InvariantCulture);
                var y = decimal.Parse(yStr, CultureInfo.InvariantCulture);

                var result = new Coordinate() {X = x, Y =  y};
                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return _fallbackBinder.BindModelAsync(bindingContext);
            }
        }
    }

    public class CoordinateBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder _binder =
            new CoordinateBinder(new SimpleTypeModelBinder(typeof(Coordinate)));

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(Coordinate) ? _binder : null;
        }
    }
}