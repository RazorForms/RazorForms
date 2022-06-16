﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorForms.Options;
using RazorForms.TagHelpers;
using RazorForms.TagHelpers.Inputs;

namespace RazorForms.Generators.Inputs;

public class TextAreaGenerator : ValidityAwareOutputGenerator<IFormComponentWithValidationOptions>, ITextAreaGenerator
{
	/// <inheritdoc />
	public override Task<TagHelperOutput> GenerateOutput(TagHelperContext context, 
	                                                     RazorFormsTagHelperBase helper, 
	                                                     TagHelperAttributeList? attributes = null, 
	                                                     TagHelperContent? childContent = null)
	{
		ThrowIfNotInitialized();

		if (helper is not TextAreaInputTagHelper localHelper)
		{
			throw new ArgumentException($"{nameof(helper)} must be an instance of {typeof(TextAreaInputTagHelper)}");
		}

		attributes ??= new TagHelperAttributeList();

		var textAreaHelper = new TextAreaTagHelper(localHelper.Generator)
		{
			ViewContext = localHelper.ViewContext,
			For = localHelper.For
		};

		var output = new TagHelperOutput(tagName: "textarea",
		                                 attributes: new TagHelperAttributeList(attributes),
		                                 getChildContentAsync: DefaultTagHelperContent);

		ApplyBaseClasses(output);

		textAreaHelper.Process(context, output);

		var result = Options.RemoveWrappers ?? false
			             ? output
			             : GenerateWrapper(output);

		return Task.FromResult(result);
	}

	protected virtual void ApplyBaseClasses(TagHelperOutput output)
	{
		ThrowIfNotInitialized();

		ApplyClasses(output,
		             Options.InputClasses,
		             Options.InputValidClasses,
		             Options.InputErrorClasses);
	}

	/// <inheritdoc />
	protected override void ApplyWrapperClasses(TagHelperOutput output)
	{
		ThrowIfNotInitialized();

		ApplyClasses(output,
		             Options.InputWrapperClasses,
		             Options.InputWrapperValidClasses,
		             Options.InputWrapperErrorClasses);
	}
}