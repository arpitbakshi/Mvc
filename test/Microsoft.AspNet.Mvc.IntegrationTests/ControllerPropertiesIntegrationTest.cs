// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.ModelBinding;
using Xunit;

namespace Microsoft.AspNet.Mvc.IntegrationTests
{
    public class ControllerPropertiesIntegrationTest
    {
        private class Address
        {
            public string Street { get; set; }
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task ControllerProperty_WithPrefix_GetsBound()
        {
            // Arrange
            var argumentBinder = ModelBindingTestHelper.GetArgumentBinder();
            var parameter = new ParameterDescriptor()
            {
                Name = "Address",
                ParameterType = typeof(Address)
            };

            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Address.Street", "SomeStreet");
            });


            var modelState = new ModelStateDictionary();

            // Act
            var modelBindingResult = await argumentBinder.BindModelAsync(parameter, modelState, operationContext);

            // Assert

            // ModelBindingResult
            Assert.NotNull(modelBindingResult);
            Assert.True(modelBindingResult.IsModelSet);

            // Model
            var boundAddress = Assert.IsType<Address>(modelBindingResult.Model);
            Assert.NotNull(boundAddress);
            Assert.Equal("SomeStreet", boundAddress.Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "Address.Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task ControllerProperty_EmptyPrefix_GetsBound()
        {
            // Arrange
            var argumentBinder = ModelBindingTestHelper.GetArgumentBinder();
            var parameter = new ParameterDescriptor()
            {
                Name = "Address",
                ParameterType = typeof(Address)
            };

            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Street", "SomeStreet");
            });


            var modelState = new ModelStateDictionary();

            // Act
            var modelBindingResult = await argumentBinder.BindModelAsync(parameter, modelState, operationContext);

            // Assert

            // ModelBindingResult
            Assert.NotNull(modelBindingResult);
            Assert.True(modelBindingResult.IsModelSet);

            // Model
            var boundAddress = Assert.IsType<Address>(modelBindingResult.Model);
            Assert.NotNull(boundAddress);
            Assert.Equal("SomeStreet", boundAddress.Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task ControllerProperty_ValueType_EmptyPrefix_GetsBound()
        {
            // Arrange
            var argumentBinder = ModelBindingTestHelper.GetArgumentBinder();
            var parameter = new ParameterDescriptor()
            {
                Name = "Address",
                ParameterType = typeof(int)
            };

            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("", "123");
            });

            var modelState = new ModelStateDictionary();

            // Act
            var result = await argumentBinder.BindModelAsync(parameter, modelState, operationContext);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsModelSet);

            // Model
            Assert.Equal(123, result.Model);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("123", modelState[key].Value.AttemptedValue);
            Assert.Equal(123, modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }
    }
}