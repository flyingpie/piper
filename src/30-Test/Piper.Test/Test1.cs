using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyMSTest;
using VerifyTests;
using static VerifyMSTest.Verifier;

namespace Piper.Test;

[TestClass]
[UsesVerify]
public partial class Test1
{
	private IBrowser _browser = null!;
	private IPlaywright _playwright = null!;

	[TestInitialize]
	public async Task Setup()
	{
		_playwright = await Playwright.CreateAsync();
		_browser = await _playwright.Chromium.LaunchAsync();
	}

	[TestMethod]
	public async Task TestMethod1()
	{
		var context = await _browser.NewContextAsync(
			new()
			{
				//
				ViewportSize = new() { Width = 1920, Height = 1080 },
			}
		);

		var page = await context.NewPageAsync();
		await page.GotoAsync("http://localhost:5217");

		var btn = await page.QuerySelectorAsync(".pp-btn-load");
		await btn.ClickAsync();

		// var x = await page.QuerySelectorAsync(".rz-tabview-panels");

		await Verify(page)
			//
			.PageScreenshotOptions(new(), screenshotOnly: true)
			.UseImageHash(threshold: 99);
	}
}

public static class ModuleInit
{
	[ModuleInitializer]
	public static void Init()
	{
		VerifyImageHash.Initialize();
		VerifyPlaywright.Initialize();
	}
}
