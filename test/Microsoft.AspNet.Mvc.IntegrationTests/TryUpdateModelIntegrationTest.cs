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
    public class TryUpdateModelIntegrationTest
    {
        private class Address
        {
            public string Street { get; set; }
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_ExistingModel_EmptyPrefix_GetsOverWritten()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Address { Street = "DefaultStreet" };
            var oldModel = model;

            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.Same(oldModel, model);
            Assert.Equal("SomeStreet", model.Street);

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
        public async Task TryUpdateModel_ExistingModel_EmptyPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Address();
            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.Equal("SomeStreet", model.Street);

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

        private class Person2
        {
            public List<Address> Address { get; set; }
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_SettableCollectionModel_EmptyPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person2();
            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);
            Assert.Equal(1, model.Address.Count);
            Assert.Equal("SomeStreet", model.Address[0].Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "Address[0].Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        private class Person3
        {
            public Person3()
            {
                Address = new List<Address>();
            }

            public List<Address> Address { get; }
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_NonSettableCollectionModel_EmptyPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person3();
            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);
            Assert.Equal(1, model.Address.Count);
            Assert.Equal("SomeStreet", model.Address[0].Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "Address[0].Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        private class Person4
        {
            public Address[] Address { get; set; }
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_SettableArrayModel_EmptyPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person4();
            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);
            Assert.Equal(1, model.Address.Count());
            Assert.Equal("SomeStreet", model.Address[0].Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "Address[0].Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        private class Person5
        {
            public Address[] Address { get; } = new Address[] { };
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_NonSettableArrayModel_EmptyPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person5();
            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);

            // Arrays should not be updated.
            Assert.Equal(0, model.Address.Count());

            // ModelState
            Assert.True(modelState.IsValid);
            Assert.Empty(modelState.Keys);
        }


        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_ExistingModel_WithPrefix_GetsOverWritten()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("prefix.Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Address { Street = "DefaultStreet" };
            var oldModel = model;

            // Act
            var result = await TryUpdateModel(model, "prefix", operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.Same(oldModel, model);
            Assert.Equal("SomeStreet", model.Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "prefix.Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_ExistingModel_WithPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("prefix.Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Address();
            // Act
            var result = await TryUpdateModel(model, "prefix", operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.Equal("SomeStreet", model.Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "prefix.Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_SettableCollectionModel_WithPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("prefix.Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person2();
            // Act
            var result = await TryUpdateModel(model, string.Empty, operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);
            Assert.Equal(1, model.Address.Count);
            Assert.Equal("SomeStreet", model.Address[0].Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "prefix.Address[0].Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_NonSettableCollectionModel_WithPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("prefix.Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person3();
            // Act
            var result = await TryUpdateModel(model, "prefix", operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);
            Assert.Equal(1, model.Address.Count);
            Assert.Equal("SomeStreet", model.Address[0].Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "prefix.Address[0].Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_SettableArrayModel_WithPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("prefix.Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person4();
            // Act
            var result = await TryUpdateModel(model, "prefix", operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);
            Assert.Equal(1, model.Address.Count());
            Assert.Equal("SomeStreet", model.Address[0].Street);

            // ModelState
            Assert.True(modelState.IsValid);

            Assert.Equal(1, modelState.Keys.Count);
            var key = Assert.Single(modelState.Keys, k => k == "Address[0].Street");
            Assert.NotNull(modelState[key].Value);
            Assert.Equal("SomeStreet", modelState[key].Value.AttemptedValue);
            Assert.Equal("SomeStreet", modelState[key].Value.RawValue);
            Assert.Empty(modelState[key].Errors);
            Assert.Equal(ModelValidationState.Valid, modelState[key].ValidationState);
        }

        [Fact(Skip = "Extra entries in model state dictionary. #2466")]
        public async Task TryUpdateModel_NonSettableArrayModel_WithPrefix_GetsBound()
        {
            // Arrange
            var operationContext = ModelBindingTestHelper.GetOperationBindingContext(request =>
            {
                request.QueryString = new QueryString("prefix.Address[0].Street", "SomeStreet");
            });

            var modelState = new ModelStateDictionary();
            var model = new Person5();
            // Act
            var result = await TryUpdateModel(model, "prefix", operationContext, modelState);

            // Assert
            Assert.True(result);

            // Model
            Assert.NotNull(model.Address);

            // Arrays should not be updated.
            Assert.Equal(0, model.Address.Count());

            // ModelState
            Assert.True(modelState.IsValid);
            Assert.Empty(modelState.Keys);
        }

        private Task<bool> TryUpdateModel(
            object model,
            string prefix,
            OperationBindingContext operationContext,
            ModelStateDictionary modelState)
        {
           return ModelBindingHelper.TryUpdateModelAsync(
               model,
               model.GetType(),
               prefix,
               operationContext.HttpContext,
               modelState,
               operationContext.MetadataProvider,
               operationContext.ModelBinder,
               operationContext.ValueProvider,
               ModelBindingTestHelper.GetObjectValidator(),
               operationContext.ValidatorProvider);
        }
    }
}