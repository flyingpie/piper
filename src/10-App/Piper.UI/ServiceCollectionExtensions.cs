using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace Piper.UI;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPiper(this IServiceCollection services)
	{
		Guard.Against.Null(services);

		return services.AddBlazorDiagram().AddRadzenComponents();
	}
}
