using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainShare.ViewModels
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            Tabs = new List<Tab>();

            Tabs.Add(new Tab("Общие","common",true));
            Tabs.Add(new Tab("Оповещения","notifications"));
            Tabs.Add(new Tab("Приватность","privacy"));
        }
        public List<Tab> Tabs { get; set; }
    }

    public class Tab
    {
        public Tab(string title,string name, bool isActive = false)
        {
            Name = name;
            IsActive = isActive;
            Title = title;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}