using Caliburn.Micro;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SignalDescriptionViewModel
    {
        public SignalDescriptionViewModel(SignalDescription description)
        {
            Properties = new BindableCollection<SignalProperty>(new []
            {
                new SignalProperty {Label = "Количество каналов", Value = description.Channels },
                new SignalProperty {Label = "Размер выборки на один канал", Value = description.SampleSize},
                new SignalProperty {Label = "Количество спектральных линий", Value = description.SpectrumLines},
                new SignalProperty {Label = "Частота среза", Value = description.Frequency},
                new SignalProperty {Label = "Частотное разрешение", Value = description.FrequencyResolution},
                new SignalProperty {Label = "Частота дискретизации сигнала", Value = description.RequestedBlocks},
                new SignalProperty {Label = "Время приема блока данных", Value = description.BlockTime},
                new SignalProperty {Label = "Общее время приема даннх", Value = description.TotalTime},
                new SignalProperty {Label = "Число принятых блоков", Value = description.ReceivedBlocks},
                new SignalProperty {Label = "Общий размер данных", Value = description.Ticks},
                new SignalProperty {Label = "Максимальное значение принятых данных", Value = description.MaxValue},
                new SignalProperty {Label = "Минимальное значение принятых данных", Value = description.MinValue}
            });
        }

        public BindableCollection<SignalProperty> Properties { get; private set; } 
    }

    public class SignalProperty
    {
        public string Label { get; set; }

        public object Value { get; set; }
    }
}
