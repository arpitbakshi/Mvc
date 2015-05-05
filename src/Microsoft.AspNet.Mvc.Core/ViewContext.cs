// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// Context for view execution.
    /// </summary>
    public class ViewContext : ActionContext
    {
        // We need a default FormContext if the user uses html <form> instead of an MvcForm
        private readonly FormContext _defaultFormContext = new FormContext();

        private FormContext _formContext;
        private DynamicViewData _viewBag;

        /// <summary>
        /// Creates an empty <see cref="ViewContext"/>.
        /// </summary>
        /// <remarks>
        /// The default constructor is provided for unit test purposes only.
        /// </remarks>
        public ViewContext()
        {
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), ModelState);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ViewContext"/>.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/>.</param>
        /// <param name="view">The <see cref="IView"/> being rendered.</param>
        /// <param name="viewData">The <see cref="ViewDataDictionary"/>.</param>
        /// <param name="tempData">The <see cref="ITempDataDictionary"/>.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to render output to.</param>
        public ViewContext(
            [NotNull] ActionContext actionContext,
            [NotNull] IView view,
            [NotNull] ViewDataDictionary viewData,
            [NotNull] ITempDataDictionary tempData,
            [NotNull] TextWriter writer,
            [NotNull] HtmlHelperOptions htmlHelperOptions)
            : base(actionContext)
        {
            View = view;
            ViewData = viewData;
            TempData = tempData;
            Writer = writer;
            var config = htmlHelperOptions;

            _formContext = _defaultFormContext;
            ClientValidationEnabled = config.ClientValidationEnabled;
            Html5DateRenderingMode = config.Html5DateRenderingMode;
            ValidationSummaryMessageElement = config.ValidationSummaryMessageElement;
            ValidationMessageElement = config.ValidationMessageElement;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ViewContext"/>.
        /// </summary>
        /// <param name="viewContext">The <see cref="ViewContext"/> to copy values from.</param>
        /// <param name="view">The <see cref="IView"/> being rendered.</param>
        /// <param name="viewData">The <see cref="ViewDataDictionary"/>.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to render output to.</param>
        public ViewContext(
            [NotNull] ViewContext viewContext,
            [NotNull] IView view,
            [NotNull] ViewDataDictionary viewData,
            [NotNull] TextWriter writer)
            : base(viewContext)
        {
            _formContext = viewContext.FormContext;
            ClientValidationEnabled = viewContext.ClientValidationEnabled;
            Html5DateRenderingMode = viewContext.Html5DateRenderingMode;
            ValidationSummaryMessageElement = viewContext.ValidationSummaryMessageElement;
            ValidationMessageElement = viewContext.ValidationMessageElement;

            View = view;
            ViewData = viewData;
            TempData = viewContext.TempData;
            Writer = writer;
        }

        /// <summary>
        /// Gets or sets the <see cref="Mvc.FormContext"/> for the form element being rendered.
        /// A default context is returned if no form is currently being rendered.
        /// </summary>
        public virtual FormContext FormContext
        {
            get
            {
                return _formContext;
            }
            set
            {
                // Never return a null form context, this is important for validation purposes.
                _formContext = value ?? _defaultFormContext;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether client-side validation is enabled.
        /// </summary>
        public bool ClientValidationEnabled { get; set; }

        /// <summary>
        /// Set this property to <see cref="Html5DateRenderingMode.Rfc3339" /> to have templated helpers such as
        /// <see cref="IHtmlHelper.Editor" /> and <see cref="IHtmlHelper{TModel}.EditorFor" /> render date and time
        /// values as RFC 3339 compliant strings. By default these helpers render dates and times using the current
        /// culture.
        /// </summary>
        public Html5DateRenderingMode Html5DateRenderingMode { get; set; }

        /// <summary>
        /// Element name used to wrap a top-level message generated by <see cref="IHtmlHelper.ValidationSummary"/> and
        /// other overloads.
        /// </summary>
        public string ValidationSummaryMessageElement { get; set; }

        /// <summary>
        /// Element name used to wrap a top-level message generated by <see cref="IHtmlHelper.ValidationMessage"/> and
        /// other overloads.
        /// </summary>
        public string ValidationMessageElement { get; set; }

        /// <summary>
        /// Gets the dynamic view bag.
        /// </summary>
        public dynamic ViewBag
        {
            get
            {
                if (_viewBag == null)
                {
                    _viewBag = new DynamicViewData(() => ViewData);
                }

                return _viewBag;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IView"/> currently being rendered, if any.
        /// </summary>
        public IView View { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewDataDictionary"/>.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITempDataDictionary"/> instance.
        /// </summary>
        public ITempDataDictionary TempData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TextWriter"/> used to write the output.
        /// </summary>
        public TextWriter Writer { get; set; }

        /// <summary>
        /// Gets or sets the path of the view file currently being rendered.
        /// </summary>
        /// <remarks>
        /// The rendering of a view may involve one or more files (e.g. _ViewStart, Layouts etc).
        /// This property contains the path of the file currently being rendered.
        /// </remarks>
        public string ExecutingFilePath { get; set; }

        public FormContext GetFormContextForClientValidation()
        {
            return (ClientValidationEnabled) ? FormContext : null;
        }
    }
}
