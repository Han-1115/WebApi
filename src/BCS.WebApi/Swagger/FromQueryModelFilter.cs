// <copyright file="QueryModelFilter.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace CityGates.Infrastructure.Swagger
{
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    /// <summary>
    /// Special operation filter to display query model in swagger
    /// </summary>
    public class FromQueryModelFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var description = context.ApiDescription;
            if (description?.HttpMethod?.ToLower() != HttpMethod.Get.ToString().ToLower())
            {
                // We only want to do this for GET requests, if this is not a
                // GET request, leave this operation as is, do not modify
                return;
            }

            var actionParameters = description.ActionDescriptor.Parameters;
            var apiParametersCount = description.ParameterDescriptions.Count(p => p.Source.IsFromRequest);

            if (actionParameters.Count == apiParametersCount)
            {
                // If no complex query parameters detected, leave this operation as is, do not modify
                //return;
            }

            operation.Parameters = CreateParameters(actionParameters, operation.Parameters.ToDictionary(p => p.Name, p => p), context);
        }

        private static IList<OpenApiParameter> CreateParameters(
            IList<ParameterDescriptor> actionParameters,
            IReadOnlyDictionary<string, OpenApiParameter> operationParameters,
            OperationFilterContext context)
        {
            return actionParameters
                .Select(p => CreateParameter(p, operationParameters, context))
                .Where(p => p != null)
                .Select(p => p!)
                .ToList();
        }

        private static OpenApiParameter? CreateParameter(
            ParameterDescriptor actionParameter,
            IReadOnlyDictionary<string, OpenApiParameter> operationParameters,
            OperationFilterContext context)
        {
            if (operationParameters.ContainsKey(actionParameter.Name))
            {
                // If parameters is defined as the action method argument, just pass it through
                return operationParameters[actionParameter.Name];
            }

            if (actionParameter.BindingInfo == null)
            {
                return null;
            }

            return new OpenApiParameter
            {
                Name = actionParameter.Name,
                In = ParameterLocation.Query,
                Schema = context.SchemaGenerator.GenerateSchema(actionParameter.ParameterType, context.SchemaRepository)
            };
        }
    }
}
