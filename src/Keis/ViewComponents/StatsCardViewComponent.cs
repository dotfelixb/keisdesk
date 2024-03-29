﻿using Keis.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Keis.ViewComponents;

public class StatsCardViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string title, int count, string extraClass = "bg-indigo-50")
    {
        // Hook to Db
        return await Task.Run(() => View(new StatsCard {Title = title, Count = count, ExtraClass = extraClass}));
    }
}