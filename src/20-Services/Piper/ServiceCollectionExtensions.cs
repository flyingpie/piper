using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System;

namespace Piper;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddUI(this IServiceCollection services)
	{
		Guard.Against.Null(services);

		return services
			.AddRadzenComponents()
			// .AddHostedServiceSingleton<IWtqUIService, WtqUI>()
			;
	}
}