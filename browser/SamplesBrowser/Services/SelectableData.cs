using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
public class SelectableDataItem : INotifyPropertyChanged
{
    private string _category;
    public string Category
    {
        get { return _category; }
        set { _category = value; OnPropertyChanged("Category"); }
    }
    private double _dataValue;
    public double DataValue
    {
        get { return _dataValue; }
        set { _dataValue = value; OnPropertyChanged("DataValue"); }
    }
    private double _selectedValue;
    public double SelectedValue
    {
        get { return _selectedValue; }
        set { _selectedValue = value; OnPropertyChanged("SelectedValue"); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}

public class SelectableData
    : ObservableCollection<SelectableDataItem>
{
    public SelectableData()
    {
        this.Add(new SelectableDataItem() { Category = @"2010", DataValue = 20, SelectedValue = 20 });
        this.Add(new SelectableDataItem() { Category = @"2011", DataValue = 40, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2012", DataValue = 35, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2013", DataValue = 50, SelectedValue = 50 });
        this.Add(new SelectableDataItem() { Category = @"2014", DataValue = 45, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2015", DataValue = 60, SelectedValue = 60 });
        this.Add(new SelectableDataItem() { Category = @"2016", DataValue = 35, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2017", DataValue = 40, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2018", DataValue = 50, SelectedValue = 50 });
        this.Add(new SelectableDataItem() { Category = @"2019", DataValue = 75, SelectedValue = 75 });
        this.Add(new SelectableDataItem() { Category = @"2020", DataValue = 65, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2021", DataValue = 40, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2022", DataValue = 50, SelectedValue = double.NaN });
        this.Add(new SelectableDataItem() { Category = @"2023", DataValue = 65, SelectedValue = 65 });
        this.Add(new SelectableDataItem() { Category = @"2024", DataValue = 70, SelectedValue = 70 });
        this.Add(new SelectableDataItem() { Category = @"2025", DataValue = 85, SelectedValue = double.NaN });
    }
}
