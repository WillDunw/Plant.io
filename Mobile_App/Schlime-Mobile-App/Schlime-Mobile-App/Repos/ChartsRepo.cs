using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Schlime_Mobile_App.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Repos
{
    public static class ChartsRepo
    {
        public static CartesianChart GetTemperatureChart(ObservableCollection<AReading> temperatures)
        {
            LineSeries<DateTimePoint>[] series =
            {
                new LineSeries<DateTimePoint>
                {
                    Name = "Temperature",
                    Values = temperatures.Select(x => new DateTimePoint(x.Time, x.Value)),
                    Stroke = new SolidColorPaint(SKColor.Parse("#5375CB")),
                    GeometrySize = 0,
                    GeometryStroke = null,
                    LineSmoothness = 0.65,
                }
            };

            Axis[] yAxes = { new Axis { MinLimit = 0, MaxLimit = 60 } };
            Axis[] xAxes = { new DateTimeAxis(TimeSpan.FromDays(1), date => $"{date.ToString("MM-dd")}") };

            LabelVisual labelVisual = new LabelVisual
            {
                Text = "Temperature Trend",
                TextSize = 18,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            return new CartesianChart
            {
                Series = series,
                YAxes = yAxes,
                XAxes = xAxes,
                Title = labelVisual
            };
        }

        public static CartesianChart GetMoistureChart(ObservableCollection<AReading> moistures)
        {
            LineSeries<DateTimePoint>[] series =
            {
                new LineSeries<DateTimePoint>
                {
                    Name = "Moisture",
                    Values = moistures.Select(x => new DateTimePoint(x.Time, x.Value)),
                    Stroke = new SolidColorPaint(SKColor.Parse("#5375CB")),
                    GeometrySize = 0,
                    GeometryStroke = null,
                    LineSmoothness = 0.65,
                }
            };

            Axis[] yAxes = { new Axis { MinLimit = 0, MaxLimit = 100 } };
            Axis[] xAxes = { new DateTimeAxis(TimeSpan.FromDays(1), date => $"{date.ToString("MM-dd")}") };

            LabelVisual labelVisual = new LabelVisual
            {
                Text = "Moisture Trend",
                TextSize = 18,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            return new CartesianChart
            {
                Series = series,
                YAxes = yAxes,
                XAxes = xAxes,
                Title = labelVisual
            };
        }

        public static CartesianChart GetHumidityChart(ObservableCollection<AReading> humidities)
        {
            LineSeries<DateTimePoint>[] series =
            {
                new LineSeries<DateTimePoint>
                {
                    Name = "Humidity",
                    Values = humidities.Select(x => new DateTimePoint(x.Time, x.Value)),
                    Stroke = new SolidColorPaint(SKColor.Parse("#5375CB")),
                    GeometrySize = 0,
                    GeometryStroke = null,
                    LineSmoothness = 0.65,
                }
            };

            Axis[] yAxes = { new Axis { MinLimit = 0, MaxLimit = 100 } };
            Axis[] xAxes = { new DateTimeAxis(TimeSpan.FromDays(1), date => $"{date.ToString("MM-dd")}") };

            LabelVisual labelVisual = new LabelVisual
            {
                Text = "Humidity Trend",
                TextSize = 18,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            return new CartesianChart
            {
                Series = series,
                YAxes = yAxes,
                XAxes = xAxes,
                Title = labelVisual
            };
        }

        public static CartesianChart GetWaterLevelChart(ObservableCollection<AReading> waterLevels)
        {
            LineSeries<DateTimePoint>[] series =
            {
                new LineSeries<DateTimePoint>
                {
                    Name = "Water Level",
                    Values = waterLevels.Select(x => new DateTimePoint(x.Time, x.Value)),
                    Stroke = new SolidColorPaint(SKColor.Parse("#5375CB")),
                    GeometrySize = 0,
                    GeometryStroke = null,
                    LineSmoothness = 0.65,
                }
            };

            Axis[] yAxes = { new Axis { MinLimit = 0, MaxLimit = 60 } };
            Axis[] xAxes = { new DateTimeAxis(TimeSpan.FromDays(1), date => $"{date.ToString("MM-dd")}") };

            LabelVisual labelVisual = new LabelVisual
            {
                Text = "Water Level Trend",
                TextSize = 18,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            return new CartesianChart
            {
                Series = series,
                YAxes = yAxes,
                XAxes = xAxes,
                Title = labelVisual
            };
        }
    }
}
