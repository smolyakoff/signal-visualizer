using Caliburn.Micro;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SignalInfoViewModel
    {
        public SignalInfoViewModel(SignalHeader header)
        {
            Properties = new BindableCollection<PropertyViewModel>(new[]
            {
                new PropertyViewModel {Label = "Количество каналов", Value = header.Channels},
                new PropertyViewModel {Label = "Размер выборки на один канал", Value = header.SampleSize},
                new PropertyViewModel {Label = "Количество спектральных линий", Value = header.SpectrumLines},
                new PropertyViewModel {Label = "Частота среза", Value = header.Frequency},
                new PropertyViewModel {Label = "Частотное разрешение", Value = header.FrequencyResolution},
                new PropertyViewModel {Label = "Частота дискретизации сигнала", Value = header.RequestedBlocks},
                new PropertyViewModel {Label = "Время приема блока данных", Value = header.BlockTime},
                new PropertyViewModel {Label = "Общее время приема даннх", Value = header.TotalTime},
                new PropertyViewModel {Label = "Число принятых блоков", Value = header.ReceivedBlocks},
                new PropertyViewModel {Label = "Общий размер данных", Value = header.Ticks},
                new PropertyViewModel {Label = "Максимальное значение принятых данных", Value = header.MaxValue},
                new PropertyViewModel {Label = "Минимальное значение принятых данных", Value = header.MinValue}
            });
        }

        public BindableCollection<PropertyViewModel> Properties { get; private set; }
    }
}