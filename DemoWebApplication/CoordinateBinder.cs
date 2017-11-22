using System;
using System.Threading.Tasks;
using DemoWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace DemoWebApplication
{
    public class CoordinateBinder : IModelBinder
    {
        private readonly IModelBinder fallbackBinder;
        public CoordinateBinder(IModelBinder fallbackBinder)
        {
            this.fallbackBinder = fallbackBinder;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                ValueProviderResult first = bindingContext.ValueProvider.GetValue("CoordinateFirst");
                if (first == ValueProviderResult.None)
                {
                    return fallbackBinder.BindModelAsync(bindingContext);
                }

                string[] tokens = first.FirstValue.Split(',');
                if (tokens.Length != 2)
                {
                    throw new ArgumentException();
                }
                // получаем значения
                string xStr = tokens[0];
                string yStr = tokens[1];

                var result = new Coordinate() {X = decimal.Parse(xStr), Y = decimal.Parse(yStr)};
                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return fallbackBinder.BindModelAsync(bindingContext);
            }
        }
    }
    public class CoordinateBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder binder =
            new CoordinateBinder(new SimpleTypeModelBinder(typeof(Coordinate)));

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(Coordinate) ? binder : null;
        }
    }
}