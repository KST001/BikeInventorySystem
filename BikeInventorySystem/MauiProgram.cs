using Microsoft.AspNetCore.Components.WebView.Maui;
using BikeInventorySystem.Models;
using BikeInventorySystem.Services;
using MudBlazor.Services;

namespace BikeInventorySystem;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		

		return builder.Build();
	}
}
