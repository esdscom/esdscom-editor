using eSDSCom.Editor.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;


Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTUzOTI0QDMxMzkyZTM0MmUzMGtZT2k3Tk5TWVBpNWkzY1Y4NEtRQkM1UnZyYldXR2RUQzlxRkZOK0ZhYjQ9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
